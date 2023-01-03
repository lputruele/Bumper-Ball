using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BumperBallGame
{
    public class BallController : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float minBounceSpeed = 5f;
        public float maxBounceSpeed = 10f;
        public Vector3 initialPos;

        public Vector3 CurrentMove { get; set; }

        private Rigidbody body;
        private Collider m_Collider;
        private Renderer m_Renderer;
        private Collision collision;
        private GameObject lastPlayerTouched;
        private int stunTimer;
        private int destroyTimer;
        private int ghostTimer;
        private bool bounceOff;
        private bool canMove;
        private bool isPlayer;
        private bool gameOver;
        public bool isGhost;

        public AudioClip BounceSound;



        private void Awake()
        {
            CurrentMove = Vector3.zero;
            body = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
            m_Renderer = GetComponent<Renderer>();
            canMove = true;
            destroyTimer = -1;
            isPlayer = GetComponent<PlayerController>() != null;
            initialPos = transform.position;
            EventManager.AddListener<GameOverEvent>(OnGameOver);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Target"), LayerMask.NameToLayer("Ghost"));
        }

        private void OnEnable()
        {
            transform.position = initialPos;
            TurnInvulnerable();
        }

        private void Update()
        {
            if (stunTimer > 0)
            {
                stunTimer--;
            }
            if (destroyTimer > 0)
            {
                destroyTimer--;
            }
            if (destroyTimer == 0)
            {
                Die();
            }
            if (ghostTimer > 0)
            {
                ghostTimer--;
            }
            if (ghostTimer == 0)
            {
                isGhost = false;
                gameObject.layer = LayerMask.NameToLayer("Target");
                Color oldColor = m_Renderer.material.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1.0f);
                m_Renderer.material.SetColor("_Color", newColor);
            }
            

        }
        private void FixedUpdate()
        { 
            if (bounceOff)
            {
                if (collision != null && collision.contacts.Length > 0)
                {
                    Vector3 force = -(collision.GetContact(0).point - transform.position).normalized;
                    force.y = 0.0f;
                    //float bounceSpeed = minBounceSpeed + body.velocity.magnitude;
                    //float bounceSpeed = minBounceSpeed + collision.rigidbody.velocity.magnitude;
                    float bounceSpeed = minBounceSpeed;
                    if (bounceSpeed > maxBounceSpeed)
                    {
                        bounceSpeed = maxBounceSpeed;
                    }
                    body.AddForce(force * moveSpeed * bounceSpeed);
                }
                bounceOff = false;
                stunTimer = isPlayer?10:13;
            }
            else
            {
                if (stunTimer == 0 && canMove && !gameOver)
                {
                    body.AddForce(CurrentMove * moveSpeed);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<BallController>())
            {
                collision = other;
                lastPlayerTouched = collision.gameObject;
                bounceOff = true;
                if (BounceSound)
                    AudioUtility.CreateSFX(BounceSound, transform.position, AudioUtility.AudioGroups.Collision, 0f);          
            }
            else
            {
                destroyTimer = -1;
                canMove = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Floor")
            {
                canMove = false;
                destroyTimer = 100;
            }
            collision = null;
        }

        private void Die()
        {
            gameObject.SetActive(false);
            destroyTimer = -1;
            PlayerDeathEvent evt = Events.PlayerDeathEvent;
            evt.Killed = gameObject;
            evt.Killer = lastPlayerTouched;
            EventManager.Broadcast(evt);
        }

        private void TurnInvulnerable ()
        {
            isGhost = true;
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            Color oldColor = m_Renderer.material.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.35f);
            m_Renderer.material.SetColor("_Color", newColor);
            ghostTimer = 100;
        }

        void OnGameOver(GameOverEvent evt)
        {
            gameOver = true;
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        }
    }
}