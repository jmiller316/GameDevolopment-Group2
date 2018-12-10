/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimationSystem : MonoBehaviour
    {
        [SerializeField] private float amount = 0.02f;
        [SerializeField] private float maxAmount = 0.03f;
        [SerializeField] private float smooth = 3;
        [SerializeField] private float smoothRotation = 2;
        [SerializeField] private float tiltAngle = 2;
        [SerializeField] private float staticY;
        [SerializeField] private float maxYPosJump;
        [SerializeField] private float smoothJump;
        [SerializeField] private float smoothLand;
        [SerializeField] private bool useSway;

        private Animator animator;
        private CharacterController characterController;
        private WeaponReloadSystem weaponReloadSystem;
        private PlayerInventory inventory;
        private Vector3 def;
        private Dictionary<string, bool> states;
        private bool[] stateValue;
        private string[] stateName;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            animator = GetComponent<Animator>();
            weaponReloadSystem = GetComponent<WeaponReloadSystem>();
            characterController = transform.root.GetComponent<CharacterController>();
            inventory = transform.root.GetComponent<PlayerInventory>();
            def = transform.localPosition;
            stateName = InitStates();
            stateValue = new bool[stateName.Length];
            stateValue[8] = false;
            stateValue[10] = true;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            //Walk
            if (SimpleInputManager.GetHorizontal() != 0 || SimpleInputManager.GetVertical() != 0)
            {
                if (SimpleInputManager.IsRunning() && SimpleInputManager.GetHorizontal() == 0 && SimpleInputManager.GetVertical() > 0)
                {
                    animator.SetInteger("Movement", 2);
                    stateValue[2] = true;
                    stateValue[1] = false;
                }
                else
                {
                    animator.SetInteger("Movement", 1);
                    stateValue[1] = true;
                    stateValue[2] = false;
                    stateValue[0] = false;
                }
            }
            else if (!SimpleInputManager.GetFire() && !SimpleInputManager.IsJumping() && !stateValue[6])
            {
                animator.SetInteger("Movement", 0);
                stateValue[0] = true;
                stateValue[1] = false;
            }

            //Fire
            if (SimpleInputManager.GetFireLong() && !weaponReloadSystem.BulletsIsEmpty)
            {
                stateValue[3] = true;
                animator.SetInteger("Fire", 1);
            }
            else if (SimpleInputManager.GetFire() && !weaponReloadSystem.BulletsIsEmpty)
            {
                stateValue[3] = true;
                animator.SetInteger("Fire", 2);
            }
            else if (SimpleInputManager.GetFireLong() && weaponReloadSystem.BulletsIsEmpty)
            {
                stateValue[3] = true;
                animator.SetInteger("Fire", 0);
            }
            else
            {
                stateValue[3] = true;
                animator.SetInteger("Fire", -1);
            }

            //Sight
            if (SimpleInputManager.GetAim())
            {
                stateValue[4] = true;
                animator.SetBool("Sight", true);
            }
            else if (!SimpleInputManager.GetAim())
            {
                stateValue[4] = false;
                animator.SetBool("Sight", false);
            }

            //Reload
            if (!weaponReloadSystem.ClipsIsEmpty)
            {
                if (SimpleInputManager.GetReload() && (weaponReloadSystem.BulletCount == 0))
                {
                    stateValue[5] = true;
                    animator.SetInteger("Reload", 0);
                }
                else if (SimpleInputManager.GetReload() && ((weaponReloadSystem.BulletCount > 0) && (weaponReloadSystem.BulletCount < weaponReloadSystem.MaxBulletCount)))
                {
                    stateValue[5] = true;
                    animator.SetInteger("Reload", 1);
                }
                else if (!weaponReloadSystem.IsReloading)
                {
                    stateValue[5] = false;
                    animator.SetInteger("Reload", -1);
                }
            }

            if (SimpleInputManager.GetCrouch())
            {
                stateValue[9] = true;
                Debug.Log("Set crouch to true");
            }
            else if (!SimpleInputManager.GetCrouch())
            {
                stateValue[9] = false;
                Debug.Log("Set crouch to false");
            }

            if (inventory.IsSelect)
            {
                animator.SetTrigger("TakeOut");
            }

            if (useSway)
                RotationSway(transform);

            JumpSway(transform);
        }

        public virtual void RotationSway(Transform weapon)
        {
            float factorX = -SimpleInputManager.GetMouseX() * amount;
            float factorY = -SimpleInputManager.GetMouseY() * amount;

            if (factorX > maxAmount)
                factorX = maxAmount;

            if (factorX < -maxAmount)
                factorX = -maxAmount;

            if (factorY > maxAmount)
                factorY = maxAmount;

            if (factorY < -maxAmount)
                factorY = -maxAmount;

            Vector3 final = new Vector3(def.x + factorX, def.y + factorY, def.z);
            weapon.localPosition = Vector3.Lerp(weapon.localPosition, final, Time.deltaTime * smooth);

            float tiltAroundZ = SimpleInputManager.GetMouseX() * tiltAngle;
            float tiltAroundX = SimpleInputManager.GetMouseY() * tiltAngle;
            Quaternion target = Quaternion.Euler(weapon.localRotation.x + tiltAroundX, staticY, weapon.localRotation.z + tiltAroundZ);
            weapon.localRotation = Quaternion.Slerp(weapon.localRotation, target, Time.deltaTime * smoothRotation);
        }

        public virtual void JumpSway(Transform weapon)
        {
            if (!characterController.isGrounded)
            {
                float newY = Mathf.Lerp(weapon.localPosition.y, weapon.localPosition.y + maxYPosJump, smoothJump * Time.deltaTime);
                weapon.localPosition = new Vector3(weapon.localPosition.x, newY, weapon.localPosition.z);
            }
            else
            {
                float newY = Mathf.Lerp(weapon.localPosition.y, def.y, smoothLand * Time.deltaTime);
                weapon.localPosition = new Vector3(weapon.localPosition.x, newY, weapon.localPosition.z);
            }
        }

        public float MoveAmount
        {
            get
            {
                return Mathf.Clamp01(Mathf.Abs(SimpleInputManager.GetVertical()) + Mathf.Abs(SimpleInputManager.GetVertical()));
            }
        }

        /// <summary>
        /// Initializing the state
        /// </summary>
        /// <returns></returns>
        protected virtual string[] InitStates()
        {
            return new string[11] { "Idle", "Walk", "Run", "Fire", "Sight", "Reload", "Fall", "Jump", "TakeOut", "Crouch", "TakeUp" };
        }

        /// <summary>
        /// Current active state
        /// </summary>
        /// <returns>ActiveState</returns>
        public string ActiveState
        {
            get
            {
                for (int i = 0; i < stateValue.Length; i++)
                    if (stateValue[i])
                        return stateName[i];
                return "No active state";
            }
        }

        public float Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public float MaxAmount
        {
            get
            {
                return maxAmount;
            }

            set
            {
                maxAmount = value;
            }
        }

        public float Smooth
        {
            get
            {
                return smooth;
            }

            set
            {
                smooth = value;
            }
        }

        public float SmoothRotation
        {
            get
            {
                return smoothRotation;
            }

            set
            {
                smoothRotation = value;
            }
        }

        public float TiltAngle
        {
            get
            {
                return tiltAngle;
            }

            set
            {
                tiltAngle = value;
            }
        }

        public float StaticY
        {
            get
            {
                return staticY;
            }

            set
            {
                staticY = value;
            }
        }

        public float MaxYPosJump
        {
            get
            {
                return maxYPosJump;
            }

            set
            {
                maxYPosJump = value;
            }
        }

        public float SmoothJump
        {
            get
            {
                return smoothJump;
            }

            set
            {
                smoothJump = value;
            }
        }

        public float SmoothLand
        {
            get
            {
                return smoothLand;
            }

            set
            {
                smoothLand = value;
            }
        }

        public bool UseSway
        {
            get
            {
                return useSway;
            }

            set
            {
                useSway = value;
            }
        }

        public Animator Animator
        {
            get
            {
                return animator;
            }

            set
            {
                animator = value;
            }
        }

        public CharacterController CharacterController
        {
            get
            {
                return characterController;
            }

            set
            {
                characterController = value;
            }
        }

        public WeaponReloadSystem WeaponReloadSystem
        {
            get
            {
                return weaponReloadSystem;
            }

            set
            {
                weaponReloadSystem = value;
            }
        }

        public PlayerInventory Inventory
        {
            get
            {
                return inventory;
            }

            set
            {
                inventory = value;
            }
        }
    }
}