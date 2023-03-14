using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UG.FE;

public class EndingsMenu : GroupPanel
{
   

    // Start is called before the first frame update
    void Start()
    {
        List<Endings> endings = EndingsContainer.singleton.GetAllEndings();
        CreateAll(OnCreateElement, endings);
    }

    void OnCreateElement(Transform t, Endings ending)
    {
        bool unlockedEnding = MainData.data.gameData.EndingIsUnlocked(ending.endingID);
        string mode = unlockedEnding ? "Unlocked" : "...LOCKED !!!";
        t.gameObject.SetActive(true);
        t.name = ending.endingID + "[" + mode + "]";
        
        EndingUI eui = t.GetComponent<EndingUI>();
        eui.endingID = ending.endingID;
        eui.gameScene = ending.gameScene;
    }
}
