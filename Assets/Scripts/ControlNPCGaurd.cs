using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public class ControlNPCGaurd : MonoBehaviour
{
    public List<GameObject> wayPoints = new List<GameObject>();
    int wayPointIndex = 0;
    [HideInInspector]
    public Animator anim;
    private NavMeshAgent agent;
    AnimatorStateInfo info;
    public enum GUARD_TYPE { IDLE = 0, PATROLLER = 1, CHASER = 2 };
    [SerializeField]
    public GUARD_TYPE guardType;
    [SerializeField]
    public GameObject player;


    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (guardType == GUARD_TYPE.IDLE) anim.SetBool("isIdleGuard", true);
        else if (guardType == GUARD_TYPE.PATROLLER) anim.SetBool("isPatrolGuard", true);
        else if (guardType == GUARD_TYPE.CHASER) anim.SetBool("isChaserGuard", true);
    }

    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        
        if (IsNearPlayer()) anim.SetBool("isWithinAttackingRange", true);
        else anim.SetBool("isWithinAttackingRange", false);

        if (info.IsName("Patrol"))
        {
            agent.isStopped = false;

            if (Vector3.Distance(transform.position, wayPoints[wayPointIndex].transform.position) < 1.5f)
            {
                wayPointIndex++;
                if (wayPointIndex >= wayPoints.Count) wayPointIndex = 0;
            }

            agent.SetDestination(wayPoints[wayPointIndex].transform.position);
        }

        if (info.IsName("Chase"))
        {
            agent.speed = 2.5f;
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else if (info.IsName("Attack"))
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            agent.isStopped = true;
            if (info.normalizedTime % 1.0 >= .98 && IsVeryNearPlayer())
            {
                player.GetComponent<ControlPlayer>().DecreaseHealth(10);
            }
        }
    }

    bool IsNearPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2.0f) return true;
        else return false;
    }

    public void Dies()
    {
        anim.SetTrigger("isDying");
    }

    bool IsVeryNearPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f) return true;
        else return false;
    }
}
