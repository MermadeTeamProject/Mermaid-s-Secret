using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayeCtrl : MonoBehaviour
{
    //?�레?�어 ?�탯관??
    public string m_s_playerName;//?�레?�어 ?�름
    private float m_f_h;
    private float m_f_v;

    private Transform m_T_tr;
    private float m_f_speed = 4f; //마우???�전?�도
    [SerializeField]
    private float m_f_upPower = 1f; //?�승??가?�는 ??
    private Rigidbody m_R_rb;

    //?�하좌우 ?�동??마우???�전?�도
    [SerializeField]
    private float m_f_mouse_speedX = 3.0f;
    [SerializeField]
    private float m_f_mouse_speedY = 3.0f;
    private float m_f_rotationY = 0f;

    //?�이?�들
    private GameObject obj_Item;

    //key
    private int m_f_key = 0;

    //?�이???�집 ?�션관??
    private bool m_b_ItemRay;

    //?�집 UI
    [Header("Get UI")]
    [SerializeField] 
    private GameObject m_G_ButtonPanel;
    [SerializeField]
    private Text m_T_buttonText;

    //?�니메이??
    private Animator m_A_anim;

    static public int m_i_bottleCount = 0;
    static public int m_i_branchCount = 0;
    

    private void Awake()
    {
        init();
    }
    private void FixedUpdate()
    {
        catchItem();
    }

    #region (1) init  
    void init()
    {
        m_T_tr = this.GetComponent<Transform>();
        m_R_rb = this.GetComponent<Rigidbody>();
        m_A_anim = this.GetComponent<Animator>();
        m_s_playerName = PlayerPrefs.GetString("Name");
    }
    #endregion
    private void Update()
    {    
        SetForMove();
        WhenInput_E();
    }
    void LateUpdate()
    {      
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_f_mouse_speedX;
        m_f_rotationY -= Input.GetAxis("Mouse Y") * m_f_mouse_speedY;
        m_f_rotationY = Mathf.Clamp(m_f_rotationY, -20.0f, 60.0f);
        transform.localEulerAngles = new Vector3(m_f_rotationY, rotationX, 0);
    }

    #region _ Move
    void SetForMove()
    {
        m_f_h = Input.GetAxis("Horizontal");
        m_f_v = Input.GetAxis("Vertical");

        Vector3 dir = (Vector3.forward * m_f_v) + (Vector3.right * m_f_h);
        dir = dir.normalized;
        m_T_tr.Translate(dir * m_f_speed * Time.deltaTime, Space.Self);     
        if (m_f_v == 0 && m_f_h == 0)
            m_A_anim.SetBool("move", false);
        else
            m_A_anim.SetBool("move", true);
        if (Input.GetKey(KeyCode.Space))
            m_R_rb.AddForce(Vector3.up * m_f_upPower, ForceMode.Impulse);

        float canMoveX = Mathf.Clamp(this.transform.position.x, 18f, 125f);
        float canMoveY = Mathf.Clamp(this.transform.position.y, 1f, 10f);
        float canMoveZ = Mathf.Clamp(this.transform.position.z, 8f, 130f);
        this.transform.position = new Vector3(canMoveX, canMoveY, canMoveZ);
    }
    #endregion

    #region _ when u input "E" key
    private void WhenInput_E()
    {
        if (m_b_ItemRay)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (obj_Item.gameObject.name == "Guide")
                {
                    if (Enums.instance.gc == Enums.GuideCurr.STAY)
                    {
                        UIManager.instance.WhenGettin();
                        Enums.instance.pc = Enums.Particle.OFF;
                    }
                    else if (Enums.instance.gc == Enums.GuideCurr.PAUSE)
                    {
                        UIManager.instance.WhenGettin();
                        Enums.instance.pc = Enums.Particle.OFF;    
                    }
                }
                else if(obj_Item.gameObject.tag == "Key")
                {
                    UIManager.instance.WasKeyEatItem = true;
                    Destroy(obj_Item);
                }
                else if (obj_Item.gameObject.tag == "Ship")
                {
                    if(UIManager.instance.WasKeyEatItem == true)
                    {
                        //�� ��ȯ
                    }    
                }
                else
                {
                    if (Enums.instance.nq == Enums.NextQuest.ON)
                    {
                        if (obj_Item.gameObject.tag == "Bottle")
                        {
                            if (m_i_bottleCount < 5)
                                m_i_bottleCount += 1;
                            else
                            {
                                UIManager.instance.Print_LimitOrQuestNoti(1);
                                Enums.instance.nq = Enums.NextQuest.OFF;
                            }
                            UIManager.instance.WasBottleEatItem = true;
                            UIManager.instance.IsBottle = true;
                            Destroy(obj_Item);
                        }
                        else if (obj_Item.gameObject.tag == "Branch")
                        {
                            if (m_i_branchCount == 0)
                            {
                                m_i_branchCount += 1;
                                UIManager.instance.WasBrunchEatItem = true;
                                UIManager.instance.IsBottle = false;
                                Enums.instance.nq = Enums.NextQuest.OFF;
                                Destroy(obj_Item);
                            }
                        }
                    }
                    else
                        UIManager.instance.Print_LimitOrQuestNoti(2);
                }
                obj_Item = null;
                m_G_ButtonPanel.SetActive(false);
            }
        }
    }
    #endregion

    #region _ when catch item, change image of text under "E" & on outline image
    public void catchItem()
    {
        var camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 tmpv;
        tmpv = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);


        if (Physics.Raycast(tmpv, this.transform.forward, out hit, 20f))
        {
            if (hit.transform.gameObject.CompareTag("Branch") || hit.transform.gameObject.CompareTag("Bottle") || hit.transform.gameObject.CompareTag("Guide"))
            {     
                obj_Item = hit.transform.gameObject;
                obj_Item.GetComponent<Outline>().enabled = true;
                m_b_ItemRay = true;
                m_G_ButtonPanel.SetActive(true);
                if (hit.transform.gameObject.CompareTag("Guide"))
                    m_T_buttonText.text = "EŰ�� ���� ��ȭ";
                else
                    m_T_buttonText.text = "EŰ�� ���� ȹ��";
            }
            else if(hit.transform.gameObject.CompareTag("Ship") || hit.transform.gameObject.CompareTag("Key"))
            {
                obj_Item = hit.transform.gameObject;          
                m_b_ItemRay = true;
                m_G_ButtonPanel.SetActive(true);
                if(this.transform.gameObject.CompareTag("Ship"))
                    m_T_buttonText.text = "ž���ϱ�";
                else
                    m_T_buttonText.text = "EŰ�� ���� ȹ��";
            }                             
            else
            {
                if (obj_Item != null)
                {
                    obj_Item.GetComponent<Outline>().enabled = false;
                    m_b_ItemRay = false;
                    m_G_ButtonPanel.SetActive(false);
                }
                return;
            }
        }    
    }
    #endregion

    #region _ when collsion enter with enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            this.m_R_rb.AddForce(new Vector3(500, 500, 500));
            StartCoroutine(Hit());
        }
    }
    #endregion

    #region _ Coroutine_ on animation when hit
    IEnumerator Hit()
    {
        m_A_anim.SetBool("Hit", true);
        yield return new WaitForSeconds(.5f);
        m_A_anim.SetBool("Hit", false);
    }
    #endregion
}