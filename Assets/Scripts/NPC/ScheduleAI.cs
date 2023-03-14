using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


// each day
[System.Serializable]
public class ScheduleAction
{
    [MinMaxSlider(MainGameData.startDayTimeAt, MainGameData.endOfDayTimeAt, true), HideLabel]
    public Vector2 sTime = new Vector2(MainGameData.startDayTimeAt, MainGameData.startDayTimeAt);
    [FoldoutGroup("Action")]
    public string testingName = "";


    public void SetInitialTime(float initialTime)
    {
        sTime = new Vector2(initialTime,
                                        sTime.y < initialTime ? initialTime : sTime.y);
    }
}

public class ScheduleAI : MonoBehaviour
{

    [MinMaxSlider("GetCurrentValue"), ReadOnly]
    public Vector2 totalUsedTime = new Vector2(MainGameData.startDayTimeAt, MainGameData.startDayTimeAt);

    // All action will partition teir time to fit the schedule !
    [SerializeField] List<ScheduleAction> dailyActions = new List<ScheduleAction>();

    private Vector2 GetCurrentValue()
    {
        
        float counter  = MainGameData.startDayTimeAt;
        for (int i = 0, length = dailyActions.Count; i < length; i++)
        {
            counter = dailyActions[i].sTime.y;
            if(i + 1 < length)
            {
                dailyActions[i + 1].SetInitialTime(counter);
            }
        }
        totalUsedTime.y = counter;
        return new Vector2(MainGameData.startDayTimeAt, MainGameData.endOfDayTimeAt);
    }

    public Dictionary<string, ScheduleChanger> scheduleChanger;

    public void Start()
    {
        ScheduleChanger[] schedules = GetComponentsInChildren<ScheduleChanger>();
        scheduleChanger = new Dictionary<string, ScheduleChanger>();

        for (int i = 0, length = schedules.Length; i < length; i++)
        {
            if (!scheduleChanger.ContainsKey(schedules[i].ScheduleChangerID))
            {
                scheduleChanger.Add(schedules[i].ScheduleChangerID, schedules[i]);
            }
        }
    }

    public void ChangeSchedule(string newSchedule)
    {
        ScheduleChanger sc = null;
        for (int i = 0, length = scheduleChanger.Count; i < length; i++)
        {
            if (scheduleChanger.ContainsKey(newSchedule))
            {
                sc = scheduleChanger[newSchedule];
            }
        }
        if(sc != null)
        {
            // Replace my actual schedule with this new one ?
            dailyActions = sc.GetDailyActions();
        }
    }
}
