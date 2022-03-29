using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target;
    public float rotDamping;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            agent.destination = target.position;
            if (agent.velocity.magnitude < 0.15f)
            {
                Vector3 lookDir = target.position - transform.position;
                lookDir.y = 0;
                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation,rot,Time.deltaTime * rotDamping);
            }
        }
    }
}
