using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public bool canRotate = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
    }

    void Update()
    {
        if (target == null) //NullReference오류 발생 방지
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                target = player.transform;
        }

        if (target != null && canRotate)
        {
            agent.SetDestination(target.position);

            Vector3 lookDirection = target.position - transform.position;
            lookDirection.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
        }
    }

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
}
