using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
public class SetSortingAlphaYValue : MonoBehaviour
{
    public enum CurrentState {Lower, Upper, Undefined};

    float lastYPosition = Mathf.Infinity;
    SortingGroup myRender = null;
    SpriteRenderer mySpriteRender = null;
    [SerializeField] bool decreasePositive = true;

    public static float playerYValue = 0;
    CurrentState currentState = CurrentState.Undefined;
    [SerializeField] public float alphaValue = 0.2f;
    [SerializeField] bool isPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        myRender = this.GetComponent<SortingGroup>();
        mySpriteRender = this.GetComponent<SpriteRenderer>();
        UpdateSorting();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSorting();
    }

    private void UpdateSorting()
    {
        float yPos = this.transform.position.y;
        if (yPos != lastYPosition && myRender != null)
        {
            int multiplier = decreasePositive ? -1 : 1;
            lastYPosition = yPos;
            float spriteYSize = (mySpriteRender.sprite.rect.size.y / mySpriteRender.sprite.pixelsPerUnit) * transform.lossyScale.y;
            myRender.sortingOrder = multiplier * Mathf.RoundToInt((yPos - spriteYSize / 2) * 100);
        }

        if (isPlayer)
        {
            playerYValue = yPos;
            ChangeColorTo(1);
        }
        else
        {
            Color lc = mySpriteRender.color;
            if (yPos <= playerYValue)
            {
                if (currentState != CurrentState.Lower)
                {
                    currentState = CurrentState.Lower;
                    ChangeColorTo(alphaValue);
                }
            }
            else if(currentState != CurrentState.Upper)
            {
                currentState = CurrentState.Upper;
                ChangeColorTo(1);
            }
        }
    }

    private void ChangeColorTo(float alpha)
    {
        
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        // Debug.Log("change to colo " + alpha + " : " + spr.Length, this);
        for (int i = 0, length = spr.Length; i < length; i++)
        {
            Color lc = spr[i].color;
            if (lc.a != alpha) spr[i].color = new Color(lc.r, lc.g, lc.b, alpha);
        }
    }
}
