using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPoint : MonoBehaviour
{   //조사 영역(LookPoint_~)에 넣는 스크립트
    public enum Type
    {
        Bridge=0,
        Password,
    }
    public Type type = Type.Bridge;

    public bool m_b_doneLook;   //조사하기가 완전히 완료되었는지 확인하는 변수
    private Component m_G_lookObject;   //조사 영역의 자식으로 들어 있는 오브젝트를 저장하는 변수

    [Header("다리(Bridge) 관련")]
    [SerializeField] private Material finalRender;  //다리 설치 시 최종적으로 적용되어야 할 머테리얼
    public bool m_b_look;


    [Header("비밀번호 시스템 관련")]
    [SerializeField] private GameObject m_G_pwPanel;    //패스워드 UI 패널
    public bool m_b_closePW = false;    //패스워드 UI 패널 닫기 용 변수


    // ↓ 조사하기 실행 시 일하는 함수... 조사영역의 타입에 따라 실행 함수가 달라짐(외부 참조용)
    public void Action(Collider other)
    {
        switch (type)
        {
            case Type.Bridge:
                // ↓ 조사 영역의 자식으로 들어 있는 오브젝트의 머테리얼 변경을 위해 Renderer에 접근
                m_G_lookObject = other.GetComponentInChildren(typeof(Renderer));
                // ↓ float 변수에 조사 영역의 자식으로 들어 있는 오브젝트의 알파값을 저장
                float bridgeMatA = m_G_lookObject.GetComponent<Renderer>().material.color.a;
                // ↓ 조사 영역의 자식으로 들어 있는 오브젝트의 알파값을 70까지 서서히 조정(힌트 오브젝트가 보이도록 하기 위해)
                m_G_lookObject.GetComponent<Renderer>().material.color
                    = new Color(1, 1, 1, Mathf.Lerp(bridgeMatA, 70 / 255f, Time.deltaTime));

                m_b_look = true;    //조사하기가 끝났는지 확인하는 변수를 true로 변경
                break;

            case Type.Password:
                if (!m_b_closePW)
                {
                   m_G_pwPanel.SetActive(true);
                   PlayerCtrl.m_b_canMove = false;
                }
                break;
        }
    }


    public void setBridge(Collider other)   //다리 설치 시 머테리얼 및 콜라이더 옵션 변경해주는 함수 (외부 참조용)
    {
        m_G_lookObject = other.GetComponentInChildren(typeof(Renderer));
        m_G_lookObject.GetComponent<Renderer>().material= finalRender;
        m_G_lookObject.GetComponent<BoxCollider>().isTrigger = false;
        m_b_doneLook = true;
    }
}
