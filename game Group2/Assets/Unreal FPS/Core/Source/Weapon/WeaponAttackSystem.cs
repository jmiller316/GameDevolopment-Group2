using UnityEngine;

namespace UnrealFPS
{
	public enum AttackType { RayCast, Physics, Throw }

	public enum ShootType { Default, Fractional }


	[RequireComponent(typeof(WeaponReloadSystem))]
	[RequireComponent(typeof(WeaponAnimationSystem))]
	[RequireComponent(typeof(AudioSource))]
	public class WeaponAttackSystem : MonoBehaviour
	{
		[SerializeField] private AttackType attackType;
		[SerializeField] private Transform attackPoint;
		[SerializeField] private Transform bullet; //For Physics/Throw Attack
		[SerializeField] private RayBullet rayBullet; //For RayCast Attack
		[SerializeField] private float delay;
		[SerializeField] private float attackImpulse;
		[SerializeField] private float attackRange;
		[SerializeField] private AudioClip attackSound;
		[SerializeField] private AudioClip emptySound;
		[SerializeField] private ParticleSystem muzzleFlash;
		[SerializeField] private ParticleSystem cartridgeEjection;
		[SerializeField] private SpreadSystem spreadSystem = new SpreadSystem();

		private WeaponReloadSystem weaponReloadSystem;
		private WeaponAnimationSystem weaponAnimationSystem;
		private AudioSource audioSource;
		private PhysicsBullet physicsBullet;

		private bool isAttack;
		private float s_Delay;


		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
			weaponReloadSystem = GetComponent<WeaponReloadSystem>();
			weaponAnimationSystem = GetComponent<WeaponAnimationSystem>();
			if (attackType == AttackType.Physics)
				physicsBullet = bullet.GetComponent<PhysicsBullet>();
			spreadSystem.Initialize(transform.root.GetComponent<FPController>().Camera.transform, transform.root.GetComponent<FPController>().MouseLook);
			s_Delay = delay;
		}


		private void Update()
		{
			AttackBehaviour();
		}

		protected virtual void AttackBehaviour()
		{
			if (SimpleInputManager.GetFire() && !isAttack && !weaponReloadSystem.BulletsIsEmpty && !weaponReloadSystem.IsReloading)
			{
				spreadSystem.BulletSpreadProcessing(attackPoint, weaponAnimationSystem);
				if (attackSound != null)
					audioSource.PlayOneShot(attackSound);
				switch (attackType)
				{
					case AttackType.RayCast:
						RayCastAttack();
						break;
					case AttackType.Physics:
						PhysicsAttack();
						break;
					case AttackType.Throw:
						StartCoroutine(ThrowAttack(attackRange));
						break;
				}
				weaponReloadSystem.BulletCount -= 1;
				AttackParticleEffect();
				spreadSystem.CameraSpreadProcessing();
				isAttack = true;
			}
			else if (SimpleInputManager.GetFire() && !isAttack && weaponReloadSystem.BulletsIsEmpty && !weaponReloadSystem.IsReloading)
			{
				if (emptySound != null)
					audioSource.PlayOneShot(emptySound);
				isAttack = true;
			}
			Delay();
		}

		protected virtual void RayCastAttack()
		{
			RaycastHit raycastHit;
			for (int i = 0; i < rayBullet.Numberbullet; i++)
			{
				if (rayBullet.Numberbullet > 1)
					attackPoint.localRotation = Quaternion.Euler(Random.Range(-rayBullet.Variance, rayBullet.Variance), Random.Range(-rayBullet.Variance, rayBullet.Variance), 0);
				if (Physics.Raycast(attackPoint.position, attackPoint.forward, out raycastHit, attackRange))
				{
					WeaponHit.OnRay(rayBullet.BulletHitEffects, raycastHit, audioSource);
					SendDamage(raycastHit, rayBullet.Damage);
					if (raycastHit.rigidbody)
						raycastHit.rigidbody.AddForceAtPosition(attackPoint.forward * attackImpulse, raycastHit.point);
				}
			}
			if (rayBullet.Numberbullet > 1)
				attackPoint.localRotation = Quaternion.identity;
		}

		protected virtual void PhysicsAttack()
		{
			for (int i = 0; i < physicsBullet.NumberBullet; i++)
			{
				if (physicsBullet.NumberBullet > 1)
					attackPoint.localRotation = Quaternion.Euler(Random.Range(-physicsBullet.Variance, physicsBullet.Variance), Random.Range(-physicsBullet.Variance, physicsBullet.Variance), 0);
				GameObject cloneBullet = GameObject.Instantiate(bullet.gameObject, attackPoint.position, attackPoint.rotation);
				cloneBullet.GetComponent<Rigidbody>().AddForce(attackPoint.forward * attackImpulse, ForceMode.Impulse);
			}
			if (physicsBullet.NumberBullet > 1)
				attackPoint.localRotation = Quaternion.identity;
		}

		protected virtual System.Collections.IEnumerator ThrowAttack(float time)
		{
			yield return new WaitForSeconds(time);
			GameObject cloneBullet = GameObject.Instantiate(bullet.gameObject, attackPoint.position, attackPoint.rotation);
			cloneBullet.GetComponent<Rigidbody>().AddForce(attackPoint.forward * attackImpulse, ForceMode.Impulse);
			yield break;
		}

		protected virtual void Delay()
		{
			if (isAttack)
			{
				delay -= Time.deltaTime;
				if (delay <= 0)
				{
					isAttack = false;
					delay = s_Delay;
				}
			}
		}

		protected virtual void SendDamage(RaycastHit raycastHit, int damage)
		{
			if (raycastHit.transform.root.GetComponent<IHealth>() != null)
				raycastHit.transform.root.GetComponent<IHealth>().TakeDamage(damage);
		}

        protected virtual void AttackParticleEffect()
		{
			if (muzzleFlash != null) muzzleFlash.Play();
			if (cartridgeEjection != null) cartridgeEjection.Play();
		}

		public AttackType W_AttackType { get { return attackType; } set { attackType = value; } }

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

        public Transform Bullet
        {
            get
            {
                return bullet;
            }

            set
            {
                bullet = value;
            }
        }

        public RayBullet RayBullet
        {
            get
            {
                return rayBullet;
            }

            set
            {
                rayBullet = value;
            }
        }

        public float Delay1
        {
            get
            {
                return delay;
            }

            set
            {
                delay = value;
            }
        }

        public float AttackImpulse
        {
            get
            {
                return attackImpulse;
            }

            set
            {
                attackImpulse = value;
            }
        }

        public float AttackRange
        {
            get
            {
                return attackRange;
            }

            set
            {
                attackRange = value;
            }
        }

        public AudioClip AttackSound
        {
            get
            {
                return attackSound;
            }

            set
            {
                attackSound = value;
            }
        }

        public AudioClip EmptySound
        {
            get
            {
                return emptySound;
            }

            set
            {
                emptySound = value;
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

        public SpreadSystem SpreadSystem
        {
            get
            {
                return spreadSystem;
            }

            set
            {
                spreadSystem = value;
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

        public WeaponAnimationSystem WeaponAnimationSystem
        {
            get
            {
                return weaponAnimationSystem;
            }

            set
            {
                weaponAnimationSystem = value;
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

        public PhysicsBullet PhysicsBullet
        {
            get
            {
                return physicsBullet;
            }

            set
            {
                physicsBullet = value;
            }
        }
    }
}