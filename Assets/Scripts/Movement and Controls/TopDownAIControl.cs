using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UG.FE.Translation;

namespace UG.FE.Movement
{
    [RequireComponent(typeof(InputProvider))]
    public class TopDownAIControl : MonoBehaviour
    {

        private Rigidbody2D m_body2d;
        private SpriteRenderer m_SR;

        private InputProvider myInputProvider;
        private InputState lastInputState;

        // Use this for initialization
        void Start()
        {
            // m_body2d = GetComponent<Rigidbody2D>();
            // m_SR = GetComponentInChildren<SpriteRenderer>();
            myInputProvider = GetComponentInChildren<InputProvider>();

            
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log("Testing Translation [" + Translation.Translation.GetText("TestingInCode") + "]");
            InputState iState = myInputProvider.GetState();
            lastInputState = iState;

            if (iState.isDead)
                return;

        }
    }
}