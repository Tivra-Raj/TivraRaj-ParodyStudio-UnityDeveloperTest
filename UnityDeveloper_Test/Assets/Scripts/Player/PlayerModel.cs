using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject", order = 1)]
    public class PlayerModel : ScriptableObject
    {
        [Header("Player Movement:")]
        public float isGroundedDetectionLength;
        public float jumpForce;
        public float movementSpeed;
        public float rotationSpeed;

        public PlayerController PlayerController { get; private set; }

        public void SetPlayerController(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }
}