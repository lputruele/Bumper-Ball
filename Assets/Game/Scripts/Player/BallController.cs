using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BumperBallGame
{
    public class BallController : MonoBehaviour
    {
        public const float STUN_TIME_PLAYER = 0.3f;
        public const float STUN_TIME_BOT = 0.35f;
        public const float DESTROY_TIME = 2.1f;
        public const float GHOST_TIME = 0.9f;

        public const float MOVE_SPEED = 9.5f;
        public const float BOUNCE_SPEED = 7.5f;

        public Vector3 initialPos;

        public Vector3 CurrentMove { get; set; }

        private Rigidbody body;
        private Renderer m_Renderer;
        private Collision collision;
        private GameObject lastPlayerTouched;
        private float stunTimer;
        private float destroyTimer;
        private float ghostTimer;
        private bool bounceOff;
        private bool canMove;
        private bool isPlayer;
        private bool gameOver;
        public bool isGhost;

        public AudioClip BounceSound;
        public AudioClip DeathSound;
        public AudioClip SpawnSound;



        private void Awake()
        {
            CurrentMove = Vector3.zero;
            body = GetComponent<Rigidbody>();
            m_Renderer = GetComponent<Renderer>();
            canMove = true;
            destroyTimer = float.PositiveInfinity;
            stunTimer = float.NegativeInfinity;
            ghostTimer = float.PositiveInfinity;
            isPlayer = GetComponent<PlayerController>() != null;
            initialPos = transform.position;
            EventManager.AddListener<GameOverEvent>(OnGameOver);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Target"), LayerMask.NameToLayer("Ghost"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ghost"), LayerMask.NameToLayer("Ghost"));
        }

        private void OnEnable()
        {
            if (SpawnSound)
                AudioUtility.CreateSFX(SpawnSound, transform.position, AudioUtility.AudioGroups.Spawn, 0f);
            transform.position = initialPos;
        }

        private void Update()
        {
            if (destroyTimer < Time.time)
            {
                destroyTimer = float.PositiveInfinity;
                Die();
            }
            if (ghostTimer < Time.time)
            {
                ghostTimer = float.PositiveInfinity;
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
                    float bounceSpeed = BOUNCE_SPEED;
                    body.AddForce(force * MOVE_SPEED * bounceSpeed);
                }
                bounceOff = false;
                stunTimer = isPlayer? Time.time + STUN_TIME_PLAYER : Time.time + STUN_TIME_BOT;
            }
            else
            {
                if (stunTimer < Time.time && canMove && !gameOver)
                {
                    body.AddForce(CurrentMove * MOVE_SPEED);
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
                BumpEvent bumpEvt = Events.BumpEvent;
                bumpEvt.Bumped = gameObject;
                bumpEvt.Bumper = other.gameObject;
                EventManager.Broadcast(bumpEvt);
            }
            else
            {
                destroyTimer = float.PositiveInfinity;
                canMove = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Floor")
            {
                canMove = false;
                destroyTimer = Time.time + DESTROY_TIME;
            }
            collision = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Flag")
            {
                if (other.transform.position == Vector3.zero)
                {
                    FlagGrabbedEvent flagGrabbedEvt = Events.FlagGrabbedEvent;
                    flagGrabbedEvt.grabber = gameObject;
                    EventManager.Broadcast(flagGrabbedEvt);
                }
            }
        }

        private void Die()
        {
            //if (DeathSound)
            //    AudioUtility.CreateSFX(DeathSound, transform.position, AudioUtility.AudioGroups.Death, 0f);
            gameObject.SetActive(false);
            destroyTimer = float.PositiveInfinity;
            PlayerDeathEvent evt = Events.PlayerDeathEvent;
            evt.Killed = gameObject;
            evt.Killer = lastPlayerTouched;
            EventManager.Broadcast(evt);
            TurnInvulnerable();
        }

        private void TurnInvulnerable ()
        {
            isGhost = true;
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            Color oldColor = m_Renderer.material.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.2f);
            m_Renderer.material.SetColor("_Color", newColor);
            ghostTimer = Time.time + GHOST_TIME;
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