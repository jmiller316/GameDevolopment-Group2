using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS
{
    /// <summary>
    /// Base First Person Controller class
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody))]
    public class FPController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip m_JumpSound;
        [SerializeField] private AudioClip m_LandSound;
        [SerializeField] private Camera m_Camera;
        [SerializeField] private NGMouseLook m_MouseLook;
        [SerializeField] private NGCurveControlledBob m_HeadBob = new NGCurveControlledBob();
        [SerializeField] private NGLerpControlledBob m_JumpBob = new NGLerpControlledBob();
        [SerializeField] public List<FootstepSurface> surfaceList = new List<FootstepSurface>();
        [SerializeField] private FPCrouch m_FPCrouch = new FPCrouch();
        [SerializeField] private FPClimb m_FPClimb = new FPClimb();
        [SerializeField] private FPGrab m_FPGrab = new FPGrab();
        [SerializeField] private PickupWeapon m_PickUpWeapon = new PickupWeapon();

        private bool lockMovement;
        private float fpSpeed;
        private bool m_Jump;
        private float wasWalkSpeed;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private int m_RandomSound;
        private PlayerInventory m_PlayerInventory;
        private string m_ColliderMaterialName;
        private Rigidbody m_Rigidbody;
        private float m_CameraRotation;


        protected virtual void Start()
        {
            wasWalkSpeed = m_WalkSpeed;
            m_CharacterController = GetComponent<CharacterController>();
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_PlayerInventory = GetComponent<PlayerInventory>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_MouseLook.Init(transform, m_Camera.transform);
            m_FPCrouch.Init(transform, m_CharacterController);
            m_FPClimb.Init(transform, m_Camera, m_CharacterController, m_AudioSource);
            m_FPGrab.Init(m_Camera.transform, m_PlayerInventory);
            m_PickUpWeapon.Init(transform, m_PlayerInventory);
        }


        protected virtual void Update()
        {
            if (lockMovement)
                return;

            RotateView();
            m_FPGrab.Grabbing();

            m_FPCrouch.UpdateCrouch();

            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump && !m_FPClimb.OnLadder)
            {
				m_Jump = SimpleInputManager.IsJumping();
			}

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded && !m_FPCrouch.IsCrouch)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded && !m_FPClimb.OnLadder)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            if (m_FPClimb.OnLadder)
            {
                StopAllCoroutines();
                m_Rigidbody.useGravity = false;
                m_Rigidbody.isKinematic = true;
                m_FPClimb.Climbing(m_ColliderMaterialName);
            }
            else
            {
                m_Rigidbody.useGravity = true;
                m_Rigidbody.isKinematic = true;
            }
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }
        
        protected virtual void FixedUpdate(){
            if (lockMovement)
                return;

            m_PickUpWeapon.Handler();

            float speed;
            GetInput(out speed);
            if (!m_FPClimb.OnLadder) { 
                Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                    m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;

                if (m_CharacterController.isGrounded) {
                    m_MoveDir.y = -m_StickToGroundForce;

                    if (m_Jump) {
                        m_MoveDir.y = m_JumpSpeed;
                        PlayJumpSound();
                        m_Jump = false;
                        m_Jumping = true;
                    }
                }
                else {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

                ProgressStepCycle(speed);
            }
            UpdateCameraPosition(speed);

        }


        private void PlayJumpSound(){
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed){
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                    Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio() {
            if (!m_CharacterController.isGrounded){
                return;
            }
            FootstepsSoundSystem.Play(surfaceList, transform, m_AudioSource);
        }


        private void UpdateCameraPosition(float speed){
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                        (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }

        private void GetInput(out float speed){
            // Read input

            float horizontal = SimpleInputManager.GetHorizontal();
			float vertical = SimpleInputManager.GetVertical();

            bool waswalking = m_IsWalking;
			m_IsWalking = !SimpleInputManager.IsRunning();

            m_WalkSpeed = m_FPCrouch.IsCrouch ? m_FPCrouch.Speed : wasWalkSpeed;
            speed = (m_IsWalking || (vertical < 0) || (horizontal != 0) || m_FPCrouch.IsCrouch || SimpleInputManager.GetAim()) ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);
            fpSpeed = speed;
            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1){
                m_Input.Normalize();
            }

            if (m_IsWalking != waswalking) {
                StopAllCoroutines();
            }

        }

		private void RotateView(){
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit){
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below){
                return;
            }

            if (body == null || body.isKinematic){
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }


        protected virtual void OnTriggerEnter(Collider enterCollider){
            Marking marking = enterCollider.GetComponent<Marking>();
            if (marking != null){

            }
        }

        protected virtual void OnTriggerStay(Collider stayCollider){
            if (stayCollider.tag == "Ladder" && m_FPClimb.UseLadder){
                if (!m_FPClimb.UseWeapon){
                    m_PlayerInventory.DeactivateActiveWeapon();
                }
                m_FPClimb.OnLadder = true;
            }

            if (stayCollider.GetComponent<BoxCollider>().sharedMaterial != null){
                m_ColliderMaterialName = stayCollider.GetComponent<BoxCollider>().sharedMaterial.name;
            }
            else{
                m_ColliderMaterialName = null;
            }
        }

        protected virtual void OnTriggerExit(Collider exitCollider){
            if (exitCollider.tag == "Ladder"){
                m_FPClimb.OnLadder = false;
                m_FPClimb.UseLadder = true;
            }
        }

        public bool IsWalking{
            get{
                return (fpSpeed == m_WalkSpeed) ? true : false;
            }
        }

        public bool IsRunning{
            get{
                return (fpSpeed == m_RunSpeed) ? true : false;
            }
        }

        public Camera Camera{
            get{
                return m_Camera;
            }

            set{
                m_Camera = value;
            }
        }

        public CharacterController FPCharacterController { get { return m_CharacterController; } }

        public bool LockMovement { get { return lockMovement; } set { lockMovement = value; } }

        public FPCrouch FPCrouch{
            get{
                return m_FPCrouch;
            }
        }
        
        public FPClimb FPClimb{
            get{
                return m_FPClimb;
            }
        }

        public FPGrab FPGrab{
            get{
                return m_FPGrab;
            }
        }

        public List<FootstepSurface> SurfaceList{
            get{
                return surfaceList;
            }

            set{
                surfaceList = value;
            }
        }

        public NGMouseLook MouseLook{
            get{
                return m_MouseLook;
            }
        }
    }
}