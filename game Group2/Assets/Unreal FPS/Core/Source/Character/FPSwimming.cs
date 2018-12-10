/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace UnrealFPS
{

    public enum WaterState
    {
        In,
        Out
    }

    [Serializable]
    public class FPSwimming
    {
        #region Private SerializeField Variable
        [SerializeField] private float speed;
        [SerializeField] private float playSoundSpeed;
        [SerializeField] private AudioClip inWaterSound;
        [SerializeField] private AudioClip outWaterSound;
        [SerializeField] private AudioClip landWaterSound;
        [SerializeField] private float waterGravity;
        #endregion

        #region Private Variable
        private WaterState waterState;
        private CharacterController characterController;
        private Transform playerCamera;
        private AudioSource audioSource;
        private float wasplaySoundSpeed;
        private bool isSwim;
        #endregion

        /// <summary>
        /// Initialize the required components
        /// </summary>
        /// <param name="player"></param>
        /// <param name="playerCamera"></param>
        /// <param name="characterController"></param>
        /// <param name="audioSource"></param>
        public void Init(Transform playerCamera, CharacterController characterController, PlayerInventory playerInventory, AudioSource audioSource)
        {
            this.characterController = characterController;
            this.audioSource = audioSource;
            this.playerCamera = playerCamera;
            wasplaySoundSpeed = playSoundSpeed;
        }

        /// <summary>
        /// Player Swimming Handler
        /// </summary>
        public void Swimming()
        {
            Collider[] colliders = Physics.OverlapSphere(playerCamera.position, 0.1f);
            for (int i = 0; i < colliders.Length; i++)
            {
                Marking marking = colliders[i].GetComponent<Marking>();
                if (marking != null && marking.CompareMark(MarkItem.EnterWater))
                {
                    audioSource.PlayOneShot(landWaterSound);
                    if (waterState == WaterState.In) { waterState = WaterState.Out; }
                    if (waterState == WaterState.Out) { waterState = WaterState.In; }
                    isSwim = true;
                }
            }

            if (!isSwim)
                return;
                

            Vector3 targetPosition = playerCamera.transform.forward;
            if (SimpleInputManager.IsJumping())
            {
                characterController.Move(Vector3.up * speed * Time.deltaTime);
            }
            if (SimpleInputManager.GetVertical() != 0 || SimpleInputManager.GetHorizontal() != 0)
            {
                if (SimpleInputManager.GetHorizontal() == 1)
                {
                    characterController.Move(targetPosition * speed * Time.deltaTime);
                }
                if (SimpleInputManager.GetVertical() == -1)
                {
                    characterController.Move(-targetPosition * speed * Time.deltaTime);
                }
                switch (waterState)
                {
                    case WaterState.In:
                        SwimmingSoundHandler(inWaterSound);
                        break;
                    case WaterState.Out:
                        SwimmingSoundHandler(outWaterSound);
                        break;
                }
            }
        }

        /// <summary>
        /// Player Swimming Sound Handler
        /// </summary>
        /// <param name="swimmingSound"></param>
        public void SwimmingSoundHandler(AudioClip swimmingSound)
        {
            playSoundSpeed -= Time.deltaTime;
            if (playSoundSpeed <= 0)
            {
                audioSource.clip = swimmingSound;
                audioSource.Play();
                playSoundSpeed = wasplaySoundSpeed;
            }
        }

        public void SetWaterState(WaterState waterState)
        {
            this.waterState = waterState;
        }

        public void SetSwim(bool isSwim)
        {
            this.isSwim = isSwim;
        }

        /// <summary>
        /// Swimming speed
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        /// <summary>
        /// Player is swimming
        /// </summary>
        public bool IsSwim
        {
            get
            {
                return isSwim;
            }
        }

        /// <summary>
        /// Water Gravity
        /// </summary>
        public float WaterGravity
        {
            get
            {
                return waterGravity;
            }

            set
            {
                waterGravity = value;
            }
        }

        /// <summary>
        /// When player out water sound
        /// </summary>
        public AudioClip OutWaterSound
        {
            get
            {
                return outWaterSound;
            }

            set
            {
                outWaterSound = value;
            }
        }

        /// <summary>
        /// When player in water sound
        /// </summary>
        public AudioClip InWaterSound
        {
            get
            {
                return inWaterSound;
            }

            set
            {
                inWaterSound = value;
            }
        }

        /// <summary>
        /// Play sound speed cycle
        /// </summary>
        public float PlaySoundSpeed
        {
            get
            {
                return playSoundSpeed;
            }

            set
            {
                playSoundSpeed = value;
            }
        }
    }
}