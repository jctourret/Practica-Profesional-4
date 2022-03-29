using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static Action onCaughtPlayer;

    NavMeshAgent agent;
    public Transform target;
    public Transform holdingPoint;
    public List<Transform> capturePoints;
    Transform chosenCapturePoint;
    bool capturePointChosen =false;
    int freeCapturepoint;

    public float rotDamping;
    float catchRadius = 3f;
    float depositRadius = 2f;

    public bool playerCaught = false;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //agent.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            if (playerCaught == false)
            {
                agent.destination = target.position;
                if (agent.velocity.magnitude < 0.15f)
                {
                    Vector3 lookDir = target.position - transform.position;
                    lookDir.y = 0;
                    Quaternion rot = Quaternion.LookRotation(lookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotDamping);
                }
                Debug.Log(Vector3.Distance(target.position, transform.position));
                if (playerCaught == false && Vector3.Distance(target.position, transform.position) <= catchRadius)
                {
                    target.transform.position = holdingPoint.position;
                    target.transform.parent = holdingPoint;
                    playerCaught = true;
                    for (int i = 0; i < capturePoints.Count; i++)
                    {
                        if (capturePointChosen == false && capturePoints[i].childCount == 0)
                        {
                            chosenCapturePoint = capturePoints[i];
                            agent.destination = chosenCapturePoint.position;
                            capturePointChosen = true;
                        }
                    }
                    onCaughtPlayer?.Invoke();
                }
            }
            else
            {
                if (Vector3.Distance(agent.destination, transform.position) < depositRadius)
                {
                    target.transform.parent = chosenCapturePoint;
                    target.transform.position = chosenCapturePoint.transform.position;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,catchRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, depositRadius);

    }
}