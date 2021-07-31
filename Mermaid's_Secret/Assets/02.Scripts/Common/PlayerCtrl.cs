using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [Header("UI 관련")]
    [SerializeField] private GameObject m_G_ButtonPanel;
    [SerializeField] private Text m_T_CommandText;
    [SerializeField] private Image m_I_ButtonGage;


    [Header("조사하기 커맨드 관련")]
    private Component m_G_lookObject;  //조사하기 실행 시 표시할 오브젝트


    [Header("이동 조작 관련")]
    // ↓  섬 이동 조작 관련
    CharacterController C_chaCtrl;
    Vector3 V3_moveDir = Vector3.zero;
    public static bool m_b_canMove = true;

    private float m_f_moveSpeed = 3f;
    private float m_f_r;


    // ↓ 애니메이션 관련
    Animator A_animator;
    private bool m_b_isAttacking;
    [SerializeField] private BoxCollider m_B_weapon;

    // ↓ 기타 조작 관련
    GameObject G_Item;   //아이템 태그가 붙은 오브젝트를 관리하는 변수
    private bool m_b_ItemRay;    //아이템에 마우스가 올라가 있는지 확인하는 변수


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

        if (G_Item != null)
            GetItem();
    }

    void FixedUpdate()
    {
        ItemRaycast();
    }

    private void OnTriggerStay(Collider other)
    {
        LookPoint(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OutLookArea(other);
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

    // ↓ 상호작용 가능한 오브젝트 OutLine 관리 함수
    void ItemRaycast()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 20f))
        {
            if (hit.transform.gameObject.CompareTag("Item") || hit.transform.gameObject.CompareTag("DelayItem"))
            {
                G_Item = hit.transform.gameObject;
                G_Item.GetComponent<Outline>().enabled = true;
                m_b_ItemRay = true;
            }
            else
            {
                if (G_Item != null)
                {
                    G_Item.GetComponent<Outline>().enabled = false;
                    m_b_ItemRay = false;
                }
                return;
            }
        }
    }

    // ↓ 아이템 획득 조작 함수
    void GetItem()
    {
        float distance = Vector3.Distance(G_Item.transform.position, transform.position);

        if (m_b_ItemRay && distance <= 3f)   //아이템에 마우스가 올라가 있고 아이템과 플레이어간의 거리가 3f 이내일 때
        {
            m_T_CommandText.text = "획득";
            m_G_ButtonPanel.SetActive(true);    //키 조작 안내 UI ON

            if (Input.GetKey(KeyCode.E) && G_Item.CompareTag("DelayItem"))    //딜레이를 가진 아이템 획득 시
            {
                ButtonGage(1);
            }
            else if (Input.GetKeyDown(KeyCode.E) && G_Item.CompareTag("Item"))    //일반 아이템 획득 시
            {
                G_Item.SetActive(false);
            }
            else
            {
                ButtonGage(0);
            }
        }
        else if (m_b_ItemRay && distance > 3f)
        {
            m_G_ButtonPanel.SetActive(false);   //키 조작 안내 UI OFF
            m_I_ButtonGage.fillAmount = 0;      //게이지의 수치를 0으로 초기화
        }
        else
        {
            m_G_ButtonPanel.SetActive(false);
            m_I_ButtonGage.fillAmount = 0;
            G_Item = null;
        }
    }

    // ↓ 딜레이 UI 게이지 조정 및 커맨드 함수들
    void ButtonGage(int a)
    {
        float t;

        if (m_I_ButtonGage.fillAmount >= 0.8)
            t = Time.deltaTime * 6;

        else
            t = Time.deltaTime;


        switch (a)
        {
            case 0: //버튼의 게이지를 서서히 0으로 변화(공통)
                m_I_ButtonGage.fillAmount = Mathf.Lerp(m_I_ButtonGage.fillAmount, 0, t);
                break;

            case 1: //딜레이를 가진 아이템 획득 시
                m_I_ButtonGage.fillAmount = Mathf.Lerp(m_I_ButtonGage.fillAmount, 1, t);

                if (m_I_ButtonGage.fillAmount >= 0.99)
                {
                    m_G_ButtonPanel.SetActive(false);
                    m_I_ButtonGage.fillAmount = 0;
                    G_Item.SetActive(false);
                    G_Item = null;
                }
                break;

            case 2: //조사하기 커맨드 실행 시 
                m_I_ButtonGage.fillAmount = Mathf.Lerp(m_I_ButtonGage.fillAmount, 1, t);

                break;


        }
    }

    // ↓ 조사하기 함수 (OnCollisionStay 함수에 넣을 것)
    void LookPoint(Collider other)
    {
        if (other.CompareTag("LookPoint"))  //플레이어가 조사 영역 내에 있을 때 
        {
            m_T_CommandText.text = "조사";
            m_G_ButtonPanel.SetActive(true);

            if (Input.GetKey(KeyCode.E))    //플레이어가 E버튼을 누르고 있을 때 
            {
                ButtonGage(2);
            }
            else if (m_I_ButtonGage.fillAmount < 0.98 && !Input.GetKey(KeyCode.E))
            {
                ButtonGage(0);
            }

            if (m_I_ButtonGage.fillAmount >= 0.99)
            {
                m_G_ButtonPanel.SetActive(false);
                other.gameObject.GetComponent<LookPoint>().Action(other);
            }
        }
    }

    // ↓ 조사 영역을 벗어났을 때의 함수(OnCollisionExit 함수에 넣을 것)
    void OutLookArea(Collider other)
    {
        if (other.CompareTag("LookPoint"))
        {
            m_G_ButtonPanel.SetActive(false);
            m_I_ButtonGage.fillAmount = 0;

            if (other.gameObject.GetComponent<LookPoint>().m_b_doneLook)
            {
                // ↓ 조사가 끝났을 시 조사영역 off(조사가 끝났는데도 조사 버튼이 계속 뜨는 걸 방지) 
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                other.gameObject.GetComponent<LookPoint>().m_b_closePW = false;
            }
        }
    }
}
