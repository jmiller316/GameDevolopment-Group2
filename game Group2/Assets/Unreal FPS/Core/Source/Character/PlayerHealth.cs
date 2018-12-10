/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnrealFPS
{
    /// <summary>
    /// Fal Damage struct
    /// </summary>
    [System.Serializable]
    public struct FallDamage
    {
        public float minHeight;
        public float maxHeight;
        public int damage;
    }

    [System.Serializable]
    public struct RegenirationSettings
    {
        public float interval;
        public int value;
        public float time;
    }

    /// <summary>
    /// Base Player Health class
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private int startHealth;
        [SerializeField] private List<FallDamage> fallDamages = new List<FallDamage>();
        [SerializeField] private UnityEvent onDeadEvent;
        [SerializeField] private KillCam killCam;
        [SerializeField] private Image damageImage;
        [SerializeField] private bool useRegeniration;
        [SerializeField] private RegenirationSettings regenerationSettings;

        private FPController controller;
        private CharacterController characterController;
        private float lastHeightPosition;
        private bool isStarting;
        private bool regCall;

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            controller = GetComponent<FPController>();
            characterController = GetComponent<CharacterController>();
            killCam.Init(controller.Camera.transform, characterController);
            health = startHealth;
        }

        private void Update()
        {
            HealthHandler();
            DamageScreenHandler();

            if (IsAlive)
            {
                HealthRegeneration();
                FallDamage(characterController.isGrounded);
            }
        }

        /// <summary>
        /// Player Health handler
        /// </summary>
        public virtual void HealthHandler()
        {
            if (!IsAlive)
            {
                OnDead();
                controller.LockMovement = true;
                killCam.Play();
                isStarting = true;
            }
            else if (isStarting)
            {
                killCam.Reset();
                controller.LockMovement = false;
                isStarting = false;
            }
        }

        /// <summary>
        /// Health Regenegation System
        /// </summary>
        public void HealthRegeneration()
        {
            if (!useRegeniration)
                return;

            if (health != maxHealth && !regCall)
            {
                StartCoroutine(Regenerate(regenerationSettings));
                regCall = true;
            }

        }

        public IEnumerator Regenerate(RegenirationSettings regenerationSettings)
        {
            bool waitBeforeStart = true;
            bool playRegenerate = false;

            while (true)
            {
                //First thread
                while (waitBeforeStart)
                {
                    float lastHealth = health;
                    yield return new WaitForSeconds(regenerationSettings.time);
                    if (lastHealth == health)
                    {
                        waitBeforeStart = false;
                        playRegenerate = true;
                        break;
                    }
                }

                //Second thread
                while (playRegenerate)
                {
                    health += regenerationSettings.value;
                    float lastHealth = health;
                    yield return new WaitForSeconds(regenerationSettings.interval);
                    if (health == maxHealth)
                    {
                        regCall = false;
                        yield break;
                    }
                    else if (lastHealth != health)
                    {
                        waitBeforeStart = true;
                        playRegenerate = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Damage Screen System Handler
        /// </summary>
        public void DamageScreenHandler()
        {
            if (damageImage == null)
                return;

            if (damageImage.color.a != (float) (100 - health) / 100)
            {
                damageImage.color = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, Mathf.MoveTowards(damageImage.color.a, (float) (100 - health) / 100, 10 * Time.deltaTime));
            }
        }

        /// <summary>
        /// Take damage
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            health -= amount;
        }

        public virtual void OnDead()
        {
            onDeadEvent.Invoke();
        }

        public virtual void FallDamage(bool isGrounded)
        {
            if (!isGrounded)
            {
                if (lastHeightPosition < transform.position.y)
                {
                    lastHeightPosition = transform.position.y;
                }
            }
            else if (lastHeightPosition > transform.position.y)
            {
                float distance = lastHeightPosition - transform.position.y;
                for (int i = 0; i < fallDamages.Count; i++)
                {
                    if (distance > fallDamages[i].minHeight && distance < fallDamages[i].maxHeight)
                    {
                        TakeDamage(fallDamages[i].damage);
                        lastHeightPosition = transform.position.y;
                    }
                }
            }
        }

        /// <summary>
        /// Player health
        /// </summary>
        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }

        /// <summary>
        /// Player is alive
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return (health > 0) ? true : false;
            }
        }

        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

        public float HealthPercent { get { return ((float) health / maxHealth) * 100; } }

        public KillCam KillCam { get { return killCam; } set { killCam = value; } }
    }
}