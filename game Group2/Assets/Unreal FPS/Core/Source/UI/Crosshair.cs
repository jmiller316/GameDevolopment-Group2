/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnrealFPS.UI
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct CrosshairParam
    {
        public string state;
        public float force;
        public float speed;
    }

    /// <summary>
    /// 
    /// </summary>
    public struct CrosshairVectors
    {
        public Vector3 up;
        public Vector3 down;
        public Vector3 left;
        public Vector3 right;
    }

    /// <summary>
    /// 
    /// </summary>
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory; 

        [SerializeField] private Image up;
        [SerializeField] private Image down;
        [SerializeField] private Image left;
        [SerializeField] private Image right;
        [SerializeField] private List<CrosshairParam> crosshairParam = new List<CrosshairParam>();

        private List<CrosshairVectors> crosshairVectors = new List<CrosshairVectors>();
        private WeaponAnimationSystem weaponAnimationSystem;
        private string lastState;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < crosshairParam.Count; i++)
            {
                crosshairVectors.Add(new CrosshairVectors()
                {
                    up = up.rectTransform.localPosition * crosshairParam[i].force,
                        down = down.rectTransform.localPosition * crosshairParam[i].force,
                        left = left.rectTransform.localPosition * crosshairParam[i].force,
                        right = right.rectTransform.localPosition * crosshairParam[i].force
                });
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (inventory.GetActiveWeapon() == null)
                return;

            if (inventory.IsSelect)
                weaponAnimationSystem = null;

            if (weaponAnimationSystem == null)
                weaponAnimationSystem = (inventory.GetActiveWeapon().GetComponent<WeaponAnimationSystem>() != null) ? inventory.GetActiveWeapon().GetComponent<WeaponAnimationSystem>() : null;

            if (weaponAnimationSystem == null)
                return;

            Handler(weaponAnimationSystem.ActiveState);
            SetActive(!SimpleInputManager.GetAim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void Handler(string state)
        {
            if (lastState != state)
            {
                for (int i = 0; i < crosshairParam.Count; i++)
                {
                    if (state == crosshairParam[i].state)
                    {
                        up.rectTransform.localPosition = Vector3.MoveTowards(up.rectTransform.localPosition, crosshairVectors[i].up, crosshairParam[i].speed * Time.deltaTime);
                        down.rectTransform.localPosition = Vector3.MoveTowards(down.rectTransform.localPosition, crosshairVectors[i].down, crosshairParam[i].speed * Time.deltaTime);
                        left.rectTransform.localPosition = Vector3.MoveTowards(left.rectTransform.localPosition, crosshairVectors[i].left, crosshairParam[i].speed * Time.deltaTime);
                        right.rectTransform.localPosition = Vector3.MoveTowards(right.rectTransform.localPosition, crosshairVectors[i].right, crosshairParam[i].speed * Time.deltaTime);
                        if (up.rectTransform.localPosition == crosshairVectors[i].up)
                        {
                            lastState = state;
                        }
                    }
                }
            }
        }

        private void SetActive(bool active)
        {
            up.gameObject.SetActive(active);
            down.gameObject.SetActive(active);
            left.gameObject.SetActive(active);
            right.gameObject.SetActive(active);
        }

    }
}