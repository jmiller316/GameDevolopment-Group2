
using UnityEngine;

namespace UnrealFPS
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private FPController playerController;
        [SerializeField] private Transform menuCanvas;
        [SerializeField] private Transform hudCanvas; 
        [SerializeField] private bool pauseGame; 
        [SerializeField] private bool isActive = false; 

        private IHealth playerHealth;

        protected virtual void Start()
        {
            playerHealth = playerController.GetComponent<IHealth>();
        }

        protected virtual void Update()
        {
            if (SimpleInputManager.GetMenu())
            {
                isActive = !isActive;
                menuCanvas.gameObject.SetActive(isActive);
                hudCanvas.gameObject.SetActive(!isActive);
                if (playerHealth.IsAlive)
                    playerController.LockMovement = isActive;
                playerController.MouseLook.SetCursorLock(!isActive);
                if (pauseGame) Time.timeScale = (isActive) ? 0 : 1;
            }
        }


        public Transform MenuCanvas { get { return menuCanvas; } set { menuCanvas = value; } }


        public Transform HUDCanvas { get { return hudCanvas; } set { hudCanvas = value; } }
    }
}