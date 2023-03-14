using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UG.FE;
using UG.FE.Translation;
using Sirenix.OdinInspector;

[System.Serializable]
public class Endings
{
    public string endingID;
    public string information;
    public Sprite endingIcon;
    public Animation endingAnimation;

    // First versión of the ending, we need 2 things, first is to determine the end of the ending, could be time or other thing
    // this first test the end will be 0.5f after we have a dialogue been displayed
    public float timeOnceFinished;
    [ValueDropdown("dialogues")]
    public string endingDialogue;

    [LevelSelector(true)]
    public string gameScene; // in witch ocurrs

    public IList<ValueDropdownItem<string>> dialogues {get { return Translation.singleton.DialogueIDs(); } }
}

[CreateAssetMenu(menuName = "TWEWB/Endings")]
public class EndingsContainer : ScriptableObjectSingleton
{
    [SerializeField] List<Endings> allEndings = new List<Endings>();
    public static EndingsContainer singleton;

    public override void SetSingletonValues()
    {
        base.SetSingletonValues();
        if (singleton != null)
        {
            singleton = this;
        }
    }

    public EndingsContainer()
    {
        Debug.Log("<color=red> TWEWB Endings Container</color>");
        singleton = this;
    }

    public List<string> GetEndingsIDs()
    {
        List<string> options = new List<string>() { "NONE" };
        for (int i = 0, length = allEndings.Count; i < length; i++)
        {
            options.Add(allEndings[i].endingID);
        }
        return options;
    }

    public Endings GetEnding(string endingID) 
    {
        for (int i = 0, length = allEndings.Count; i < length; i++)
        {
            if (allEndings[i].endingID == endingID)
            {
                return allEndings[i];
            }
        }
        return null;
    }

    public List<Endings> GetAllEndings()
    {
        return allEndings;
    }
}
