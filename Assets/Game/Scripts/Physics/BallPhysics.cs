using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BumperBallGame
{
    public class BallPhysics : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float minBounceSpeed = 5f;
        public float maxBounceSpeed = 10f;

        public Vector3 CurrentMove { get; set; }

        private Rigidbody body;
        private Collision collision;
        private int stunTimer;
        private int destroyTimer;
        private bool bounceOff;
        private bool canMove;

        public AudioClip BounceSound;



        private void Awake()
        {
            CurrentMove = Vector3.zero;
            body = GetComponent<Rigidbody>();
            canMove = true;
            destroyTimer = -1;
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
                    Debug.Log(force);
                    body.AddForce(force * moveSpeed * bounceSpeed);
                }
                bounceOff = false;
                stunTimer = 15;
            }
            else
            {
                if (stunTimer == 0 && canMove)
                {
                    body.AddForce(CurrentMove * moveSpeed);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<BallPhysics>())
            {
                collision = other;
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
            collision = null;
            if (!other.gameObject.GetComponent<BallPhysics>())
            {
                canMove = false;
                destroyTimer = 300;
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
            PlayerDeathEvent evt = Events.PlayerDeathEvent;
            evt.Player = gameObject;
            evt.RemainingPlayerCount--;
            EventManager.Broadcast(evt);
        }
    }
}