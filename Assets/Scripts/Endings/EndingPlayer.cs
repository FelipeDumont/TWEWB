using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



public class EndingPlayer : MonoBehaviour
{
    [ValueDropdown("endingsIds")]
    public string currentSelectedEnding;

    public List<string> endingsIds { get { return EndingsContainer.singleton.GetEndingsIDs(); } }

    public void GetEnding()
    {
        GetEnding(currentSelectedEnding);
    }

    public void GetEnding(string endingID)
    {
        if (endingsIds.Contains(endingID))
        {
            MainData.data.gameData.GetEnding(endingID);
            PlayEnding();
        }
    }

    private void PlayEnding()
    {
        
        FindObjectOfType<EndingController>().StartPlayingEnding(currentSelectedEnding);
    }

    


}
