using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hoursUI;
    [SerializeField] TextMeshProUGUI minutesUI;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentTimer();
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentTimer();
    }

    void SetCurrentTimer()
    {
        if (MainData.IsReady())
        {
            MainData.data.gameData.GetClockTime(out string hours, out string minutes);
            // Debug.Log("Display Timer ??? [" + hours + ":" + minutes + "] ");
            if (hoursUI.gameObject != null) hoursUI.text = hours;
            if (minutesUI.gameObject != null) minutesUI.text = minutes;
        }
    }
}
