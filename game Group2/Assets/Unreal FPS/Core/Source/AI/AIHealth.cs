/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS.AI
{
	/// <summary>
	/// AI health system
	/// </summary>
	public class AIHealth : MonoBehaviour, IHealth
	{
		[SerializeField] private int health;
		[SerializeField] private int maxHealth;
        
        /// <summary>
        /// Take damage by damage
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
		{
			health -= damage;
		}

        /// <summary>
        /// Health count
        /// </summary>
		public int Health { get { return health; } set { health = value; } }

        /// <summary>
        /// Max health count
        /// </summary>
		public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

        /// <summary>
        /// Health state
        /// </summary>
		public bool IsAlive { get { return (health > 0) ? true : false; } }
	}
}