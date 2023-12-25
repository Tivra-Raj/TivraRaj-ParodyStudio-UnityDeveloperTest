using Services;
using UnityEngine;

namespace Player
{
    public class PlayerController
    {
        private PlayerView playerView;
        private PlayerModel playerModel;

        private float horizontalInput;
        private float verticalInput;
        private Vector3 moveDirection;

        private Vector3 gravityDirection = Vector3.down;

        private int pointCubeCollected;
        public int TotalPointCubeCollected { get => pointCubeCollected; set => pointCubeCollected = value; }

        public PlayerController(PlayerView playerView, PlayerModel playerData)
        {
            this.playerView = playerView;
            this.playerView.SetPlayerController(this);

            this.playerModel = playerData;
            this.playerModel.SetPlayerController(this);

            EventService.Instance.OnPoinCubeColletedEvent.AddListener(OnPointCubeCollected);
        }
        
        ~PlayerController()
        {
            EventService.Instance.OnPoinCubeColletedEvent.RemoveListener(OnPointCubeCollected);
        }


        #region PlayerMovement
        public void HandleMovementInput()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");                                                                                                        
        }

        public void HandlePlayerMovement(Rigidbody playerRigidbody, Transform mainCamera)
        {
            moveDirection = mainCamera.forward * verticalInput;
            moveDirection += mainCamera.right * horizontalInput;
            moveDirection.y = 0;
            moveDirection.Normalize();
            moveDirection *= playerModel.movementSpeed * Time.deltaTime;

            Vector3 movementVelocity = moveDirection;
            playerRigidbody.velocity = movementVelocity;
        }

        public void HandlePlayerRotation(Transform mainCamera)
        {
            Vector3 targetDirection = Vector3.zero;

            targetDirection = mainCamera.forward * verticalInput;
            targetDirection += mainCamera.right * horizontalInput;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if(targetDirection == Vector3.zero)
            {
                targetDirection = playerView.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(playerView.transform.rotation, targetRotation, playerModel.rotationSpeed * Time.deltaTime);
        
            playerView.transform.rotation = playerRotation;
        }
        #endregion

        #region PlayerJump
        public void HandlePlayerJump(Rigidbody playerRigidbody, Transform transform)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded(transform))
            {
                Vector3 jumpForceDirection = -gravityDirection;
                playerRigidbody.AddForce(jumpForceDirection * playerModel.jumpForce, ForceMode.Impulse);
            }
        }

        bool IsGrounded(Transform transform)
        {
            bool grounded = Physics.Raycast(transform.position, gravityDirection, playerModel.isGroundedDetectionLength);
            return grounded;
        }

        public void VisualizeGroundCheck(Transform transform)
        {
            Debug.DrawRay(transform.position, gravityDirection * playerModel.isGroundedDetectionLength, Color.yellow);
        }
        #endregion

        #region PlayerAnimation
        public void HandlePlayerAnimation(Animator playerAnimator, Transform transform)
        {
            bool isRunning = playerAnimator.GetBool(playerView.isRunningAnimationParameter);
            bool isFalling = playerAnimator.GetBool(playerView.isFallingAnimationParameter);

            if (horizontalInput != 0 || verticalInput != 0 && !isRunning)
            {
                playerAnimator.SetBool(playerView.isRunningAnimationParameter, true);
            }
            else if (horizontalInput == 0 && verticalInput == 0 && isRunning)
            {
                playerAnimator.SetBool(playerView.isRunningAnimationParameter, false);
            }
                
            if (!IsGrounded(transform) && !isFalling)
            {
                playerAnimator.SetBool(playerView.isFallingAnimationParameter, true);
            }
            else if (IsGrounded(transform) && isFalling)
            {
                playerAnimator.SetBool(playerView.isFallingAnimationParameter, false);
            }

        }
        #endregion

        #region HandleGravityManipulation

        public void HandleChangeGravityInput(Rigidbody playerRigidbody)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetGravity(Vector3.right, playerRigidbody);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetGravity(Vector3.left, playerRigidbody);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetGravity(Vector3.forward, playerRigidbody);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetGravity(Vector3.back, playerRigidbody);
            }
        }

        private void SetGravity(Vector3 direction, Rigidbody playerRigidbody)
        {
            gravityDirection = direction;

            Quaternion targetRotation = Quaternion.LookRotation(-gravityDirection, Vector3.up);
            playerRigidbody.rotation = targetRotation;
        }

        public void ChangeGravity(Rigidbody playerRigidbody)
        {
            Vector3 gravityForce = gravityDirection * playerView.gravityMultiplier;
            playerRigidbody.AddForce(gravityForce, ForceMode.Acceleration);
        }
        #endregion


        private void OnPointCubeCollected(int value)
        {
            TotalPointCubeCollected = value;
            GameService.Instance.GetGameUI().UpdateTotalPointCubeCollectedText(value);
        }

        public bool PlayerDeath(Transform transform, LayerMask grounLayer)
        {
            return (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 50f, grounLayer));
        }
    }
}