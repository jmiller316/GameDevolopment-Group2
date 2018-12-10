/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace UnrealFPS
{
    /// <summary>
    /// Kill Cam is a class used for handle the camera after the death of the player
    /// </summary>
    [Serializable]
    public class KillCam
    {

        [SerializeField] private Transform killCamera;
        [SerializeField] private float radius;
        [SerializeField] private float minDistance;
        [SerializeField] private GameObject playerModel;
        [SerializeField] private GameObject _FPSBody;
        [SerializeField] private Transform lookAt;

        private CharacterController characterController;
        private Transform playerCamera;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="characterController"></param>
        public virtual void Init(Transform _FPSCamera, CharacterController characterController)
        {
            this.characterController = characterController;
            playerCamera = _FPSCamera;
        }

        /// <summary>
        /// Start KillCam processing 
        /// </summary>
        public virtual void Play()
        {
            DisableInterferingComponents();
            EnableRequiredComponents();
            Collider[] collider = Physics.OverlapSphere(killCamera.position, radius);
            for (int i = 0; i < collider.Length; i++)
            {
                if (killCamera.root != collider[i].transform && (Vector3.Distance(killCamera.position, collider[i].transform.position) > minDistance))
                {
                    Vector3 dir = killCamera.position - collider[i].transform.position;
                    killCamera.position = Vector3.MoveTowards(killCamera.position, (killCamera.position + dir * 2), 5 * Time.deltaTime);
                }
            }
            Quaternion targetRotation = Quaternion.LookRotation(lookAt.position - killCamera.position);
            killCamera.rotation = Quaternion.Slerp(killCamera.rotation, targetRotation, 2 * Time.deltaTime);
        }

        /// <summary>
        /// Reset components on deflaut settigns
        /// </summary>
        public virtual void Reset()
        {
            killCamera.gameObject.SetActive(false);
            SetKinematic(true);
            playerModel.SetActive(false);
            characterController.enabled = true;
            playerCamera.gameObject.SetActive(true);
            _FPSBody.SetActive(true);
        }

        /// <summary>
        /// Disable Interfering Components
        /// </summary>
        public virtual void DisableInterferingComponents()
        {
            characterController.enabled = false;
            _FPSBody.SetActive(false);
            playerCamera.gameObject.SetActive(false);
            SetKinematic(false);
        }

        /// <summary>
        /// Enable Required Components
        /// </summary>
        public virtual void EnableRequiredComponents()
        {
            killCamera.gameObject.SetActive(true);
            playerModel.SetActive(true);
        }

        /// <summary>
        /// Enable/Disable Kinematic on Player bones
        /// </summary>
        /// <param name="active"></param>
        private void SetKinematic(bool active)
        {
            Rigidbody[] rigidbody = playerModel.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigidbody.Length; i++)
            {
                rigidbody[i].isKinematic = active;
            }
        }

        public Transform Camera { get { return killCamera; } set { killCamera = value; } }

        public float Radius { get { return radius; } set { radius = value; } }

        public GameObject PlayerModel { get { return playerModel; } set { playerModel = value; } }
    }
}