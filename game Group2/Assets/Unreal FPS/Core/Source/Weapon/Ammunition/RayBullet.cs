/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;
using System.Collections.Generic;

namespace UnrealFPS
{
    public class RayBullet : ScriptableObject
    {
        [SerializeField] private string model;
        [SerializeField] private string caliber;
        [SerializeField] private string type;
        [SerializeField] private int damage;
        [SerializeField] private float variance;
        [SerializeField] private int numberbullet = 1;
        [SerializeField] private List<WeaponHitParams> bulletHitEffects = new List<WeaponHitParams>();


        /// <summary>
        /// Bullet model
        /// </summary>
        public string Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        /// <summary>
        /// Bullet caliber
        /// </summary>
        public string Caliber
        {
            get
            {
                return caliber;
            }

            set
            {
                caliber = value;
            }
        }

        /// <summary>
        /// Bullet type
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Bullet hit damage
        /// </summary>
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

        /// <summary>
        /// Bullet variance (Shotgun bullet)
        /// </summary>
        public float Variance
        {
            get
            {
                return variance;
            }

            set
            {
                variance = value;
            }
        }

        /// <summary>
        /// Number bullet (Shotgun bullet)
        /// </summary>
        public int Numberbullet
        {
            get
            {
                return numberbullet;
            }

            set
            {
                numberbullet = value;
            }
        }

        public List<WeaponHitParams> BulletHitEffects
        {
            get
            {
                return bulletHitEffects;
            }
        }
    }
}
