
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS
{
    [Serializable]
    public class FPClimb
    {
        #region Private SerializeField Variable
        [SerializeField] private float speed;
        [SerializeField] private float playSoundCycle;
        [SerializeField] private float playTime;
        [SerializeField] private bool useWeapon;
        [SerializeField] private List<FootstepSurface> surfaceList = new List<FootstepSurface>();
        #endregion

        #region Private Variable
        private Transform playerTransform;
        private Camera camera;
        private CharacterController characterController;
        private AudioSource audioSource;
        private float cameraRotation;
        private float wasPlaySoundCycle;
        private float downThreshold;
        private bool onLadder;
        private bool useLadder;
        private Vector3 climbDirection;
        private Vector3 lateralMove;
        private Vector3 ladderMovement;
        #endregion

        /// <summary>
        /// Initialize the required components
        /// </summary>
        /// <param name="playerTransform"></param>
        /// <param name="camera"></param>
        /// <param name="characterController"></param>
        /// <param name="audioSource"></param>
        /// <param name="surfaceHandler"></param>
        public void Init(Transform playerTransform, Camera camera, CharacterController characterController, AudioSource audioSource)
        {
            this.playerTransform = playerTransform;
            this.camera = camera;
            this.characterController = characterController;
            this.audioSource = audioSource;
            wasPlaySoundCycle = playSoundCycle;
            downThreshold = -0.4f;
            climbDirection = Vector3.up;
            lateralMove = Vector3.zero;
            ladderMovement = Vector3.zero;
            onLadder = false;
            useLadder = true;
        }

        /// <summary>
        /// Climb Handler
        /// </summary>
        public void Climbing(string colliderMaterial)
        {
            cameraRotation = camera.transform.forward.y;
            if (onLadder)
            {
                Vector3 verticalMove;
                verticalMove = climbDirection.normalized;
                verticalMove *= SimpleInputManager.GetVertical();
                verticalMove *= (cameraRotation > downThreshold) ? 1 : -1;
                lateralMove = new Vector3(SimpleInputManager.GetHorizontal(), 0, SimpleInputManager.GetVertical());
                lateralMove = playerTransform.TransformDirection(lateralMove);
                ladderMovement = verticalMove + lateralMove;
                characterController.Move(ladderMovement * speed * Time.deltaTime);

                if (SimpleInputManager.GetVertical() == 1 && Time.time >= playTime && playSoundCycle >= 0)
                {
                    playSoundCycle -= Time.deltaTime;
                    if (playSoundCycle <= 0)
                    {
                        FootstepsSoundSystem.Play(surfaceList, colliderMaterial, audioSource);
                        playSoundCycle = wasPlaySoundCycle;
                    }

                }

                if (SimpleInputManager.IsJumping())
                {
                    useLadder = false;
                    onLadder = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool OnLadder
        {
            get
            {
                return onLadder;
            }

            set
            {
                onLadder = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UseLadder
        {
            get
            {
                return useLadder;
            }

            set
            {
                useLadder = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FootstepSurface> SurfaceList
        {
            get
            {
                return surfaceList;
            }

            set
            {
                surfaceList = value;
            }
        }

        /// <summary>
        /// Use weapon when player on ladder
        /// </summary>
        public bool UseWeapon
        {
            get
            {
                return useWeapon;
            }

            set
            {
                useWeapon = value;
            }
        }
    }
}