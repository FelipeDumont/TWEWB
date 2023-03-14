using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshRuntimeUpdate : MonoBehaviour
{
    [SerializeField] NavMeshSurface2d Surface2D;

    void Start()
    {
        Surface2D.BuildNavMesh();
    }

    void Update()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
