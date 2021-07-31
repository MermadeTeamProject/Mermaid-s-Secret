using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyCtrl : MonoBehaviour
{


    public enum State
    {
        IDLE,//정지
        WALK, //순찰
        RUN, //추적
        ATTACK, //공격
        DIE, //사망
    }

    public State state = State.WALK;//초기 상태 (순찰)

    public float moveSpeed;




    Transform playerTr;//플레이어 위치 저장 변수
    Transform enemyTr;//적 위치 저장 변수

    public float attackDist = 5; //공격 사거리
    public float traceDist = 10f; //추적 사거리
    public bool isDie = false;//사망 여부 판단 변수

    WaitForSeconds ws;//시간 지연 변수
    public Transform target;//적의 목적지(플레이어)
    

    Transform tr;//움직임
    Rigidbody rb;//리지드바디
    Animator anim;//애니메이터
    NavMeshAgent agent;//적의 움직임 nav
    EnemyCtrl attack;//enemy 공격
    PlayerCtrl player;

    


    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashRun = Animator.StringToHash("IsRun");
    readonly int hashDie = Animator.StringToHash("IsDie");

    Vector3 destination;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();//enemy 정의
        tr = GetComponent<Transform>();

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GetComponent<PlayerCtrl>();
        

        target = GameObject.FindWithTag("Player").transform;
        //타겟은 플레이어
        agent.destination = target.transform.position;
        //enemy의 목적지
        
        //생성시 WALK 상태로 만듦 (순찰)
        state = State.WALK;

        moveSpeed = 3f;

    }

    void Update()
    {
        destination = target.transform.position;

        if(state == State.WALK)
        {
            UpdateWalk();
        }
        else if (state == State.RUN)
        {
            UpdateRunning();
        }
        else if (state == State.ATTACK)
        {
            UpdateAttack();
        }

    }

    private void UpdateAttack()
    {
        agent.speed = 0;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 2)
        {
            state = State.RUN;
            anim.SetTrigger("IsRun");
        }
    }

    private void UpdateRunning()
    {
        //남은 거리가 1미터일 때 공격한다
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 2)
        {
            state = State.ATTACK;
            anim.SetTrigger("IsAttack");
        }

        //타겟 방향으로 이동
        agent.speed = 5f;
        //enemy에게 목적지 알려줌
        agent.destination = target.transform.position;

    }

    private void UpdateWalk()
    {
        agent.speed = 0;
        //시작시 목적지 (player)찾기
        target = GameObject.Find("Player").transform;
        //타겟을 찾았을 때 Running 상태로 전이
        if(target != null)
        {
            state = State.RUN;
            //animation 동기화
            anim.SetTrigger("IsRun");
        }
    }



}
