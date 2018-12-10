

using UnityEngine;

namespace UnrealFPS
{
	[RequireComponent(typeof(Animator))]
	public class FPSBody : MonoBehaviour
	{
		[SerializeField] private float default_Y;
		[SerializeField] private float crouch_Y;
		[SerializeField] private InverseKinematics inverseKinematics = new InverseKinematics();

		private FPController controller;
		private Animator animator;

		private void Start()
		{
			animator = GetComponent<Animator>();
			controller = transform.root.GetComponent<FPController>();
			inverseKinematics.Init(animator);
		}


		private void Update()
		{
			float speed = SimpleInputManager.GetVertical();
			float direction = SimpleInputManager.GetHorizontal();
			float amount = Mathf.Clamp01(Mathf.Abs(speed) + Mathf.Abs(direction));

			if (amount > 0)
			{
				if (controller.IsRunning)
					speed = 2;
				animator.SetFloat("Speed", speed);
				animator.SetFloat("Direction", direction);
			}if (amount == 0) {
                animator.SetFloat("Speed", 0);
                animator.SetFloat("Direction", 0);
            }

			animator.SetBool("IsCrouching", controller.FPCrouch.IsCrouch);
			float fixedVerticalPosition;
			if (controller.FPCrouch.IsCrouch)
				fixedVerticalPosition = Mathf.MoveTowards(transform.localPosition.y, crouch_Y, 7 * Time.deltaTime);
			else
				fixedVerticalPosition = Mathf.MoveTowards(transform.localPosition.y, default_Y, 7 * Time.deltaTime);
			transform.localPosition = new Vector3(transform.localPosition.x, fixedVerticalPosition, transform.localPosition.z);
		}


		void OnAnimatorIK(int layerIndex)
		{
			inverseKinematics.FootIK();
		}
	}
}