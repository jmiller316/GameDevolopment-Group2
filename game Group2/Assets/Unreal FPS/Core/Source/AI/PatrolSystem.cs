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

namespace UnrealFPS
{
	/// <summary>
	/// 
	/// </summary>
	public enum PatrolType { Random, Sequential }

	[System.Serializable]
	public class PatrolSystem
	{
		[SerializeField] PatrolType patrolType;
		[SerializeField] private Transform[] points;
		[SerializeField] private float updatePointDistance;

		private Transform transform;
		private NavMeshAgent navMeshAgent;
		private int point = 0;

		/// <summary>
		/// Init is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		public virtual void Initialize(Transform transform, NavMeshAgent navMeshAgent)
		{
			this.transform = transform;
			this.navMeshAgent = navMeshAgent;

			if (points.Length > 0)
			{
				switch (patrolType)
				{
					case PatrolType.Random:
						point = Random.Range(0, points.Length);
						break;
					case PatrolType.Sequential:
						point = 0;
						break;
				}
			}
			
		}

		/// <summary>
		/// Handling patrol system
		/// </summary>
		public virtual void PatrolHandler()
		{
			if (points.Length < 0)
				return;

			switch (patrolType)
			{
				case PatrolType.Random:
					RandomPatrol();
					break;
				case PatrolType.Sequential:
					SequentialPatrol();
					break;
			}
		}

		/// <summary>
		/// Random patrol system
		/// </summary>
		public virtual void RandomPatrol()
		{
			if (Vector3.Distance(transform.position, points[point].position) <= updatePointDistance)
				point = Random.Range(0, points.Length);
			else
				navMeshAgent.destination = points[point].position;
		}

		/// <summary>
		/// Sequential patrol system
		/// </summary>
		public virtual void SequentialPatrol()
		{
			if (Vector3.Distance(transform.position, points[point].position) <= updatePointDistance)
				point = (point < points.Length - 1) ? point + 1 : 0;
			else
				navMeshAgent.destination = points[point].position;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public PatrolType _PatrolType { get { return patrolType; } set { patrolType = value; } }

		/// <summary>
		/// Points array
		/// </summary>
		/// <value></value>
		public Transform[] Points { get { return points; } set { points = value; } }

		/// <summary>
		/// Current point index
		/// </summary>
		/// <value></value>
		public int Point { get { return point; } protected set { point = value; } }

        public float UpdatePointDistance
        {
            get
            {
                return updatePointDistance;
            }

            set
            {
                updatePointDistance = value;
            }
        }

        public Transform Transform
        {
            get
            {
                return transform;
            }

            set
            {
                transform = value;
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
    }
}