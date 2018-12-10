/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.AI;

namespace UnrealFPS.AI
{
	/// <summary>
	/// 
	/// </summary>
	public class AIAnimatorHandler : MonoBehaviour
	{
		[SerializeField] private InverseKinematics inverseKinematics = new InverseKinematics();

		private bool isAiming;
		private bool isCrouching;

		private Vector2 smoothDeltaPosition;
		private Vector2 velocity;

		private Animator animator;
		private NavMeshAgent navMeshAgent;
		private AIBehaviour behaviour;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		private void Start()
		{
			animator = GetComponent<Animator>();
			navMeshAgent = GetComponent<NavMeshAgent>();
			behaviour = GetComponent<AIBehaviour>();
			inverseKinematics.Init(animator);
			smoothDeltaPosition = Vector2.zero;
			velocity = Vector2.zero;
			navMeshAgent.updatePosition = false;
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void Update()
		{
			Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;

			// Map 'worldDeltaPosition' to local space
			float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
			float dx = Vector3.Dot(transform.right, worldDeltaPosition);
			Vector2 deltaPosition = new Vector2(dx, dy);

			// Low-pass filter the deltaMove
			float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
			smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

			// Update velocity if time advances
			if (Time.deltaTime > 1e-5f)
				velocity = smoothDeltaPosition / Time.deltaTime;

			// Update animation parameters
			animator.SetFloat("Speed", velocity.y);
			animator.SetFloat("Direction", velocity.x);
		}

		/// <summary>
		/// Callback for processing animation movements for modifying root motion.
		/// </summary>
		private void OnAnimatorMove()
		{
			if (navMeshAgent != null)
				// Update position to agent position
				transform.position = navMeshAgent.nextPosition;
		}

		/// <summary>
		/// Callback for setting up animation IK (inverse kinematics).
		/// </summary>
		/// <param name="layerIndex">Index of the layer on which the IK solver is called.</param>
		void OnAnimatorIK(int layerIndex)
		{
			inverseKinematics.FootIK();

			if (behaviour != null)
			{
				inverseKinematics.UpdateTarget((behaviour.Target != null) ? behaviour.Target : inverseKinematics.LookTarget);
				if (inverseKinematics.LookTarget != null)
					inverseKinematics.HeadIK();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public float Speed { get { return velocity.y; } }

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public float Direction { get { return velocity.x; } }

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public bool IsAiming { get { return isAiming; } set { isAiming = value; } }

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public bool IsCrouching { get { return isCrouching; } set { isCrouching = value; } }

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public Animator _Animator { get { return animator; } }

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

        public AIBehaviour Behaviour
        {
            get
            {
                return behaviour;
            }

            set
            {
                behaviour = value;
            }
        }
    }
}