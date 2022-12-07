using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BumperBallGame
{
    public class PlayerController : MonoBehaviour
    {
        private BallPhysics ballPhysics;


        private void Awake()
        {
            ballPhysics = GetComponent<BallPhysics>();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            // This returns Vector2.zero when context.canceled
            // is true, so no need to handle these separately.
            Vector2 move = context.ReadValue<Vector2>();
            ballPhysics.CurrentMove = new Vector3(move.x, 0.0f, move.y);
        }


    }
}