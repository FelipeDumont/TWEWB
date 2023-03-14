using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UG.FE.Movement
{
    public class DialogueMiddleware : InputMiddleware
    {
        public override InputState Process(InputState lastState, InputProvider provider)
        {
            // If the pause menu is active, stop the player and pass the data to it
            if (PanelDialogueManager.IsActive())
            {
                lastState.isCrouching    = false;
                lastState.isMoving       = false;
                lastState.isSlowWalk     = false;

                lastState.moveDirectionX  = 0;
                lastState.moveDirectionY  = 0;

                // No movement and use the ActionA
                if (lastState.actionA)
                {
                    PanelDialogueManager.singleton.DisplayNextSentence();
                    lastState.actionA = false;
                }
                lastState.actionB = false;
                lastState.actionC = false;
            }

            return lastState;
        }
    }
}

