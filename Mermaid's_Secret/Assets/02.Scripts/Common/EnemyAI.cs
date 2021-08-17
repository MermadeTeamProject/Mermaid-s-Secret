using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        IDLE,
        WALK,
        TRACE,
        ATTACK,
        DAMAGED,
        DIE,
    }

    public State state;

    //==NevMesh 관련 변수======================================================================

    [SerializeField] Transform m_G_wayPointGroup;   //하이어라키에서 WayPoints를 담고 있는 부모 오브젝트
    [SerializeField] List<Transform> m_L_wayPoints; //WayPoint 위치를 저장할 리스트
    [SerializeField] CapsuleCollider m_G_AttackCollider;    //공격 상태 시 활성화 할 주먹 콜라이더
    [SerializeField] private int m_i_nextIdx;   //에너미가 이동할 다음 목적지(WayPoint)를 저장하기 위한 변수
    [SerializeField] private float m_f_traceDistance = 7f;  //에너미의 추적 범위
    [SerializeField] private float m_f_attackDistance = 3f; //에너미의 공격 가능 범위

    NavMeshAgent m_N_agent;
    Animator m_A_animator;
    WaitForSeconds m_W_ws;
    Transform m_T_player;

    private float m_f_walkSpeed = 2f;   //순찰 상태에서의 이동 속도
    readonly float m_f_traceSpeed = 5f; //추적 상태에서의 이동 속도

    private bool m_b_isDamaged = false; //에너미가 공격받았는지 확인하는 변수
    private bool m_b_isDie = false;     //에너미가 죽었는지 확인하는 변수
    private bool m_b_isArrive = false; //에너미가 목적지(waypoint)까지 도달했는지 확인하는 변수

    readonly int hashIdle = Animator.StringToHash("IDLE");
    readonly int hashWALK = Animator.StringToHash("WALK");
    readonly int hashTrace = Animator.StringToHash("TRACE");
    readonly int hashAttack = Animator.StringToHash("ATTACK");
    readonly int hashDamaged = Animator.StringToHash("DAMAGED");
    readonly int hashDie = Animator.StringToHash("DIE");

    //==Enemy 피격 관련====================================================================

    private int m_i_hp = 30;

    //==파라미터 ==========================================================================
    #region
    private bool m_b_patrol;
    private bool Patrolling
    {
        get
        {
            return m_b_patrol;
        }

        set
        {
            m_b_patrol = value;   //value: set 접근자가 할당하는 값을 정의하는 데에 사용
                                  //set 동작 시 전달받은 값을 value에 대입, value의 값을 walk 변수에 전달

            if (m_b_patrol)   //m_b_walk = true일 시
            {
                m_N_agent.speed = m_f_walkSpeed;

                //경로 변경 함수
                WayTarget();
            }
        }
    }

    Vector3 _target;
    Vector3 target
    {
        get
        {
            return _target;
        }

        set
        {
            _target = value;
            m_N_agent.speed = m_f_traceSpeed;

            //추적대상 지정 함수
            TraceTarget(_target);
        }
    }
    #endregion

    //==Unity 함수========================================================================
    #region
    void Awake()
    {
        m_A_animator = GetComponent<Animator>();

        m_N_agent = GetComponent<NavMeshAgent>();
        m_i_nextIdx = Random.Range(0, m_L_wayPoints.Count);

        m_N_agent.updateRotation = true;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        m_T_player = playerObj.GetComponent<Transform>();

        m_G_wayPointGroup.GetComponentsInChildren<Transform>(m_L_wayPoints);
        m_L_wayPoints.RemoveAt(0);

        m_W_ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        m_i_hp = 30;
        m_b_isDie = false;
        m_b_isDamaged = false;
        m_b_isArrive = false;
        state = State.WALK;
        m_i_nextIdx = Random.Range(0, m_L_wayPoints.Count);
        StartCoroutine(StateCheck());
        StartCoroutine(Action());
    }

    void Update()
    {
        if (!m_b_isDie)
        {

            if (!m_b_patrol)  //enemy가 순찰상태가 아니라면
            {
                return; //현재 시점에서 실행 중인 Update문 종료
            }

            if (m_N_agent.remainingDistance <= 0.3f)    //에너미와 목적지(WayPoint)까지의 거리가 0.3 이하일 시
            {
                m_b_isArrive = true;
            }
        }
    }


    private void LateUpdate()
    {
        if (state == State.ATTACK)
        {
            transform.LookAt(new Vector3(m_T_player.position.x,transform.position.y,m_T_player.position.z));
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
                m_b_isDamaged = true;
                m_i_hp -= 5;
                if (m_i_hp <= 0)
                {
                    state = State.DIE;
                }
                else
                {
                    state = State.DAMAGED;
                }
        }
    }

    #endregion


    //==작성 함수==========================================================================
    #region

    void WayTarget()    //경로 변경 함수
    {
        if (m_N_agent.isPathStale)  //AI가 경로 계산 중일 때는
        {
            return; //함수 실행 안 함
        }

        m_N_agent.destination = m_L_wayPoints[m_i_nextIdx].position;
        m_N_agent.isStopped = false;
    }

    void TraceTarget(Vector3 target)   //추적 함수
    {
        if (m_N_agent.isPathStale)
        {
            return;
        }

        m_N_agent.destination = target; 
        m_N_agent.isStopped = false;
    }

    void Stop() //정지 함수
    {
        m_N_agent.isStopped = true;
        m_N_agent.ResetPath();
    }

    IEnumerator StateCheck()   //에너미 스탯 변경 함수
    {
        yield return new WaitForSeconds(1f);

        while (!m_b_isDie)
        {
            if(state == State.DIE)
            {
                yield break;
            }

            
            else
            {
                float dist = Vector3.Distance(m_T_player.position, transform.position); //플레이어와 자신(Enemy) 사이의 거리를 계산

                if (dist <= m_f_attackDistance&& !m_b_isDamaged) //플레이어가 에너미의 공격 범위 안에 들어갔을 때
                {
                    state = State.ATTACK;   //에너미의 스탯을 공격 상태로 변경                    
                }

                else if (dist <= m_f_traceDistance&& !m_b_isDamaged)   //플레이어가 에너미의 추적 범위 안에 들어왔을 때
                {
                    state = State.TRACE;    //에너미의 스탯을 추적 상태로 변경
                }

                else if (dist >= m_f_traceDistance && state == State.TRACE&& !m_b_isDamaged) //플레이어가 에너미의 추적 범위를 벗어나고 에너미의 스탯이 추적 상태일 때
                {
                    state = State.WALK; //에너미가 WayPoint로 이동하도록 에너미의 스탯을 Walk로 변경
                }

                else if (m_b_isArrive&& !m_b_isDamaged)  //에너미가 목적지(WayPoint)에 도달했을 때 
                {
                    state = State.IDLE; //에너미의 상태를 Idle로 변경
                    m_i_nextIdx = Random.Range(0, m_L_wayPoints.Count); //wayPoint랜덤 이동을 위한 변수값에 난수 할당
                    WayTarget();
                    m_b_isArrive = false;   //혹시 모를 버그를 위해 변수에 강제로 false 할당
                }

                else if (!m_b_isArrive && state == State.IDLE&& !m_b_isDamaged)  //에너미가 아직 목적지(WayPoint)에 도달하지 않았고 Idle 상태일 때
                {
                    state = State.WALK;
                }
            }

            yield return m_W_ws;    // 무한루프 방지를 위한 WaitForSeconds
        }
    }

    IEnumerator Action()   //스탯에 따른 이동 및 애니메이션 제어 함수
    {
        while (!m_b_isDie)  //에너미가 죽지 않는 한 해당 함수 반복
        {
            yield return m_W_ws;    // 무한루프 방지를 위한 WaitForSeconds

            switch (state)
            {
                case State.IDLE:    //Idle 상태일 때의 애니메이터 파라메터 조정
                    m_G_AttackCollider.enabled = false;
                    disableAttackTrail();
                    Patrolling = false;
                    Stop();
                    m_A_animator.SetBool(hashIdle, true);
                    m_A_animator.SetBool(hashWALK, false);
                    m_A_animator.SetBool(hashTrace, false);
                    m_A_animator.SetBool(hashAttack, false);

                    break;

                case State.WALK:    //Idle 상태일 때의 애니메이터 파라메터 조정
                    m_G_AttackCollider.enabled = false;
                    disableAttackTrail();
                    Patrolling = true;
                    m_N_agent.stoppingDistance = 0.5f;
                    
                    m_A_animator.SetBool(hashTrace, false);
                    m_A_animator.SetBool(hashAttack, false);
                    m_A_animator.SetBool(hashIdle, false);
                    m_A_animator.SetBool(hashDamaged, false);
                    m_A_animator.SetBool(hashWALK, true);
                    break;

                case State.TRACE:   //TRACE 상태일 때의 애니메이터 파라메터 조정
                    m_G_AttackCollider.enabled = false;
                    disableAttackTrail();
                    target = m_T_player.position;
                    m_N_agent.stoppingDistance = 2.3f;

                    m_A_animator.SetBool(hashIdle, false);
                    m_A_animator.SetBool(hashAttack, false);
                    m_A_animator.SetBool(hashWALK, false);
                    m_A_animator.SetBool(hashDamaged, false);
                    m_A_animator.SetBool(hashTrace, true);
                    break;

                case State.ATTACK:  //ATTACK 상태일 때의 애니메이터 파라메터 조정
                    m_A_animator.SetBool(hashDamaged, false);
                    m_A_animator.SetBool(hashIdle, false);
                    m_A_animator.SetBool(hashTrace, false);
                    m_A_animator.SetBool(hashWALK, false);
                    m_A_animator.SetBool(hashAttack, true);
                    Stop();
                    break;

                case State.DAMAGED: //DAMAGED 상태일 때의 애니메이터 파라메터 조정
                    disableAttackTrail();
                    m_A_animator.SetBool(hashIdle, false);
                    m_A_animator.SetBool(hashTrace, false);
                    m_A_animator.SetBool(hashWALK, false);
                    m_A_animator.SetBool(hashAttack, false);
                    m_A_animator.SetBool(hashDamaged,true);
                    break;

                case State.DIE: //DIE 상태일 때의 애니메이터 파라메터 조정
                    m_b_isDie = true;
                    disableAttackTrail();
                    Stop();
                    m_A_animator.SetTrigger(hashDie);

                    GetComponent<CapsuleCollider>().enabled = false;
                    m_G_AttackCollider.enabled = false;
                    yield return new WaitForSeconds(4f);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    #endregion




    //애니메이션 이벤트 클립에 사용할 함수들==================================================

    //공격 모션 시 주먹 콜라이더 활성화
    void enableAttackCollider()
    {
        m_G_AttackCollider.enabled = true;
    }

    //공격 모션 종료 시 주먹 콜라이더 비활성화
     void disableAttackCollider()
    {
        m_G_AttackCollider.enabled = false;
    }

    //공격 모션 시 Trail Renderer 활성화
     void enableAttackTrail()
    {
        m_G_AttackCollider.gameObject.GetComponent<TrailRenderer>().enabled = true;
    }

    //공격 모션 시 Trail Renderer 비활성화
     void disableAttackTrail()
    {
        m_G_AttackCollider.gameObject.GetComponent<TrailRenderer>().enabled = false;
    }


    //피격 후 스탯을 Trace로 변화
     void setStateTrace()
    {
        m_b_isDamaged = false;
        //transform.position += new Vector3(-1, 0, -1.46f);
        state = State.ATTACK;
    }

}
