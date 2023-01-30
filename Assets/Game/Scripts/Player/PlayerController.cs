using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        private BallController ball;


        private void Awake()
        {
            ball = GetComponent<BallController>();
        }

        private void Start()
        {
            
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            // This returns Vector2.zero when context.canceled
            // is true, so no need to handle these separately.
            Vector2 move = context.ReadValue<Vector2>();
            ball.CurrentMove = new Vector3(move.x, 0.0f, move.y).normalized;
            /*if (context.control.parent is Keyboard)
            {
                ball.CurrentMove = new Vector3(move.x, 0.0f, move.y);
            }
            else
            {
                ball.CurrentMove = new Vector3(move.x, 0.0f, move.y).normalized;
                ball.CurrentMove = ball.CurrentMove * 1.2f;
            }*/

        }
    }
}