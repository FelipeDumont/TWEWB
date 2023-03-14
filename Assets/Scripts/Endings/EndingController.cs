using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UG.FE.Movement;
using UnityEngine.SceneManagement;
using UG.FE;
using UG.FE.Translation;

// This should look up in the current scene the endongs elmentsto trigger the correct ending
public class EndingController : MonoBehaviour
{
    [LevelSelector(false)]
    public string creditsScene;

    Endings currentEnding = null;
    InputProvider player = null;

    public void Start()
    {
        CheckAndPlayEnding();    
    }

    // Play ending from here !
    public void StartPlayingEnding(string endingID)
    {
        if (currentEnding != null) return;
        player = null;
        InputProvider[] ip = FindObjectsOfType<InputProvider>();
        for (int i = 0, length = ip.Length; i < length; i++)
        {
            if (ip[i].tag == "Player")
            {
                player = ip[i];
            }
            else
            {
                ip[i].isActive = false;
            }
        }

        currentEnding = EndingsContainer.singleton.GetEnding(endingID);
        DialogTester[] dt = FindObjectsOfType<DialogTester>();
        Transform parent = player.midlewares[0].transform;
        player.midlewares = new List<InputMiddleware>();

        // Assign player things !!!
        for (int i = 0, length = dt.Length; i < length; i++)
        {
            if(dt[i].dialogueID == currentEnding.endingDialogue)
            {
                // Player to this position, and start dialogue
                DialogueMiddleware dm = parent.gameObject.AddComponent<DialogueMiddleware>();
                GoToMiddleware gm = parent.gameObject.AddComponent<GoToMiddleware>();
                NavMeshAgent nma = player.gameObject.AddComponent<NavMeshAgent>();
                InteractorControl ic = player.gameObject.AddComponent<InteractorControl>();
                nma.baseOffset = 0;

                gm.AssignNavMeshAgent(nma);
                gm.AssignPath(dt[i].transform.position);
                gm.OnFinishedMoving(() => { ic.Interact(); Debug.Log("Try dialogue !"); });
                
                // Add middleware to the player, so it does something !
                player.midlewares.Add(gm);
                player.midlewares.Add(dm);
                // Add to dialog window to call here when it ends !
                PanelDialogueManager.singleton.AddOnDialogueEnded(() => { NextEndingStep(); }) ;
            }
        }
        // e.endingDialogue;
        player.transform.rotation = new Quaternion(0, 0, 0, 0); 
    }


    private void NextEndingStep()
    {
        // Called from everywhere to state that an ending need to pass to the next stage
        // for now the ending will just wait and go
        Debug.Log("Time to go " + currentEnding.timeOnceFinished);
        Invoke("GoToCredits", currentEnding.timeOnceFinished);
    }

    private void GoToCredits()
    {
        SceneManager.LoadScene(creditsScene);
        currentEnding = null; // just in case
    }

    private void CheckAndPlayEnding()
    {
        if (MainData.data == null || string.IsNullOrEmpty(MainData.data.playEnding)) return;
        string acquiredEnding = MainData.data.playEnding;
        MainData.data.playEnding = "";

        if (!string.IsNullOrEmpty(acquiredEnding))
        {
            StartPlayingEnding(acquiredEnding);
        }
    }
}
