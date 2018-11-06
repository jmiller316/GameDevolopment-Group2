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
using UnrealFPS.Utility;

namespace UnrealFPS.AI
{
	/// <summary>
	/// AI Spawn Manager
	/// </summary>
	public class AISpawnManager : MonoBehaviour
	{
		[SerializeField] private SpawnShape shape;
		[SerializeField] private Transform _AI;
		[SerializeField] private Vector3 rotation;
		[SerializeField] private float radius = 1;
		[SerializeField] private float lenght = 1;
		[SerializeField] private float weight = 1;
		[SerializeField] private float interval;
		[SerializeField] private float startTime = -1;
		[SerializeField] private Transform[] patrolPoint;

		private bool isSpawn;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		protected virtual void Start()
		{
			isSpawn = (startTime >= 0);
			StartCoroutine(Spawn(interval));
		}

		/// <summary>
		/// AI spawn handler
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public virtual IEnumerator Spawn(float interval)
		{
			while (isSpawn)
			{
				yield return new WaitForSeconds(interval);
				Vector3 randomPoint = transform.position;
				switch (shape)
				{
					case SpawnShape.Rectangle:
						randomPoint = randomPoint.RandomPositionInRectangle(lenght, weight);
						break;
					case SpawnShape.Circle:
						randomPoint = randomPoint.RandomPositionInCircle(radius);
						break;
				}
				GameObject _AIClone = Instantiate(_AI.gameObject, randomPoint, Quaternion.Euler(rotation));
				AIBehaviour behaviour = _AIClone.GetComponent<AIBehaviour>();
				behaviour._PatrolSystem.Points = patrolPoint;
				yield return null;
			}
		}

		/// <summary>
		/// Return spwan state
		/// 
		/// 	True: AI is spawning
		/// 	False: AI not spawning
		/// </summary>
		/// <value></value>
		public bool IsSpawn { get { return isSpawn; } set { isSpawn = value; } }

		public SpawnShape Shape { get { return shape; } set { shape = value; } }

		public Vector3 Rotation { get { return this.rotation; } set { this.rotation = value; } }

		public float Radius { get { return radius; } set { radius = value; } }

		public float Lenght { get { return lenght; } set { lenght = value; } }

		public float Weight { get { return weight; } set { weight = value; } }

		public float Interval { get { return interval; } set { interval = value; } }

		public float StartTime { get { return startTime; } set { startTime = value; } }

        public Transform[] PatrolPoint
        {
            get
            {
                return patrolPoint;
            }

            set
            {
                patrolPoint = value;
            }
        }
    }
}