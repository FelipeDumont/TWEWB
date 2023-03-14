using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UG.FE;

namespace UG.FE
{
    /// <summary>
    /// Pass Time if we want it !
    /// </summary>
    public class PassGameTime : MonoBehaviour
    {
        public const float defaultTime = 1;
        public const float defaultPauseTime = 0;

        [SerializeField] float timeMultiplier = defaultTime;

        public void ChangeTimeMultiplier(float newMultiplier)
        {
            timeMultiplier = newMultiplier;
        }

        void Update()
        {
            if (MainData.IsReady() && this.isActiveAndEnabled)
            {
                MainData.data.gameData.UpdateTime(timeMultiplier * Time.deltaTime);
            }
        }
    }
}