using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using System;

namespace UG.FE.Movement
{

    public class GoToMiddleware : InputMiddleware
    {
        [SerializeField] NavMeshAgent myAgent;
        [SerializeField] List<Vector2> pathPositions = new List<Vector2>();
        [SerializeField] int currentNode;

        [SerializeField] int loadNode;

        // Called internally from other ending related issues
        Action onFinishedMoving;

        public void Start()
        {
            myAgent.updateRotation = false;
            myAgent.updateUpAxis = false;
        }

        public override InputState Process(InputState lastState, InputProvider provider)
        {
            // Debug.Log("Check Go to " + this.name + " dead ?  "+ lastState.isDead, this);
            if (lastState.isDead) return lastState;
            // Debug.Log("Go to process middleware " + currentNode + " [" + myAgent.hasPath + ", " + (myAgent.remainingDistance <= myAgent.stoppingDistance) + "]");
            if (currentNode == 0)
            {
                if (pathPositions.Count >= 1)
                {
                    // Debug.Log("Set Destination ? " + pathPositions[0]);
                    
                    myAgent.SetDestination(new Vector3(pathPositions[0].x, pathPositions[0].y, 0));
                    currentNode++;
                }
            }
            else if (myAgent != null && myAgent.remainingDistance <= myAgent.stoppingDistance)
            {
                
                int lastNode = currentNode;
                currentNode++;
                if (currentNode >= pathPositions.Count)
                {
                    currentNode = pathPositions.Count - 1;
                }

                // Debug.Log(myAgent.pathStatus + " ~~~ " + currentNode + " lastNode " + lastNode);

                if (lastNode < pathPositions.Count)
                {
                    myAgent.SetDestination(pathPositions[currentNode]);
                }
                else
                {
                    // Finished moving
                    onFinishedMoving?.Invoke();
                    onFinishedMoving = null;
                }
            }

            return lastState;
        }


        [Button("Add Current Position")]
        public void AssignCurrentPosition()
        {
            pathPositions.Add(myAgent.transform.position);
        }

        [Button("Return to position")]
        public void ReturnToPosition()
        {
            myAgent.transform.position = pathPositions[loadNode];
        }

        public void AssignNavMeshAgent(NavMeshAgent nma)
        {
            myAgent = nma;
        }

        public void AssignPath(Vector2 newPos)
        {
            pathPositions = new List<Vector2>();
            pathPositions.Add(newPos);
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

        public void OnFinishedMoving(Action onFinishedMovingNew)
        {
            onFinishedMoving += onFinishedMovingNew;
        }
    }
}
