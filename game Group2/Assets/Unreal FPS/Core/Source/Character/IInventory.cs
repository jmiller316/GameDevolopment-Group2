/* ==================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS
{
	/// <summary>
	/// Interface describing the architecture of the inventory
	/// </summary>
	public interface IInventory
	{
		/// <summary>
		/// Add weapon
		/// </summary>
		/// <param name="weapon"></param>
		void AddWeapon(Weapon weapon);

		/// <summary>
		/// Drop weapon
		/// </summary>
		/// <param name="weapon"></param>
		void DropWeapon(Weapon weapon);

		/// <summary>
		/// Activate weapon by id
		/// </summary>
		/// <param name="id"></param>
		void ActivateWeapon(string id);

		/// <summary>
		/// Deativate weapon by id
		/// </summary>
		/// <param name="id"></param>
		void DeactivateWeapon(string id);

		/// <summary>
		/// Get weapon by unique identifier
		/// </summary>
		/// <param name="id"></param>
		Transform GetWeapon(string id);

		/// <summary>
		/// Get weapon by index in FPCamera child
		/// </summary>
		/// <param name="index"></param>
		Transform GetWeapon(int index);

		/// <summary>
		/// Get current active weapon
		/// </summary>
		/// <returns>Transform</returns>
		Transform GetActiveWeapon();
	}
}