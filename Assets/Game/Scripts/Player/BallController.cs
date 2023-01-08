using Game.Audio;
using UnityEngine;

namespace Game.Player
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
        public bool CanMove { get; private set; }

        private Rigidbody body;
        private Renderer m_Renderer;
        private Collision collision;
        private GameObject lastPlayerTouched;
        private float stunTimer;
        private float destroyTimer;
        private float ghostTimer;
        private bool bounceOff;
        private bool gameOver;

        public bool IsPlayer { get; private set; }  
        public bool IsGhost { get; private set; }



        private void Awake()
        {
            CurrentMove = Vector3.zero;
            body = GetComponent<Rigidbody>();
            m_Renderer = GetComponent<Renderer>();
            CanMove = true;
            destroyTimer = float.PositiveInfinity;
            stunTimer = float.NegativeInfinity;
            ghostTimer = float.PositiveInfinity;
            IsPlayer = GetComponent<PlayerController>() != null;
            initialPos = transform.position;
            EventManager.AddListener<GameOverEvent>(OnGameOver);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Target"), LayerMask.NameToLayer("Ghost"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ghost"), LayerMask.NameToLayer("Ghost"));
        }

        private void OnEnable()
        {
            if (InGameSounds.Instance.SpawnSound)
                AudioUtility.CreateSFX(InGameSounds.Instance.SpawnSound, transform.position, AudioUtility.AudioGroups.Spawn, 0f);
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
                IsGhost = false;
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
                    float bounceSpeed = BOUNCE_SPEED;
                    body.AddForce(force * MOVE_SPEED * bounceSpeed);
                }
                bounceOff = false;
                stunTimer = IsPlayer? Time.time + STUN_TIME_PLAYER : Time.time + STUN_TIME_BOT;
            }
            else
            {
                if (stunTimer < Time.time && CanMove && !gameOver)
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
                if (InGameSounds.Instance.BumpSound)
                    AudioUtility.CreateSFX(InGameSounds.Instance.BumpSound, transform.position, AudioUtility.AudioGroups.Collision, 0f);
                BumpEvent bumpEvt = Events.BumpEvent;
                bumpEvt.Bumped = gameObject;
                bumpEvt.Bumper = other.gameObject;
                EventManager.Broadcast(bumpEvt);
            }
            else
            {
                destroyTimer = float.PositiveInfinity;
                CanMove = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Floor")
            {
                CanMove = false;
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
                    flagGrabbedEvt.Grabber = gameObject;
                    EventManager.Broadcast(flagGrabbedEvt);
                }
            }
        }

        private void Die()
        {
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
            IsGhost = true;
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