using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace BumperBallGame
{
    public class AIController : MonoBehaviour
    {

        private BallController ball;
        private FieldOfView fov;


        [SerializeField]
        private Transform[] targets;

        private void Awake()
        {
            fov = GetComponent<FieldOfView>();
            ball = GetComponent<BallController>();
        }

        private void Start()
        {
            StartCoroutine(GoIdle());
        }

        private void OnEnable()
        {
            StartCoroutine(GoIdle());
        }

        private void Update()
        {
            CalculateMovement();        
        }

        private IEnumerator GoIdle()
        {
            WaitForSeconds wait = new WaitForSeconds(Random.Range(0.1f,0.3f));

            while (true)
            {
                yield return wait;
                ball.CurrentMove = Vector3.zero;
            }
        }


        public void CalculateMovement()
        {
            if (!fov.canSeeCenter)
            {
                ball.CurrentMove = -(transform.position - fov.centerRef.position).normalized;
            }
            else
            {
                if (fov.canSeeTarget)
                {
                    ball.CurrentMove = -(transform.position - fov.targetRef.position).normalized;
                }
                else
                {
                    ball.CurrentMove = -(transform.position - fov.centerRef.position).normalized;
                }
            }
            ball.CurrentMove = new Vector3(ball.CurrentMove.x, 0.0f, ball.CurrentMove.z);
        }

    }
}