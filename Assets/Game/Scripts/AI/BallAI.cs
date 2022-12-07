using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace BumperBallGame
{
    public class BallAI : MonoBehaviour
    {

        private BallPhysics ballPhysics;
        private FieldOfView fov;

        [SerializeField]
        private Transform[] targets;

        private void Awake()
        {
            fov = GetComponent<FieldOfView>();
            ballPhysics = GetComponent<BallPhysics>();
        }

        private void Update()
        {
            Move();
        }


        public void Move()
        {
            if (fov.canSeeBorder)
            {
                ballPhysics.CurrentMove = (transform.position - fov.borderRef.position).normalized;
            }
            else
            {
                if (fov.canSeeTarget)
                {
                    ballPhysics.CurrentMove = -(transform.position - fov.targetRef.position).normalized;
                }
                else
                {
                    ballPhysics.CurrentMove = Vector3.zero;
                }
            }
            ballPhysics.CurrentMove = new Vector3(ballPhysics.CurrentMove.x, 0.0f, ballPhysics.CurrentMove.z);
        }

    }
}