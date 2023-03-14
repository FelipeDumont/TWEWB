using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UG.FE;

public class EndingUI : MonoBehaviour
{
    [LevelSelector(true)]
    public string gameScene; // This will go to the ending thingy !

    [SerializeField] Button endingPlay;
    public string endingID;

    public void PlayEnding()
    {
        LoadAndPlayEnding(endingID);
    }

    private void LoadAndPlayEnding(string endingID)
    {
        Debug.Log("Loading and GO !");
        MainData.data.playEnding = endingID;
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene); // This will be asigned by the ending settings as well as the "in Game time
    }
}
