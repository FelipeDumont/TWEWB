using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UG.FE.Translation;

namespace UG.FE.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InputProvider))]
    public class TopDownMovementControl : MonoBehaviour
    {
        public const float deltaTimeMultiplier = 60; // asumming delta time = 0.02 (normal), we add 60 to get to "1"
        public bool initialPositionRight = true;
        public float m_runSpeed = 4.5f;
        public float m_walkSpeed = 2.0f;
        //public float m_jumpForce = 8f;
        float m_maxSpeed = 4.5f;

        private Rigidbody2D m_body2d;
        private SpriteRenderer m_SR;

        private InputProvider myInputProvider;
        private InputState lastInputState;

        // Use this for initialization
        void Start()
        {
            m_body2d = GetComponent<Rigidbody2D>();
            m_SR = GetComponentInChildren<SpriteRenderer>();
            myInputProvider = GetComponentInChildren<InputProvider>();

            
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = MainGameData.DeltaTime * deltaTimeMultiplier;
            // Debug.Log("Testing Translation [" + Translation.Translation.GetText("TestingInCode") + "]");
            InputState iState = myInputProvider.GetState();
            lastInputState = iState;

            if (iState.isDead || iState.isEnding)
                return;

            // Swap direction of sprite depending on move direction
            if (iState.moveDirectionX != 0)
            {
                m_SR.flipX = iState.moveDirectionX > 0 ? !initialPositionRight : initialPositionRight;
            }

            // SlowDownSpeed helps decelerate the characters when stopping
            float SlowDownSpeed = iState.isMoving ? 1.0f : 0.5f;

            // Set movement
            if (!iState.isCrouching) // not actually crouching, more like slow walking
                m_body2d.velocity = new Vector2(iState.moveDirectionX * m_maxSpeed * SlowDownSpeed, iState.moveDirectionY * m_maxSpeed * SlowDownSpeed) * deltaTime;

            //Crouch / Stand up
            if (iState.isLookingDown && iState.isCrouching)
            {
                m_body2d.velocity = new Vector2(m_body2d.velocity.x * SlowDownSpeed, m_body2d.velocity.y * SlowDownSpeed) * deltaTime;
            }
            //Walk
            else if (iState.isMoving && iState.isSlowWalk)
            {
                m_maxSpeed = m_walkSpeed;
            }

            //Run
            else if (iState.isMoving)
            {
                m_maxSpeed = m_runSpeed;
            }
        }

        public bool IsGrounded()
        {
            return lastInputState.isGrounded;
        }

        public bool IsIddle()
        {
            return !lastInputState.isLookingDown &&
                   !lastInputState.isLookingUp &&
                    lastInputState.isMoving == false; // Not falling and not jumping
        }
    }
}