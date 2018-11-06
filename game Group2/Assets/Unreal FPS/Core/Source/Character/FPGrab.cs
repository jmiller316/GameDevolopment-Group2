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
using UnityEngine;

namespace UnrealFPS
{
    [Serializable]
    public class FPGrab
    {
        #region Private SerializeField Variable
        [SerializeField] private float range;
        [SerializeField] private float rangeObject;
        [SerializeField] private float throwForce;
        [SerializeField] private bool useIsKinematic;
        #endregion

        #region Private Variable
        private Transform camera;
        private PlayerInventory playerInventory;
        private GameObject grabObject;
        private RaycastHit raycastHit;
        private bool isGrab;
        #endregion

        /// <summary>
        /// Initialize the required components
        /// </summary>
        /// <param name="player"></param>
        /// <param name="camera"></param>
        /// <param name="playerInventory"></param>
        public void Init(Transform camera, PlayerInventory playerInventory)
        {
            this.camera = camera;
            this.playerInventory = playerInventory;
            isGrab = false;
        }

        /// <summary>
        /// Grabbing Handler
        /// </summary>
        public void Grabbing()
        {
            if (SimpleInputManager.GetInteract2())
            {
                if (grabObject == null && !isGrab)
                {
                    if (Physics.Raycast(camera.position, camera.forward, out raycastHit, range))
                    {
                        if (raycastHit.transform.GetComponent<Marking>() != null && raycastHit.transform.GetComponent<Marking>().CompareMark(MarkItem.Grab))
                        {
                            grabObject = raycastHit.transform.gameObject;
                            grabObject.GetComponent<Rigidbody>().useGravity = false;
                            if (useIsKinematic)
                            {
                                grabObject.GetComponent<Rigidbody>().isKinematic = true;
                            }
                            else
                            {
                                grabObject.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            isGrab = true;
                        }
                    }
                }
                else if (grabObject != null && isGrab)
                {
                    Rigidbody rb = grabObject.GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    grabObject = null;
                    rb = null;
                    isGrab = false;
                }
            }

            if ((grabObject != null && isGrab) && SimpleInputManager.GetFireLong())
            {
                Rigidbody rb = grabObject.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.AddForce(camera.forward * throwForce, ForceMode.Impulse);
                grabObject = null;
                rb = null;
                isGrab = false;
            }

            if (grabObject != null)
            {
                playerInventory.DeactivateActiveWeapon();
                Vector3 newVectorPosition = camera.position + camera.transform.forward * rangeObject;
                grabObject.transform.position = Vector3.Lerp(grabObject.transform.position, newVectorPosition, 7 * Time.deltaTime);
            }
        }
    }
}