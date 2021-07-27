using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Password : MonoBehaviour
{
    [SerializeField] private LookPoint m_G_thisLookPoint;
    [SerializeField] private GameObject m_G_pwPanel;
    [SerializeField] private Button m_B_access;
    [SerializeField] private Text m_T_guide;

    private TMP_InputField  m_TMP_inputField;


    private void Start()
    {
        m_TMP_inputField = GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            m_G_pwPanel.SetActive(false);
            PlayerCtrl.m_b_canMove = true;
        }
    }

    public void checkInput()
    {
        if (m_TMP_inputField.text == "8413")
        {
            StartCoroutine(changeLog(true));
        }
        else
        {
            StartCoroutine(changeLog(false));
        }
    }

    IEnumerator changeLog(bool correct)
    {
        switch (correct)
        {
            case true:
                m_T_guide.text = "Access Succeed.";
                m_B_access.interactable = false;
                yield return new WaitForSeconds(2f);
                yield return StartCoroutine(closePwPanel());
                PlayerCtrl.m_b_canMove = true;
                m_G_pwPanel.SetActive(false);
                m_G_thisLookPoint.m_b_closePW = true;
                m_G_thisLookPoint.m_b_doneLook = true;
                yield return null;
                break;

            case false:
                m_B_access.interactable = false;
                m_T_guide.text = "Incorrect Password";
                m_T_guide.color = Color.red;
                yield return new WaitForSeconds(2f);
                m_T_guide.text = "Enter the Password";
                m_T_guide.color = new Color(0.4198113f, 1, 0.9812263f,1);
                m_TMP_inputField.text = null;
                m_B_access.interactable = true;
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

    IEnumerator closePwPanel()
    {
        for (float a = 1.1f; a >= 0f; a -= 0.1f)
        {
            m_G_pwPanel.transform.localScale = new Vector3(m_G_pwPanel.transform.localScale.x, a, m_G_pwPanel.transform.localScale.z);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
