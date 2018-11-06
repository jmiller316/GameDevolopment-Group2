using UnityEngine;

namespace UnrealFPS
{
    [System.Serializable]
    public class InverseKinematics
    {
        #region [Variables are editable in the inspector]
        [SerializeField] private Transform leftFootPivot;
        [SerializeField] private Transform rightFootPivot;
        [SerializeField] private float offsetY;
        [SerializeField] private Transform lookTarget;
        [SerializeField] private float upperBodyIKWeight;
        [SerializeField] private float bodyIKWeight;
        [SerializeField] private float headIKWeight;
        [SerializeField] private float eyesIKWeight;
        [SerializeField] private float clampWeight;
        [SerializeField] private bool active;
        [SerializeField] private bool footIKActive;
        [SerializeField] private bool headIKActive;
        #endregion

        #region [Required variables]
        private Animator animator;
        private float leftFootWeight;
        private float rightFootWeight;
        private Transform leftFoot;
        private Transform rightFoot;
        private LayerMask ignoreLayer;
        #endregion

        #region [Functions]
        /// <summary>
        /// Init is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        /// <param name="animator">Interface to control the Mecanim animation system.</param>
        public void Init(Animator animator)
        {
            this.animator = animator;
            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            ignoreLayer = ~1 << LayerMask.NameToLayer("Player");
        }

        /// <summary>
        /// Foot IK system
        /// </summary>
        public virtual void FootIK()
        {
            //If the IK is not active, set the position and rotation of the hand and head back to the original position.
            if (!active || !footIKActive)
                return;

            leftFootWeight = animator.GetFloat("IK Left Foot");
            rightFootWeight = animator.GetFloat("IK Right Foot");

            //Activate IK, set the rotation directly to the goal.
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);

            RaycastHit leftHit;
            if (Physics.Raycast(leftFootPivot.position, -Vector3.up, out leftHit, ignoreLayer))
            {
                Quaternion ikRotation = Quaternion.FromToRotation(leftFoot.up, leftHit.normal) * leftFoot.rotation;
                ikRotation = new Quaternion(ikRotation.x, leftFoot.rotation.y, ikRotation.z, ikRotation.w);
                Vector3 ikPosition = new Vector3(leftFoot.position.x, leftHit.point.y, leftFoot.position.z);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikPosition + (Vector3.up * offsetY));
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, ikRotation);
            }

            RaycastHit rightHit;
            if (Physics.Raycast(rightFootPivot.position, -Vector3.up, out rightHit, ignoreLayer))
            {
                Quaternion ikRotation = Quaternion.FromToRotation(rightFoot.up, rightHit.normal) * rightFoot.rotation;
                ikRotation = new Quaternion(ikRotation.x, rightFoot.rotation.y, ikRotation.z, ikRotation.w);
                Vector3 ikPosition = new Vector3(rightFoot.position.x, rightHit.point.y, rightFoot.position.z);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, ikPosition + (Vector3.up * offsetY));
                animator.SetIKRotation(AvatarIKGoal.RightFoot, ikRotation);
            }
        }

        /// <summary>
        /// Head IK system
        /// </summary>
        public virtual void HeadIK()
        {
            if (!active || !headIKActive)
                return;

            animator.SetLookAtWeight(upperBodyIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampWeight);
            animator.SetLookAtPosition(lookTarget.position);
        }

        /// <summary>
        /// Sets Global IK system active state
        /// 	True: IK system is active, however, subsystems can be disabled.
        /// 	False: Completely disables the IK system and all its subsystems.
        /// </summary>
        /// <param name="active">bool value</param>
        public void SetActive(bool active)
        {
            this.active = active;
        }

        /// <summary>
        /// Set foot IK system
        /// </summary>
        /// <param name="active">bool value</param>
        public void SetFootIKActive(bool active)
        {
            this.footIKActive = active;
        }

        /// <summary>
        /// Set head IK system
        /// </summary>
        /// <param name="active">bool value</param>
        public void SetHeadIKActive(bool active)
        {
            this.headIKActive = active;
        }

        /// <summary>
        /// Update look at position target
        /// </summary>
        /// <param name="target"></param>
        public void UpdateTarget(Transform target)
        {
            lookTarget = target;
        }
        #endregion

        #region [Properties]
        /// <summary>
        /// Global IK system active state
        /// </summary>
        /// <returns></returns>
        public bool Active { get { return active; } }

        /// <summary>
        /// Foot IK system active state
        /// </summary>
        /// <returns></returns>
        public bool FootIKActive { get { return footIKActive; } }

        /// <summary>
        /// Head IK system active state
        /// </summary>
        /// <returns></returns>
        public bool HeadIKActive { get { return headIKActive; } }

        public Transform LookTarget { get { return lookTarget; } set { lookTarget = value; } }
        #endregion
    }
}