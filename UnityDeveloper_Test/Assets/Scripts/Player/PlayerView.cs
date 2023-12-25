using Interactables;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerView : MonoBehaviour
    {
        public PlayerController PlayerController { get; private set; }

        private Transform mainCamera;
        private Rigidbody playerRigidbody;
        private Animator playerAnimator;

        public string isRunningAnimationParameter = "";
        public string isFallingAnimationParameter = "";

        public float gravityMultiplier = 1.0f;

        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            playerAnimator = GetComponent<Animator>();
            mainCamera = Camera.main.transform;
            playerRigidbody.useGravity = false;
        }

        private void Update()
        {
            PlayerController.HandleMovementInput();

            PlayerController.HandlePlayerJump(playerRigidbody, transform);
            PlayerController.HandlePlayerAnimation(playerAnimator, transform);

            PlayerController.HandleChangeGravityInput(playerRigidbody);
            PlayerController.VisualizeGroundCheck(transform);
        }

        private void FixedUpdate()
        {
            PlayerController.HandlePlayerMovement(playerRigidbody, mainCamera);
            PlayerController.HandlePlayerRotation(mainCamera);

            PlayerController.ChangeGravity(playerRigidbody);
        }

        public void SetPlayerController(PlayerController playerController)
        {
            PlayerController = playerController;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }

    }
}