using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyCtrlK : WhenAlive
{
    private Vector3 m_V_posi; //?�동 목표�????�치

    //?�니메이??
    private Animator m_A_anim;
    //경로, 목표
    private NavMeshAgent m_N_nav;
    private Transform m_T_playerTr;

    [Header("Enemy ?�동 경로")]
    [SerializeField]
    private Transform[] T_inPoint; //?��??�찰

    private void Start()
    {
        init();
    }
    private void init()
    {
        m_N_nav = this.GetComponent<NavMeshAgent>();
        m_A_anim = this.GetComponent<Animator>();
        m_T_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        RanPosi();
    }
    private void Update()
    {
        if (!b_isDead)
            Move_AccodingToSituation();
    }    

    //?�레?�어 발견, 미발견에 ?�른 ?�동변??
    private void Move_AccodingToSituation()
    {
        float navDis = Vector3.Distance(m_V_posi, this.transform.position);
        float playerDis = Vector3.Distance(this.transform.position, m_T_playerTr.position);

        if(navDis < 3f)
            RanPosi();
        if (playerDis < 15f)
        {
            this.m_N_nav.speed = 2;
            m_V_posi = m_T_playerTr.position;
            if (playerDis > 3f) //?�레?�어가 가까이 ?��??�않?�??주먹금�?
            {
                m_A_anim.SetBool("IsAttack", false);
                if (playerDis > 10f)
                    RanPosi();
            }
            else
                m_A_anim.SetBool("IsAttack", true);            
        }
        else
        {
            this.m_N_nav.speed = 1f;  
        }
        if(playerDis > 3f)
            transform.LookAt(m_V_posi);  

        m_N_nav.SetDestination(m_V_posi);     
    }

    private void RanPosi()
    {
        int i = Random.Range(0, 6);
        m_V_posi = T_inPoint[i].position;
    }
}
