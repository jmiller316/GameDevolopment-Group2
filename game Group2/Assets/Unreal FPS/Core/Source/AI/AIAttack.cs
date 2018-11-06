/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.AI
{
	[System.Serializable]
	public class AIAttack
	{
		[System.Serializable]
		private struct Attack
		{
			public RayBullet rayBullet;
			public float delay;
			public float attackImpulse;
			public float attackRange;
			public AudioClip attackSound;
			public AudioClip emptySound;
		}

		[SerializeField] private Transform attackPoint;
		[SerializeField] private Attack shoot;
		[SerializeField] private Attack melee;
		[SerializeField] private ParticleSystem muzzleFlash;
		[SerializeField] private ParticleSystem cartridgeEjection;

		private AudioSource audioSource;
		private float[] s_Delay;

        /// <summary>
        /// Initialize is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        public virtual void Initialize(AudioSource audioSource)
		{
			this.audioSource = audioSource;
			s_Delay = new float[2] { shoot.delay, melee.delay};
		}

		/// <summary>
		/// Shoot system
		/// </summary>
		public virtual void Shoot(Vector3 direction)
		{
			if (Delay(ref shoot.delay, s_Delay[0]))
			{
				audioSource.PlayOneShot(shoot.attackSound);
				RaycastHit raycastHit;
				for (int i = 0; i < shoot.rayBullet.Numberbullet; i++)
				{
					if (shoot.rayBullet.Numberbullet > 1)
						attackPoint.localRotation = Quaternion.Euler(Random.Range(-shoot.rayBullet.Variance, shoot.rayBullet.Variance), Random.Range(-shoot.rayBullet.Variance, shoot.rayBullet.Variance), 0);
					Vector3 _Direction = (direction - attackPoint.position).normalized;
					if (Physics.Raycast(attackPoint.position, _Direction, out raycastHit, shoot.attackRange))
					{
						WeaponHit.OnRay(shoot.rayBullet.BulletHitEffects, raycastHit, audioSource);
						SendDamage(raycastHit, shoot.rayBullet.Damage);
						if (raycastHit.rigidbody)
							raycastHit.rigidbody.AddForceAtPosition(attackPoint.forward * shoot.attackImpulse, raycastHit.point);
					}
				}
				if (shoot.rayBullet.Numberbullet > 1)
					attackPoint.localRotation = Quaternion.identity;
				AttackParticleEffect();
			}
		}

		/// <summary>
		/// Melee attack system
		/// </summary>
		/// <param name="direction"></param>
		public virtual void Impact(Vector3 direction)
		{
			if (Delay(ref melee.delay, s_Delay[1]))
			{
				audioSource.PlayOneShot(melee.attackSound);
				RaycastHit raycastHit;
				Vector3 _Direction = (direction - attackPoint.position).normalized;
				if (Physics.Raycast(attackPoint.position, _Direction, out raycastHit, melee.attackRange))
				{
					WeaponHit.OnRay(melee.rayBullet.BulletHitEffects, raycastHit, audioSource);
					SendDamage(raycastHit, melee.rayBullet.Damage);
					if (raycastHit.rigidbody)
						raycastHit.rigidbody.AddForceAtPosition(attackPoint.forward * melee.attackImpulse, raycastHit.point);
				}
			}
		}

		/// <summary>
		/// Attack delay
		/// </summary>
		protected virtual bool Delay(ref float delay, float s_Delay)
		{
			delay -= Time.deltaTime;
			if (delay <= 0)
			{
				delay = s_Delay;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Sending damage to the hit object
		/// </summary>
		/// <param name="raycastHit"></param>
		/// <param name="damage"></param>
		protected virtual void SendDamage(RaycastHit raycastHit, int damage)
		{
			IHealth health = raycastHit.transform.root.GetComponent<IHealth>();
			if (health != null)
				health.TakeDamage(damage);
		}

		/// <summary>
		/// Play attack effect
		/// </summary>
		protected virtual void AttackParticleEffect()
		{
			if (muzzleFlash != null) muzzleFlash.Play();
			if (cartridgeEjection != null) cartridgeEjection.Play();
		}

        public Transform AttackPoint
        {
            get
            {
                return attackPoint;
            }

            set
            {
                attackPoint = value;
            }
        }

        private Attack Shoot1
        {
            get
            {
                return shoot;
            }

            set
            {
                shoot = value;
            }
        }

        private Attack Melee
        {
            get
            {
                return melee;
            }

            set
            {
                melee = value;
            }
        }

        public ParticleSystem MuzzleFlash
        {
            get
            {
                return muzzleFlash;
            }

            set
            {
                muzzleFlash = value;
            }
        }

        public ParticleSystem CartridgeEjection
        {
            get
            {
                return cartridgeEjection;
            }

            set
            {
                cartridgeEjection = value;
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