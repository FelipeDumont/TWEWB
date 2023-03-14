using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] Material currentMaterial;
    [SerializeField] Image circle;
    // Start is called before the first frame update
    void Start()
    {
        Texture2D texture = new Texture2D(128, 128);
        texture.filterMode = FilterMode.Point;

        if (currentMaterial != null){
            currentMaterial.mainTexture = texture;
        }

        int offset = 64;
        Color color = new Color(0, 0, 0, 0);

        for (int i = 0, length = offset*2; i < length; i++)
        {
            for (int j = 0, length2 = offset*2; j < length2; j++)
            {
                texture.SetPixel(i, j, color);
            }
        }

        
        texture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
