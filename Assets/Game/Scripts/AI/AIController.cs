using Game.Persistence;
using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public class AIController : MonoBehaviour
    {

        private BallController ball;
        private FieldOfView fov;
        private float idleLimit;


        [SerializeField]
        private Transform[] targets;

        private void Awake()
        {
            fov = GetComponent<FieldOfView>();
            ball = GetComponent<BallController>();
        }

        private void Start()
        {
            switch (GameData.difficulty)
            {
                case Gameplay.Difficulty.EASY: idleLimit = 0.0f; break;
                case Gameplay.Difficulty.NORMAL: idleLimit = 0.5f; break;
                case Gameplay.Difficulty.HARD: idleLimit = 1.0f; break;
            }
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
            WaitForSeconds wait;

            while (true)
            {
                wait = new WaitForSeconds(Random.Range(idleLimit, idleLimit + 0.2f));
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