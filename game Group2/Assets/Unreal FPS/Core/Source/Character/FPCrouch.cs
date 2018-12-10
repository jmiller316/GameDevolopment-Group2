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
    [Serializable]
    public class FPCrouch
    {
        #region Private SerializeField Variable
        [SerializeField] private float speed;
        [SerializeField] private float smooth;
        [SerializeField] private float crouchHeight;
        #endregion

        #region Private Variable
        private Transform player;
        private CharacterController characterController;
        private float wasControllerHeight;
        private bool isCrouch;
        #endregion

        /// <summary>
        /// Initialize the required components
        /// </summary>
        /// <param name="player"></param>
        /// <param name="key_crouch"></param>
        public void Init(Transform player, CharacterController characterController)
        {
            this.player = player;
            this.characterController = characterController;
            wasControllerHeight = characterController.height;
        }

        /// <summary>
        /// Player Crouch Handler
        /// </summary>
        public void UpdateCrouch()
        {
            float fpHeight = wasControllerHeight;
            isCrouch = SimpleInputManager.GetCrouch();

            if (isCrouch)
                fpHeight = wasControllerHeight * crouchHeight;

            float lastFPHeight = characterController.height;
            characterController.height = Mathf.Lerp(characterController.height, fpHeight, smooth * Time.deltaTime);
            float fixedVerticalPosition = player.position.y + (characterController.height - lastFPHeight) / 2;
            player.position = new Vector3(player.position.x, fixedVerticalPosition, player.position.z);
        }

        /// <summary>
        /// Crouch smooth
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsCrouch
        {
            get
            {
                return isCrouch;
            }

            set
            {
                isCrouch = value;
            }
        }

        /// <summary>
        /// Height when player is crouching
        /// </summary>
        public float CrouchHeight
        {
            get
            {
                return crouchHeight;
            }

            set
            {
                crouchHeight = value;
            }
        }

        public float Smooth { get { return smooth; } }
    }
}