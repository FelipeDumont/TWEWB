using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UG.FE;

namespace UG.FE.Movement
{

    public struct InputState
    {
        public bool isLookingUp;
        public bool isLookingDown;
        public bool isCrouching;
        public bool isGrounded;
        public bool isWallSlide;
        public bool isMoving;
        public bool isDead;
        public bool isEnding;
        public bool isSlowWalk;

        // jumping
        public int moveDirectionX;
        public int moveDirectionY;
        public int facingDirection;
        public Vector2 rigidBodyVelocity;

        // Other Options
        public bool actionA;
        public bool actionB;
        public bool actionC;

        // AI Settings
        public Vector2 finalPosition;
    }

    public class InputMiddleware : MonoBehaviour
    {
        public virtual InputState Process(InputState lastState, InputProvider provider) { return lastState; }
    }

    [System.Serializable]
    public class InputProvider : MonoBehaviour
    {
        public bool isActive = true;
        public event Action OnJump;
        public List<InputMiddleware> midlewares = new List<InputMiddleware>();

        // Do not process more than once each Frame 
        bool processedOnce;
        InputState processResult;

        // Chain Of responsability
        public InputState GetState()
        {
            if (isActive == false)
            {
                processResult.isEnding = true;
                return processResult;
            }
            if (processedOnce) return processResult;

            InputState inputState = new InputState();

            // Pass the time later ...
            for (int i = 0, length = midlewares.Count; i < length; i++)
            {
                inputState = midlewares[i].Process(inputState, this);
            }
            processedOnce = true;
            processResult = inputState;
            return inputState;
        }

        public void TakePlayerControlEnding()
        {
            isActive = false;
        }

        public void Update()
        {
            processedOnce = false;
        }

        public void Jump()
        {
            OnJump?.Invoke();
        }

        public void DoAction(int actionID)
        {
            if(actionID == 0)
            {
                processResult.actionA = true;
            }
            if(actionID == 1)
            {
                processResult.actionB = true;
            }
            if(actionID == 2)
            {
                processResult.actionC = true;
            }
        }
    }
}