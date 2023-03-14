using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;

namespace UG.FE.Movement
{

    public class GoTo : MonoBehaviour
    {
        [SerializeField] NavMeshAgent myAgent;
        [SerializeField] public List<Vector2> pathPositions = new List<Vector2>();
        [SerializeField] int currentNode;

        public void Start()
        {
            
            myAgent.updateRotation = false;
            myAgent.updateUpAxis = false;
            if (pathPositions.Count > 1)
            {
                Debug.Log("Set Destination ? " + pathPositions[1]);
                myAgent.destination = new Vector3(pathPositions[1].x, pathPositions[1].y, this.transform.position.z);
                // myAgent.SetDestination(new Vector3(pathPositions[1].x, pathPositions[1].y, this.transform.position.z));
                currentNode++;
            }
        }

        [Button("Add Current Position")]
        public void AssignCurrentPosition()
        {
            pathPositions.Add(this.transform.position);
        }

        public static void DebugDrawPath(Vector3[] corners)
        {
            if (corners.Length < 2) { return; }
            int i = 0;
            for (; i < corners.Length - 1; i++)
            {
                Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
            }
            Debug.DrawLine(corners[0], corners[1], Color.red);
        }

        private void OnDrawGizmos()
        {
            DrawGizmos(myAgent, true, false);
        }

        public static void DrawGizmos(NavMeshAgent agent, bool showPath, bool showAhead)
        {
            if (Application.isPlaying)
            {
                if (showPath && agent.hasPath)
                {
                    var corners = agent.path.corners;
                    if (corners.Length < 2) { return; }
                    int i = 0;
                    for (; i < corners.Length - 1; i++)
                    {
                        Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(agent.path.corners[i + 1], 0.03f);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
                    }
                    Debug.DrawLine(corners[0], corners[1], Color.blue);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(agent.path.corners[1], 0.03f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(agent.path.corners[0], agent.path.corners[1]);
                }

                if (showAhead)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawRay(agent.transform.position, agent.transform.up * 0.5f);
                }
            }
        }
    }
}
