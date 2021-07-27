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

    [SerializeField] GameObject m_G_enterPwPanel;
    [SerializeField] private GameObject m_G_pwPanel;

    private Component m_G_lookObject;
    public bool m_b_doneLook;
    public bool m_b_closePW = false;

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
                    m_G_enterPwPanel.SetActive(true);
                    PlayerCtrl.m_b_canMove = false;
                }
                break;
                

        }
    }

    IEnumerator openPwPanel()
    {
        for (float a = 0f; a >= 1.1f; a += 0.1f)
        {
            m_G_pwPanel.transform.localScale = new Vector3(m_G_pwPanel.transform.localScale.x, a, m_G_pwPanel.transform.localScale.z);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
