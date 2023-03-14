using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateShaderWithYValue : MonoBehaviour
{
    [SerializeField] Material wallsMaterial;

    // Update is called once per frame
    void Update()
    {
        wallsMaterial.SetFloat("_CurrentPlayerHeight", this.transform.position.y);
        
    }

}
