using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScheduleChanger : MonoBehaviour
{

    public string ScheduleChangerID = "";

    [MinMaxSlider("GetCurrentValue"), ReadOnly]
    public Vector2 totalUsedTime = new Vector2(MainGameData.startDayTimeAt, MainGameData.startDayTimeAt);

    // All action will partition teir time to fit the schedule !
    [SerializeField] List<ScheduleAction> dailyActions = new List<ScheduleAction>();

    private Vector2 GetCurrentValue()
    {

        float counter = MainGameData.startDayTimeAt;
        for (int i = 0, length = dailyActions.Count; i < length; i++)
        {
            counter = dailyActions[i].sTime.y;
            if (i + 1 < length)
            {
                dailyActions[i + 1].SetInitialTime(counter);
            }
        }
        totalUsedTime.y = counter;
        return new Vector2(MainGameData.startDayTimeAt, MainGameData.endOfDayTimeAt);
    }

    public List<ScheduleAction> GetDailyActions()
    {
        return dailyActions;
    }
}
