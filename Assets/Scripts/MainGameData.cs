using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MainGameData
{
    public int day;
    public float timePassedInCurrentDay = startDayTimeAt;
    [SerializeField] static float lastDeltaTime;

    // This gives me a 24 minutes per game Day, use GameTimeToMinute, to change the total time per Game Day
    // Total Game Time Per day -> 8h to 18h [10 hours game] -> [10 minutes Game]
    public const float GameTimeToMinute = 1f;       // 1 second is 1 minute in the simulation (x60 times)
    public const float minutesPerHour = 60;         // 60 minutes is 1 hour
    public const float hoursPerDay = 24;            // 24 hours a day
    // Start Each day at TimeNumber * minutesPerHour * GameTimeToMinute
    public const float startDayTimeAt = 8 * minutesPerHour * GameTimeToMinute;
    public const float endOfDayTimeAt = 18 * minutesPerHour * GameTimeToMinute;

    // A Day has Passed
    bool dayHasPassed;

    // keep knowdelage if the player started and it is in the simulation
    public bool isInSimulation;

    // Endings Unlocked
    public List<string> unlockedEndings = new List<string>();
    public bool DayHasPassed() { return dayHasPassed; }

    public void UpdateTime(float addTime)
    {
        lastDeltaTime = addTime;
        timePassedInCurrentDay += addTime * GameTimeToMinute;

        if (timePassedInCurrentDay > endOfDayTimeAt)
        {
            PassDay();
        }
    }

    // Use this for simulated actions !
    public static float DeltaTime { get { return lastDeltaTime == 0 ? Time.deltaTime : lastDeltaTime; } }

    public void AdvanceTimeTo(float newTime)
    {
        timePassedInCurrentDay = newTime;
    }

    public void AdvanceDateTo(float newTime, int newDay)
    {
        day = newDay;
        AdvanceTimeTo(newTime);
    }

    public void PassDay()
    {
        dayHasPassed = true;
        day++;
        AdvanceTimeTo(startDayTimeAt);
    }

    public void StartNextDay()
    {
        dayHasPassed = false;
    }

    public void GetClockTime(out string hours, out string minutes)
    {
        hours = "00";
        minutes = "00";

        int allTimePassed = UnityEngine.Mathf.FloorToInt(timePassedInCurrentDay);
        int hoursN      = UnityEngine.Mathf.FloorToInt(allTimePassed / minutesPerHour);
        int minutesN    = UnityEngine.Mathf.FloorToInt(allTimePassed - (hoursN * minutesPerHour));

        hours   = hoursN < 10 ? "0" + hoursN : "" + hoursN;
        minutes = minutesN < 10 ? "0" + minutesN : "" + minutesN;
    }

    public void GetClockTimePerc(out float result)
    {
        result = 0;
        int allTimePassed = UnityEngine.Mathf.FloorToInt(timePassedInCurrentDay);
        result = allTimePassed / (GameTimeToMinute* minutesPerHour* hoursPerDay);
    }

    public bool EndingIsUnlocked(string endingID)
    {
        return unlockedEndings.Contains(endingID);
    }

    public bool GetEnding(string endingID)
    {
        if (!unlockedEndings.Contains(endingID))
        {
            unlockedEndings.Add(endingID);
            return true;
        }
        return false;
    }

}
