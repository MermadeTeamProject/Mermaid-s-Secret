using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; //��ǲ�ؽ�Ʈ �˿�
using UnityEngine.SceneManagement;

public class IntroUI : MonoBehaviour
{
    //�̺κ� ���߿� �̱������� �׳� �������� ���� ���...
    public InputField inputName;
    public Text inputbar;

    private string tmpstr;

    //���߿� Json���� �����ϱ�
   

    private void Start()
    {
        StartCoroutine(TwinkleInputBar());
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            tmpstr = inputName.text.Replace(" ", "");
            inputName.text = tmpstr;
        }
    }
    public void OnSave()
    {
        PlayerPrefs.SetString("Name", inputName.text); //�̸� ����
        SceneManager.LoadScene("GameScene");
    }
    public void OnLoad()
    {
        PlayerPrefs.GetString("Name");
    }

    IEnumerator TwinkleInputBar()
    {
        while(true) 
        { 
                inputbar.text = "l";
                yield return new WaitForSeconds(1f);
                inputbar.text = "";
                yield return new WaitForSeconds(1f);
        }
    }
}
