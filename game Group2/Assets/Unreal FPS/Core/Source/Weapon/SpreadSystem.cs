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
using Random = UnityEngine.Random;

namespace UnrealFPS
{
    [Serializable]
    public struct BulletSpreadParam
    {
        public string state;
        public float maxX;
        public float minX;
        public float maxY;
        public float minY;
    }

    [Serializable]
    public class SpreadSystem
    {
        [SerializeField] private float amplitudeX;
        [SerializeField] private float amplitudeZmin;
        [SerializeField] private float amplitudeZmax;
        [SerializeField] private float force;
        [SerializeField] private bool lockCamera;
        [SerializeField] private float speed;
        [SerializeField] private List<BulletSpreadParam> bulletSpreadParams = new List<BulletSpreadParam>();

        private Transform fpsCamera;
        private NGMouseLook mouseLook;

        public void Initialize(Transform fpsCamera, NGMouseLook mouseLook)
        {
            this.fpsCamera = fpsCamera;
            this.mouseLook = mouseLook;
        }

        /// <summary>
        /// Processing camera spread effect
        /// </summary>
        public virtual void CameraSpreadProcessing()
        {
            float x = fpsCamera.localRotation.x - amplitudeX;
            float z = Random.Range(fpsCamera.localRotation.z - amplitudeZmin, fpsCamera.localRotation.z + amplitudeZmax);
            Quaternion spread = new Quaternion(x, fpsCamera.localRotation.y, z, fpsCamera.localRotation.w);
            fpsCamera.localRotation = Quaternion.Slerp(fpsCamera.localRotation, spread, force * Time.deltaTime);
            if (!lockCamera)
                mouseLook.m_CameraTargetRot.x -= speed * Time.deltaTime;
        }
        
        /// <summary>
        /// Spreading bullet
        /// </summary>
        /// <param name="attackPoint"></param>
        /// <param name="weaponAnimation"></param>
        public virtual void BulletSpreadProcessing(Transform attackPoint, WeaponAnimationSystem weaponAnimation)
        {
            for (int i = 0; i < bulletSpreadParams.Count; i++)
            {
                if (bulletSpreadParams[i].state == weaponAnimation.ActiveState)
                {
                    attackPoint.localRotation = Quaternion.Euler(Random.Range(bulletSpreadParams[i].minX, bulletSpreadParams[i].maxX), Random.Range(bulletSpreadParams[i].minY, bulletSpreadParams[i].maxY), 0);
                }
            } 
        }

        public float AmplitudeX
        {
            get
            {
                return amplitudeX;
            }

            set
            {
                amplitudeX = value;
            }
        }

        public float AmplitudeZmin
        {
            get
            {
                return amplitudeZmin;
            }

            set
            {
                amplitudeZmin = value;
            }
        }

        public float AmplitudeZmax
        {
            get
            {
                return amplitudeZmax;
            }

            set
            {
                amplitudeZmax = value;
            }
        }

        public float Force
        {
            get
            {
                return force;
            }

            set
            {
                force = value;
            }
        }

        public bool LockCamera
        {
            get
            {
                return lockCamera;
            }

            set
            {
                lockCamera = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        public List<BulletSpreadParam> BulletSpreadParams
        {
            get
            {
                return bulletSpreadParams;
            }

            set
            {
                bulletSpreadParams = value;
            }
        }

        public Transform FPSCamera
        {
            get
            {
                return fpsCamera;
            }

            set
            {
                fpsCamera = value;
            }
        }

        public NGMouseLook MouseLook
        {
            get
            {
                return mouseLook;
            }

            set
            {
                mouseLook = value;
            }
        }
    }
}