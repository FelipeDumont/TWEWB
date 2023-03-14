using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UG.FE.Movement
{
    [System.Serializable]
    public class InputButtons
    {
        // For now just one alternative
        // [FoldoutGroup("Input")]
        public KeyCode leftKeyCode = KeyCode.A;
        // [FoldoutGroup("Input")]
        public KeyCode rightKeyCode = KeyCode.D;
        // [FoldoutGroup("Input")]
        public KeyCode upKeyCode = KeyCode.W;
        // [FoldoutGroup("Input")]
        public KeyCode downKeyCode = KeyCode.S;

        public KeyCode ActionACode = KeyCode.Space;

        // Slow Walk
        [FoldoutGroup("Other Input")]
        public KeyCode slowWalkKeyCode = KeyCode.LeftControl;

        // Action To JUMP
        [FoldoutGroup("Other Input")]
        public KeyCode jumpKeyCode = KeyCode.Space;
    }

    public class InputSystemMiddleware : InputMiddleware
    {
        public InputButtons mainButtons;
        public InputButtons alternativeButtons;

        public override InputState Process(InputState lastState, InputProvider provider)
        {
            // Normal Moves
            if (Input.GetKey(mainButtons.leftKeyCode) || Input.GetKey(alternativeButtons.leftKeyCode))
            {
                lastState.moveDirectionX += -1;
                lastState.isMoving = true;
            }
            if (Input.GetKey(mainButtons.rightKeyCode) || Input.GetKey(alternativeButtons.rightKeyCode))
            {
                lastState.moveDirectionX += 1;
                lastState.isMoving = true;
            }
            if (Input.GetKey(mainButtons.upKeyCode) || Input.GetKey(alternativeButtons.upKeyCode))
            {
                lastState.moveDirectionY += 1;
                lastState.isMoving = true;
            }
            if (Input.GetKey(mainButtons.downKeyCode) || Input.GetKey(alternativeButtons.downKeyCode))
            {
                lastState.moveDirectionY -= 1;
                lastState.isMoving = true;
            }

            if (Input.GetKey(mainButtons.slowWalkKeyCode) || Input.GetKey(alternativeButtons.slowWalkKeyCode))
            {
                lastState.isSlowWalk = true;
            }

            // Action A
            if ((Input.GetKeyDown(mainButtons.ActionACode) || Input.GetKeyDown(alternativeButtons.ActionACode)))
            {
                lastState.actionA = true;
            }
            return lastState;
        }
    }
}
