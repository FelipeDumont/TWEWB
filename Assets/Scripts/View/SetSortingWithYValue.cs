using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
public class SetSortingWithYValue : MonoBehaviour
{
    float lastYPosition = Mathf.Infinity;
    SortingGroup myRender = null;
    SpriteRenderer mySpriteRender = null;
    [SerializeField] bool decreasePositive = true;

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
            int multiplier = decreasePositive ? -1 : 1 ;
            lastYPosition = yPos;
            float spriteYSize = (mySpriteRender.sprite.rect.size.y / mySpriteRender.sprite.pixelsPerUnit) * transform.lossyScale.y;
            myRender.sortingOrder = multiplier * Mathf.RoundToInt((yPos - spriteYSize/2) *100);

            // Debug.Log("Y Size "+ spriteYSize);
        }
    }
}
