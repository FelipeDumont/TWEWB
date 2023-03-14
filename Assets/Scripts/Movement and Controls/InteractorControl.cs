using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UG.FE.Movement
{
    [RequireComponent(typeof(InputProvider))]
    public class InteractorControl : MonoBehaviour
    {
        [SerializeField] Interactor myInteractor;
        private InputProvider myInputProvider;
        private InputState lastInputState;

        // Use this for initialization
        void Start()
        {
            myInputProvider = GetComponentInChildren<InputProvider>();
            if(myInteractor == null) myInteractor = GetComponentInChildren<Interactor>();


        }


        // Update is called once per frame
        void Update()
        {
            InputState iState = myInputProvider.GetState();
            lastInputState = iState;

            if (iState.isDead)
                return;

            if (iState.actionA)
            {
                myInteractor?.Interact();
            }
        }

        public void Interact()
        {
            if (myInteractor == null) myInteractor = GetComponentInChildren<Interactor>();
            myInteractor?.Interact();
        }
    }
}
