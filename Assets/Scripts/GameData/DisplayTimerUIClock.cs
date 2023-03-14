using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTimerUIClock : MonoBehaviour
{
    [SerializeField] Image hoursUI;
    [SerializeField] float initialOffset;

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
            MainData.data.gameData.GetClockTimePerc(out float perc);
            // Debug.Log("Display Timer ??? {" + perc + "}");
            if (hoursUI != null)
            {
                hoursUI.transform.rotation = Quaternion.Euler(0, 0, 360 - (perc * 360 + initialOffset));
            }
        }
    }
}
