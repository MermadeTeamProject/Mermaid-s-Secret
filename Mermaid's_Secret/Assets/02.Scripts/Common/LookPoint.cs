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

    [SerializeField] private GameObject m_G_pwPanel;    //패스워드 UI 패널

    private Component m_G_lookObject;   //조사 영역의 자식으로 들어 있는 오브젝트를 저장하는 변수
    public bool m_b_doneLook;   //조사하기가 완전히 완료되었는지 확인하는 변수
    public bool m_b_closePW = false;    //패스워드 UI 패널 닫기 용 변수


    // ↓ 조사하기 실행 시 일하는 함수... 조사영역의 타입에 따라 실행 함수가 달라짐
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

                m_b_doneLook = true;    //조사하기가 끝났는지 확인하는 변수를 true로 변경
                break;

            case Type.Password:
                if (!m_b_closePW)
                {
                    StartCoroutine(openPwPanel());
                    m_G_pwPanel.SetActive(true);
                   PlayerCtrl.m_b_canMove = false;
                }
                break;
        }
    }

    // ↓ 패스워드 UI 패널 열리는 연출용 코루틴
    IEnumerator openPwPanel()
    {
        for (float a = 0f; a >= 1.1f; a += 0.1f)
        {
            m_G_pwPanel.transform.localScale = new Vector3(m_G_pwPanel.transform.localScale.x, a, m_G_pwPanel.transform.localScale.z);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
