/* ================================================================
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
    /// 
    /// </summary>
    public class WeaponIdentifier : MonoBehaviour
    {
        [SerializeField] Weapon weapon;

        /// <summary>
        /// Weapon
        /// </summary>
        public Weapon Weapon { get { return weapon; } }
    }
}