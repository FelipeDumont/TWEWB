using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressOnMiddleMinigameCircle : MonoBehaviour
{
    Action<bool> onFinishGame;
    [SerializeField] bool miniGameActive = false;
    [SerializeField] Image imageBackground;
    [SerializeField] Image mid, center;
    [SerializeField] Image pointer;

    float moveSize;
    Vector3 initialPosition;
    Vector3 initialCenterPosition;

    [SerializeField] float difficulty = 1;
    float currentDifficulty = 1;
    [SerializeField] float maxDifficulty = 20;
    float dificultyIncrease = 0;
    float initialHitsToWin = 0;
    [SerializeField] float hitsToWin;

    float centerSize, middleSize, midCenterXPos;

    float currentSpeed = 2f;
    float currentDirection = 1f;
    const float DifficultyLimit = 100;
    const float maxSpeedMultiplier = 8f;
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
        mid.transform.localScale = new Vector3(1, 1, 1);
        center.transform.localScale = new Vector3(1, 1, 1);
        // Debug.Log("Size delta ? :" + imageBackground.rectTransform.rect.width);
        moveSize = imageBackground.rectTransform.rect.width / 2;
        initialCenterPosition = mid.transform.localPosition;

        // positioning
        initialPosition = pointer.transform.localPosition;
        pointer.transform.localPosition = new Vector3(moveSize * -1f, initialPosition.y, initialPosition.z);

        SetCurrentDificulty();
    }

    public void SetCurrentDificulty()
    {
        float maxCenterSize = 0.40f;

        // Assign Sizes depending on the difficulty
        // 1 = 50%|25% 
        if (difficulty < 1) return;
        // .Log(difficulty) goes from 0 to 2; diff [1,100] it will start from 50% to 10% (since we will from difficulty 0 to 100)
        Debug.Log("Difficulty [" + difficulty + " | " + ((Mathf.Log(difficulty, 10)) * maxCenterSize / 2) + "]");
        // mathf log -> (difficulty foes from 1 to 100

        float midSizePerc = 0.5f - ((Mathf.Log(difficulty, 10)) * maxCenterSize / 2); // log 100 = 2, log(1) = 0

        float centerSizePerc = midSizePerc / 2.5f; // Little !
        Debug.Log("size perc [" + midSizePerc + "]");
        float totalSize = moveSize * 2;
        middleSize = midSizePerc * (totalSize);
        centerSize = centerSizePerc * totalSize;

        // Set Them
        mid.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, middleSize);
        center.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, centerSize);

        // Select position between my size and the max size
        float selection = totalSize - middleSize;
        float newPosition = UnityEngine.Random.Range(-selection / 2, selection / 2);
        Debug.Log("Size: " + middleSize + " [" + (-1 * selection / 2) + ", " + (selection / 2) + "] = " + newPosition);
        Vector3 pos = new Vector3(newPosition, initialCenterPosition.y, initialCenterPosition.z);
        midCenterXPos = pos.x;
        mid.transform.localPosition = pos;
        center.transform.localPosition = pos;

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
        float totalSize = moveSize * 2;
        // move the pointer at a certain speed| (difficulty/maxDifficulty) == 0 to 1
        pointer.transform.localPosition += new Vector3(currentSpeed + maxSpeedMultiplier * (difficulty / DifficultyLimit), 0, 0) * currentDirection;
        Debug.Log("Speed " + (currentSpeed + maxSpeedMultiplier * (difficulty / DifficultyLimit)));
        if (pointer.transform.localPosition.x > (totalSize / 2))
        {
            float excess = (totalSize / 2);
            pointer.transform.localPosition = new Vector3(excess, pointer.transform.localPosition.y, pointer.transform.localPosition.z);
            currentDirection *= -1;

        }
        if (pointer.transform.localPosition.x < -(totalSize / 2))
        {
            float excess = -(totalSize / 2);
            pointer.transform.localPosition = new Vector3(excess, pointer.transform.localPosition.y, pointer.transform.localPosition.z);
            currentDirection *= -1;
        }
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
            // use actual position
            float xPosition = pointer.transform.localPosition.x;
            // use the range of the zones !
            if (midCenterXPos + (centerSize / 2) > xPosition &&
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
