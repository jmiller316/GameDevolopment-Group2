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
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody))]
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField] private int damage;
        [SerializeField] private float force;
        [SerializeField] private List<WeaponHitParams> weaponHitSurface = new List<WeaponHitParams>();

        private AudioSource audioSource;
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision collision)
        {
            IHealth iHealth = collision.transform.GetComponent<IHealth>();
            Rigidbody rigidbody = collision.transform.GetComponent<Rigidbody>();

            if (iHealth != null)
                collision.transform.GetComponent<IHealth>().TakeDamage(damage);

            if (rigidbody != null)
                rigidbody.AddForce(-collision.transform.forward * force, ForceMode.Impulse);

            WeaponHit.OnCollision(weaponHitSurface, collision, collision.contacts[0], audioSource);
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

        public float Force
        {
            get
            {
                return force;
            }

            set
            {
                force = value;
            }
        }

        public List<WeaponHitParams> WeaponHitSurface
        {
            get
            {
                return weaponHitSurface;
            }

            set
            {
                weaponHitSurface = value;
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