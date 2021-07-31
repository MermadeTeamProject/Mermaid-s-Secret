using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Enemy가 Player를 추적하는 스크립트 (NavMesh)
public class MoveAgent : MonoBehaviour
{
    NavMeshAgent nav;
    GameObject target;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
    }

    private void Update()
    {
        if(nav.destination != target.transform.position)
        {
            nav.SetDestination(target.transform.position);
        }
        else
        {
            nav.SetDestination(transform.position);
        }
    }



}


