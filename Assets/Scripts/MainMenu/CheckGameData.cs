using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assume the main game is just load and the get to this screen !
/// </summary>
public class CheckGameData : MonoBehaviour
{
    public GameObject startButton;
    public GameObject continueButton;
    public GameObject endingsButton;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        while( MainData.IsReady() == false && MainData.data.gameData == null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        // Set active if in simulation or start if needs to start a "new" day
        bool isInSimulation = MainData.data.gameData.isInSimulation;
        startButton.SetActive(!isInSimulation);
        continueButton.SetActive(isInSimulation);

        // if there is data, the chekc if there are unlocked endings
        if (MainData.data.gameData.unlockedEndings != null && MainData.data.gameData.unlockedEndings.Count > 0)
        {
            endingsButton.SetActive(true);
        }
    }

    public void ResetGameData()
    {
        Debug.Log("Reset Game Data !");
        MainData.CleanGameData();
        StartCoroutine(Start());
    }
}
