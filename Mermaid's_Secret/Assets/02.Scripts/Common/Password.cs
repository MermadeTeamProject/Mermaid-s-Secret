using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Password : MonoBehaviour
{ //해당 스크립트는 Inputfield(TMP)에 집어넣을 것

    public enum Scene
    {
        Island,
        Lab,
    }

    public Scene scene;
    string m_s_password;

    [SerializeField] private LookPoint m_G_thisLookPoint;  //해당 UI 패널에 연결되어 있는 lookPoint 오브젝트(lookPoint 오브젝트에 상호작용 시 이 UI가 표시됨)
    [SerializeField] private GameObject m_G_pwPanel;    //패스워드 UI 패널
    [SerializeField] private Button m_B_access;         //Access 버튼 UI 
    [SerializeField] private Text m_T_guide;            //안내 텍스트
    [SerializeField] private BoxCollider[] m_Box_Door; //해당 스크립트의 비밀번호를 맞출 시 활성화하고자 하는 도어 콜라이더(Trigger)

    private TMP_InputField  m_TMP_inputField;    //Inputfield를 변수에 저장


    private void Awake()
    {
        m_TMP_inputField = GetComponent<TMP_InputField>();
        switch (scene)
        {
            case Scene.Island:
                m_s_password = "8413";
                break;

            case Scene.Lab:
                m_s_password = "Yeil";
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(openPwPanel());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(close_());
        }
    }

    public void checkInput()
    {
        if (m_TMP_inputField.text == m_s_password)    //비밀번호 일치 시 
        {
            StartCoroutine(changeLog(true));
        }
        else//틀린 비밀번호일 시
        {
            StartCoroutine(changeLog(false));
        }
    }

    IEnumerator changeLog(bool correct)
    {
        switch (correct)
        {
            case true:  //비밀번호 일치 시 
                m_T_guide.text = "Access Succeed."; //안내 텍스트 변경
                m_B_access.interactable = false;    //버튼 상호작용 금지
                yield return new WaitForSeconds(2f);

                yield return StartCoroutine(closePwPanel());//패널 닫는 연출이 끝날 때까지 대기

                PlayerCtrl.m_b_canMove = true;  //플레이어 이동 제한 해제

                m_G_pwPanel.SetActive(false);   //패널 UI 끄기
                m_G_thisLookPoint.m_b_closePW = true;   //패스워드 UI 패널 닫기 용 변수를 true로 변경
                m_G_thisLookPoint.m_b_doneLook = true;  //조사가 완전히 끝났으므로 m_b_doneLook를 true로 변경

                EnableDoorCollider();   //문의 트리거 콜라이더 활성화
                break;


            case false://틀린 비밀번호일 시
                m_B_access.interactable = false;    //버튼 상호작용 금지
                m_T_guide.text = "Incorrect Password";  //안내 텍스트 변경
                m_T_guide.color = Color.red;            //안내 텍스트 색상 변경
                yield return new WaitForSeconds(2f);

                m_T_guide.text = "Enter the Password";  //안내 텍스트 원 상태로 변경
                m_T_guide.color = new Color(0.4198113f, 1, 0.9812263f,1);   //안내 텍스트 색상 원 상태로 변경

                m_TMP_inputField.text = null;   //플레이어가 입력했던 텍스트를 지워줌
                m_B_access.interactable = true; //버튼 상호작용 허용
                break;

        }
    }

    IEnumerator openPwPanel()   //패널 열리는 연출 코루틴 함수
    {
        for (float a = 0f; a <= 1.2f; a += 0.1f)
        {
            m_G_pwPanel.transform.localScale = new Vector3(m_G_pwPanel.transform.localScale.x, a, m_G_pwPanel.transform.localScale.z);
            yield return new WaitForSeconds(0.003f);
        }
    }

    IEnumerator closePwPanel()  //패널 닫히는 연출 코루틴 함수
    {
        for (float a = 1.1f; a >= 0f; a -= 0.1f)
        {
            m_G_pwPanel.transform.localScale = new Vector3(m_G_pwPanel.transform.localScale.x, a, m_G_pwPanel.transform.localScale.z);
            yield return new WaitForSeconds(0.003f);
        }
    }

    IEnumerator close_()    //ESC키를 통해 비밀번호 입력창에서 나갈 때 패널 닫히는 연출 및 제한 변수 조작
    {
        yield return StartCoroutine(closePwPanel());    //패널 닫는 연출이 끝날 때까지 대기
        m_G_pwPanel.SetActive(false);   //패널 UI 끄기
        PlayerCtrl.m_b_canMove = true;  //플레이어의 이동 제한을 해제
        m_G_thisLookPoint.m_b_closePW = true;   ////패스워드 UI 패널 닫기 용 변수를 true로 변경
    }

    public void EnableDoorCollider()
    {
        for (int i = 0; i < 2; i++)
        {
            m_Box_Door[i].enabled = true;
        }
    }

}
