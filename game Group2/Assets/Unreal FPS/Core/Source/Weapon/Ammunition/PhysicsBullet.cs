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

namespace UnrealFPS
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class PhysicsBullet : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private float radius;
        [SerializeField] private float power;
        [SerializeField] private float lifetime;
        [SerializeField] private int numberBullet = 1;
        [SerializeField] private float variance;
        [SerializeField] private bool destroyOnHit;
        [SerializeField] private List<WeaponHitParams> bulletHitEffects = new List<WeaponHitParams>();

        private AudioSource audioSource;
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(LifeTime(lifetime));
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision collision)
        {
            WeaponHit.OnCollision(bulletHitEffects, collision, collision.contacts[0], audioSource);
            OnHitDamage(collision, damage);
            ExplosionForce(power, radius);
            if (destroyOnHit)
                Destroy(gameObject);
        }

        /// <summary>
        /// Explosion Handler
        /// </summary>
        /// <param name="explosionPower"></param>
        /// <param name="explosionRadius"></param>
        public virtual void ExplosionForce(float explosionPower, float explosionRadius)
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            for (int i = 0; i < colliders.Length; i++)
            {
                IHealth health = colliders[i].GetComponent<IHealth>();
                if (health != null)
                    health.TakeDamage(damage);

                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
            }
        }

        /// <summary>
        /// On Hit Damage Action
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="damageValue"></param>
        public virtual void OnHitDamage(Collision collision, int damageValue)
        {
            if (collision.transform.GetComponent<IHealth>() != null)
                collision.gameObject.GetComponent<IHealth>().TakeDamage(damage);
        }

        /// <summary>
        /// Destroy buller when timeout
        /// </summary>
        protected virtual IEnumerator LifeTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
            yield break;
        }

        public int Damage
        {
            get
            {
                return damage;
            }

            set
            {
                damage = value;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public float Power
        {
            get
            {
                return power;
            }

            set
            {
                power = value;
            }
        }

        public float Lifetime
        {
            get
            {
                return lifetime;
            }

            set
            {
                lifetime = value;
            }
        }

        public int NumberBullet
        {
            get
            {
                return numberBullet;
            }

            set
            {
                numberBullet = value;
            }
        }

        public float Variance
        {
            get
            {
                return variance;
            }

            set
            {
                variance = value;
            }
        }

        public bool DestroyOnHit
        {
            get
            {
                return destroyOnHit;
            }

            set
            {
                destroyOnHit = value;
            }
        }

        public List<WeaponHitParams> BulletHitEffects
        {
            get
            {
                return bulletHitEffects;
            }

            set
            {
                bulletHitEffects = value;
            }
        }

        public AudioSource AudioSource
        {
            get
            {
                return audioSource;
            }

            set
            {
                audioSource = value;
            }
        }
    }
}