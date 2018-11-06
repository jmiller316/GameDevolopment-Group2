/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace UnrealFPS.AI
{
    /// <summary>
    /// Main AI behaviour class
    /// </summary>
    [RequireComponent(typeof(AIHealth))]
    [RequireComponent(typeof(AIFieldOfView))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    public class AIBehaviour : MonoBehaviour
    {
        [SerializeField] private float shootDistance;
        [SerializeField] private float maxFollowDistance;
        [SerializeField] private float timeBeforeDestory;

        [SerializeField] private PatrolSystem patrolSystem = new PatrolSystem();
        [SerializeField] private AIAttack attack = new AIAttack();

        //Require Components
        private Transform target;
        private IHealth health;
        private AIFieldOfView fieldOfView;
        private AIAnimatorHandler animatorHandler;
        private NavMeshAgent navMeshAgent;
        private AudioSource audioSource;
        private Collider _collider;
        private float lastHealth;
        private bool c_Bool = false;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            //Init Require Components
            health = GetComponent<IHealth>();
            fieldOfView = GetComponent<AIFieldOfView>();
            animatorHandler = GetComponent<AIAnimatorHandler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            audioSource = GetComponent<AudioSource>();
            _collider = GetComponent<Collider>();

            //Init Require Instances
            patrolSystem.Initialize(transform, navMeshAgent);
            attack.Initialize(audioSource);
            SetBoneKinematic(true);

            lastHealth = health.Health;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            AIHandler();
        }

        /// <summary>
        /// Handle AI system
        /// </summary>
        protected virtual void AIHandler()
        {
            if (!health.IsAlive)
            {
                if (!c_Bool)
                {
                    navMeshAgent.enabled = false;
                    _collider.enabled = false;
                    StartRagdoll();
                    Destroy(gameObject, timeBeforeDestory);
                    c_Bool = true;
                }
                return;
            }

            target = (fieldOfView.VisibleTargets.Count > 0) ? fieldOfView.VisibleTargets[0] : null;
            LookBehindByHit();

            if (target == null)
            {
                patrolSystem.PatrolHandler();
                navMeshAgent.isStopped = false;
                return;
            }

            if (Vector3.Distance(transform.position, target.position) <= shootDistance / 2)
            {
                navMeshAgent.destination = (transform.position + Vector3.back) * 2;
                navMeshAgent.isStopped = false;
                attack.Shoot(target.position);
            }
            else if (Vector3.Distance(transform.position, target.position) <= shootDistance)
            {
                LootAt(target.position);
                navMeshAgent.isStopped = true;
                attack.Shoot(target.position);
            }
            else if ((Vector3.Distance(transform.position, target.position) > shootDistance) && (Vector3.Distance(transform.position, target.position) <= maxFollowDistance))
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.destination = target.position;
            }
            else if (Vector3.Distance(transform.position, target.position) > maxFollowDistance)
            {
                navMeshAgent.isStopped = false;
                target = null;
            }
        }

        /// <summary>
        /// Start ragdoll
        /// </summary>
        public void StartRagdoll()
        {
            animatorHandler._Animator.enabled = false;
            SetBoneKinematic(false);
        }

        /// <summary>
        /// Enable/Disable Kinematic on Player bones
        /// </summary>
        /// <param name="active"></param>
        public void SetBoneKinematic(bool active)
        {
            Rigidbody[] rigidbody = transform.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigidbody.Length; i++)
            {
                rigidbody[i].isKinematic = active;
            }
        }

        /// <summary>
        /// AI smooth look at target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damping"></param>
        public void LootAt(Vector3 target, float damping = 3f)
        {
            Vector3 lookPos = target - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        /// <summary>
        /// Look behind AI
        /// </summary>
        public virtual void LookBehindByHit()
        {
            if (health.Health < lastHealth)
            {
                LootAt(-transform.forward * 2);
                lastHealth = health.Health;
            }
        }

        /// <summary>
        /// Current AI target
        /// </summary>
        /// <value></value>
        public Transform Target { get { return target; } }

        public PatrolSystem _PatrolSystem { get { return patrolSystem; } set { patrolSystem = value; } }

        public float ShootDistance
        {
            get
            {
                return shootDistance;
            }

            set
            {
                shootDistance = value;
            }
        }

        public float MaxFollowDistance
        {
            get
            {
                return maxFollowDistance;
            }

            set
            {
                maxFollowDistance = value;
            }
        }

        public float TimeBeforeDestory
        {
            get
            {
                return timeBeforeDestory;
            }

            set
            {
                timeBeforeDestory = value;
            }
        }

        public AIAttack Attack
        {
            get
            {
                return attack;
            }

            set
            {
                attack = value;
            }
        }

        public IHealth Health
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

        public AIFieldOfView FieldOfView
        {
            get
            {
                return fieldOfView;
            }

            set
            {
                fieldOfView = value;
            }
        }

        public AIAnimatorHandler AnimatorHandler
        {
            get
            {
                return animatorHandler;
            }

            set
            {
                animatorHandler = value;
            }
        }

        public NavMeshAgent NavMeshAgent
        {
            get
            {
                return navMeshAgent;
            }

            set
            {
                navMeshAgent = value;
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

        public Collider Collider
        {
            get
            {
                return _collider;
            }

            set
            {
                _collider = value;
            }
        }
    }
}