using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PressOnMiddleMinigame : MonoBehaviour
{
    
    Action<bool> onFinishGame;
    [SerializeField] bool miniGameActive = false;
    [SerializeField] Image mid, center;
    [SerializeField] RectTransform pointerRelative;

    float moveSize;

    [SerializeField] float difficulty = 1;
    float currentDifficulty = 1;
    [SerializeField] float maxDifficulty = 20;
    float dificultyIncrease = 0;
    float initialHitsToWin = 0;
    [SerializeField] float hitsToWin;

    float centerAngleSize, middleAngleSize, midCenterAngle, centerAngle;

    float currentSpeed = 2f;
    float currentDirection = 1f;
    const float DifficultyLimit = 100;
    const float maxSpeedMultiplier = 8f;
    float currentAngle = 0;
    List<int> ranges;

    public void StartMiniGame(Action<bool> onFinishGame, float diff, float maxDiff, float hitsToWin)
    {
        miniGameActive = true;
        this.onFinishGame = onFinishGame;
        this.difficulty = diff;
        this.maxDifficulty = maxDiff;
        this.hitsToWin = hitsToWin;
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (miniGameActive == false) ExitMiniGame();
        if (initialHitsToWin <= 0) initialHitsToWin = 1;
        if (maxDifficulty > DifficultyLimit) maxDifficulty = DifficultyLimit;
        if (difficulty > maxDifficulty) difficulty = maxDifficulty;


        currentDifficulty = difficulty;
        dificultyIncrease = (maxDifficulty - difficulty) / hitsToWin;
        initialHitsToWin = hitsToWin;
        // mid.transform.localScale = new Vector3(1, 1, 1);
        // center.transform.localScale = new Vector3(1, 1, 1);
        // Debug.Log("Size delta ? :" + imageBackground.rectTransform.rect.width);
        // moveSize = imageBackground.rectTransform.rect.width / 2;
        // initialCenterPosition = mid.transform.localPosition;

        // positioning
        // initialPosition = pointer.transform.localPosition;
        // pointer.transform.localPosition = new Vector3(moveSize * -1f, initialPosition.y, initialPosition.z);

        SetCurrentDificulty();
    }

    [Button("Test Position")]
    public void SetCurrentDificulty()
    {
        float maxCenterSize = 0.40f;

        if (difficulty < 1) return;
        Debug.Log("Difficulty [" + difficulty + " | " + ((Mathf.Log(difficulty, 10)) * maxCenterSize / 2) + "]");

        float midSizePerc = 0.5f - ((Mathf.Log(difficulty, 10)) * maxCenterSize / 2); // Fill total from 50% to 10%

        float centerSizePerc = midSizePerc / 2.5f; // Little !
        Debug.Log("size perc [" + midSizePerc + "]");

        middleAngleSize = midSizePerc;
        centerAngleSize = centerSizePerc;

        mid.fillAmount = middleAngleSize;
        center.fillAmount = centerAngleSize;

        float newAnglePosition = UnityEngine.Random.Range(0, 360);
        float offsettedAnglePosition = newAnglePosition - (((middleAngleSize - centerAngleSize) * 360) / 2);//size are in 0 to 1 so * 360

        mid.transform.rotation = Quaternion.Euler(0, 0, newAnglePosition);
        center.transform.rotation = Quaternion.Euler(0, 0, offsettedAnglePosition);

        midCenterAngle = Get360(mid.transform.localEulerAngles.z); // put the current Angle !
        centerAngle =  Get360(center.transform.localEulerAngles.z);
        // mid.transform.localPosition = pos;
        // center.transform.localPosition = pos;

    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();
        MovePointer();
        CheckExitMiniGame();
    }

    private void MovePointer()
    {
         // angle
        // move the pointer at a certain speed| (difficulty/maxDifficulty) == 0 to 1
        pointerRelative.transform.Rotate(new Vector3(0, 0, (-1)*(currentSpeed + maxSpeedMultiplier * (difficulty / DifficultyLimit))*0.5f));
        currentAngle = pointerRelative.transform.localEulerAngles.z;
        Color colorCheck = Color.blue;

        float rCenterSide = Get360(centerAngle);
        float lCenterSide = Get360(centerAngle -(centerAngleSize * 360));

        float rMiddleSide = Get360(midCenterAngle);
        float lMiddleSide = Get360(midCenterAngle - (middleAngleSize * 360));
        currentAngle = Get360(currentAngle);

        Debug.Log(centerAngle + ",  [" + lCenterSide + ", " + rCenterSide + "] = "  + currentAngle);
        if (IsInMiddle(lCenterSide, centerAngle, rCenterSide, currentAngle))
        {
            print("<color=green>currentAngle " + currentAngle + " [ " + (lMiddleSide) + ", " + (rCenterSide) + "]</color>");
        }
        else 
        {
            
            if (IsInMiddle(lMiddleSide, midCenterAngle, rMiddleSide, currentAngle))
            {
                print("<color=yellow>currentAngle " + currentAngle + " [ " + (midCenterAngle + (centerAngleSize * 180)) + ", " + (midCenterAngle - (centerAngleSize * 180)) + "</color>");
            }
            else
            {

                print("<color=red>currentAngle " + currentAngle + " [ " + (midCenterAngle + (centerAngleSize * 180)) + ", " + (midCenterAngle - (centerAngleSize * 180)) + "</color>");
            }
            
            
        }
       

        Debug.DrawLine(this.transform.position, pointerRelative.GetChild(0).transform.position, colorCheck);
    }

    private float Get360(float value)
    {
        value = value > 360 ? value - 360 : value;
        value = value < 0 ? value + 360 : value;
        return value;
    }

    // 270 , 9 , 98
    // 0 , 50 , 100
    // 310 , 360 , 40
    private bool IsInMiddle(float left, float center, float right, float testValue)
    {
        if(left  <= 360 && (center < left || right < left))
        {
            return (left <= testValue && left <= 360) || (0 <= testValue && testValue <= right);
        }

        return left <= testValue && testValue <= right;
    }

    private void CheckExitMiniGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMiniGame();
        }
    }
    /// <summary>
    /// Check on the click :3, later it will move
    /// </summary>
    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if(midCenterAngle + (centerAngleSize * 180) < currentAngle && 
                midCenterAngle - (centerAngleSize * 180) > currentAngle)
            {

            }
            
            /*
            // use actual position
            float xPosition = pointer.transform.localPosition.x;
            // use the range of the zones !
            if( midCenterXPos + (centerSize/2) > xPosition &&
                midCenterXPos - (centerSize / 2) < xPosition)
            {
                // In the Center
                AcquirePoints(+2);
            }
            else if (midCenterXPos + (middleSize / 2) > xPosition &&
                    midCenterXPos - (middleSize / 2) < xPosition)
            {
                // Middle

                AcquirePoints(+1);
            }
            else
            {
                AcquirePoints(-1);
            }
            */

            if (hitsToWin == 0)
            {
                //WIN
                this.gameObject.SetActive(false);
                if (onFinishGame != null)
                {
                    onFinishGame(true);
                }
            }
        }
    }

    private void ExitMiniGame()
    {
        Debug.Log("Exit game ?");
        this.gameObject.SetActive(false);
        if (onFinishGame != null)
        {
            onFinishGame(false);
        }
    }

    private void AcquirePoints(float pointsAcquired)
    {
        // Debug.Log("Acquire Points ? " + pointsAcquired);
        if (initialHitsToWin >= hitsToWin - pointsAcquired)
        {
            difficulty += dificultyIncrease * pointsAcquired;
            hitsToWin -= pointsAcquired;
            SetCurrentDificulty();
        }
        // Else you do nothing, just reset the thing
        else
        {
            SetCurrentDificulty();
        }
    }
}
