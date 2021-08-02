using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [Header("이동 조작 관련")]
    // ↓  섬 이동 조작 관련
    CharacterController C_chaCtrl;
    Vector3 V3_moveDir = Vector3.zero;
    public static bool m_b_canMove = true;

    private float m_f_moveSpeed = 3f;
    private float m_f_r;


    [Header("애니메이션 관련")]
    Animator A_animator;
    private bool m_b_isAttacking;
    [SerializeField] private BoxCollider m_B_weapon;


    [Header("스탯 관련")]
    private int m_i_hp;   // 현재 HP(물)
    private int m_i_satiety;   // 현재 포만감

    private int m_i_maxHp = 100;    //최대 HP
    private int m_i_maxSatiety = 50;    //최대 포만감


    private void Awake()
    {
        m_i_hp = m_i_maxHp;
        m_i_satiety = m_i_maxSatiety;
    }

    void Start()
    {
        Init();
    }

    // ↓ Start 함수에서 실행할 내용들
    void Init()
    {
        C_chaCtrl = GetComponent<CharacterController>();
        A_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!m_b_canMove|| m_b_isAttacking)
            C_chaCtrl.SimpleMove(Vector3.zero);
        else
            Move();

        Ani();
    }

    


    // ↓ 섬 맵 플레이어 이동 조작 함수
    void Move()
    {
        m_f_r = Input.GetAxisRaw("Mouse X");

        if (C_chaCtrl.isGrounded)
        {
            V3_moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            V3_moveDir = transform.TransformDirection(V3_moveDir.normalized);
            V3_moveDir *= m_f_moveSpeed;
        }

        C_chaCtrl.SimpleMove(V3_moveDir);
        transform.Rotate(m_f_r * transform.up);

    }

    // ↓ 플레이어 애니메이션 함수
    void Ani()
    {
        if (C_chaCtrl.isGrounded)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.F))
            {  //공격 모션
                m_b_isAttacking = true;
                A_animator.SetTrigger("Attack");
                m_B_weapon.tag = "PlayerWeapon";
            }
            else if (!m_b_isAttacking && (h != 0 || v != 0) && Input.GetKey(KeyCode.LeftShift))
            {  //달리기 모션
                A_animator.SetBool("isWalk", false);
                A_animator.SetBool("isRun", true);
                m_f_moveSpeed = 6f;
            }
            else if (!m_b_isAttacking && (h != 0 || v != 0))
            {  //걷기 모션
                A_animator.SetBool("isWalk", true);
                A_animator.SetBool("isRun", false);
                m_f_moveSpeed = 3f;
            }
            
            else
            {  //Idle
                A_animator.SetBool("isWalk", false);
                A_animator.SetBool("isRun", false);
            }
        }
    }

    // ↓ 공격 애니메이션 시 무기의 태그, 콜라이더 활성화/비활성화 용으로 넣을 이벤트 함수
    void doneAttack()
    {
        m_b_isAttacking = false;
        m_B_weapon.tag = "Untagged";
    }

    void enableAttackCollider()
    {
        m_B_weapon.enabled = true;
    }

    void disableWeaponCollider()
    {
        m_B_weapon.enabled = false;
    }



    //===플레이어 스탯 관련 함수=====================================================================

    public void healHP(int value)
    {
        if (m_i_hp < m_i_maxHp)
        {
            m_i_hp += value;
            if (m_i_hp > m_i_maxHp)
            {
                m_i_hp = m_i_maxHp;
            }
        }
    }

    public void healSatiety(int value)
    {
        if (m_i_satiety < m_i_maxSatiety)
        {
            m_i_satiety += value;
            if (m_i_satiety > m_i_maxSatiety)
            {
                m_i_hp = m_i_maxHp;
            }
        }
    }

    public void damage(int value)
    {
        m_i_hp -= value;
    }
}
