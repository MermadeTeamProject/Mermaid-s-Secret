using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region # singleton
    private static UIManager m_UI_instance = null;
    public static UIManager instance
    {
        get
        {
            if(m_UI_instance == null)
            {
                return null;
            }
            return m_UI_instance;
        }
    }
    #endregion

    // if u pick up Item
     public bool WasBottleEatItem = false;
     public bool WasBrunchEatItem = false;
    public bool WasKeyEatItem = false;

    //bout Guide's Talk
    public Text T_guide;
    private List<string> m_s_TalkList = new List<string>();
    //private string[] m_s_TalkList;
    public int m_i_talkCount = 0; //���° ��ȭ����
    //stop talk and start moving for item pick up guide

    //for Quest
    public Text G_quest;
    public GameObject G_questUI;
    private List<string> m_s_questList = null; //?�스??목록    
    private int m_i_questCount = 0;

    [Header("Go to QuickSlot")]
    public Image[] I_quickSlotArr; 
    private int m_i_slotCount = 0;
    public Text[] T_quickCount;
    public Text T_limitText;

    public Sprite S_key;
    public Sprite S_bottle;
    public Sprite S_brunch;
    public  bool IsBottle = false;

    [Header("Item InFo Text")]
    public Text T_infotext;

    [Header("Guide Talking Fade In")]
    [SerializeField]
    private Image m_I_fade;

    private void Awake()
    {
        if (m_UI_instance == null)
        {          
            m_UI_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //else
        //{
        //    //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
        //    //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
        //    //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
        //    Destroy(this.gameObject);
        //}
        init();
    }
    private void init()
    {
        m_s_questList = new List<string>();
        StartCoroutine(FaidIn());
        talkList();
        StartCoroutine(TypingGuide());
        questListArr();
    }    
    private void Update()
    {     
        if (WasBottleEatItem || WasBrunchEatItem || WasKeyEatItem)
            QuickSlotUI();
        if (Enums.instance.nq == Enums.NextQuest.ON)
        {
            PrintQuest();
        }
    }

    #region _ when u pick up item, change of quick slot image //have to modify number = 1
    public void QuickSlotUI()
    {
        FindEmptySlot();     
        if (WasBottleEatItem && IsBottle)
        {                      
            I_quickSlotArr[m_i_slotCount].sprite = S_bottle;
            T_quickCount[m_i_slotCount].text = PlayeCtrl.m_i_bottleCount.ToString();
            WasBottleEatItem = false;
            IsBottle = false;
        }
        else if(WasBrunchEatItem && IsBottle == false) //have to modify number = 1
        {
            I_quickSlotArr[m_i_slotCount].sprite = S_brunch;
            T_quickCount[m_i_slotCount].text = PlayeCtrl.m_i_branchCount.ToString();
            WasBrunchEatItem =false;
        }
        else if(WasKeyEatItem)
        {
            I_quickSlotArr[m_i_slotCount].sprite = S_key;
            T_quickCount[m_i_slotCount].text = "1";
            WasKeyEatItem = false;
        }
    }
    #endregion

    #region _ find where is empty quickslot
    private void FindEmptySlot()
    {
        //
        for(int i = 0; i< I_quickSlotArr.Length; i++)
        {
            //���ڸ��� ã������ �տ� ������ �ִ��� ã�ƾߵɰŰ�����?     
            if (I_quickSlotArr[i].sprite == S_bottle && IsBottle)
            //�����׸��� �������鼭 && ������ �����̸�
            {
                m_i_slotCount = i;
                break;
            }
            else if(I_quickSlotArr[i].sprite == S_brunch && !IsBottle)
            //�����׸��� �������鼭 && ������ ������
            {
                m_i_slotCount = i;
                break;
            }                      
            if(I_quickSlotArr[i].enabled ==false) 
                //�����ִ� ĭ�� ������
            {
                m_i_slotCount = i; //�װ� ���²�� ����
                I_quickSlotArr[i].enabled = true; //�� ĭ�� Ȱ��ȭ
                break;
            }       
        }
    }
    #endregion

    #region _ if u didnt something, will print these
    public void Print_LimitOrQuestNoti(int text)
    {
        StartCoroutine(_Print_LimitOrQuestNoti(text));
    }

    IEnumerator _Print_LimitOrQuestNoti(int text)
    {
        if (text == (int)Enums.Limit_Quest.LIMITTEXT)
            T_limitText.text = "���Ѽ��� �ʰ�(10��)";
        else if (text == (int)Enums.Limit_Quest.QUESTNOTI)
            T_limitText.text = "����� ��ȭ�ϼ���.";
        Enums.instance.lq = Enums.Limit_Quest.NOTHING;
        yield return new WaitForSeconds(1f);
        T_limitText.text = "";
    }
    #endregion
    public void WhenGettin()
    {
        if (m_i_talkCount >=4 && m_i_talkCount < 7)
        {          
            if (m_i_talkCount == 5)
                PrintQuest();
            if (m_i_talkCount == 6)
                Enums.instance.nq = Enums.NextQuest.ON;
            m_I_fade.enabled = true;
            T_guide.enabled = true;
            Enums.instance.gc = Enums.GuideCurr.FIRSTMOVE;
            StartCoroutine(TypingGuide());
            return;
        }
        else if (m_i_talkCount >=7)
        {
            if (WasBottleEatItem == false)
                return;
            Enums.instance.gc = Enums.GuideCurr.NEXTMOVE;
            Enums.instance.nq = Enums.NextQuest.ON;
            StartCoroutine(TypingGuide());
            return;
        }
        StartCoroutine(TypingGuide());
    }

    #region  _ Coroutine_ printing text about Guide's Talk 
    IEnumerator TypingGuide()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < m_s_TalkList[m_i_talkCount].Length; i++)
        {
            T_guide.text = m_s_TalkList[m_i_talkCount].Substring(0, i + 1);
            yield return new WaitForSeconds(.05f);
        }
        if (m_i_talkCount == 4)
        {
            PrintQuest();
            yield return new WaitForSeconds(1f);
            m_I_fade.enabled = false;
            T_guide.enabled = false;
        }
        if(m_i_talkCount+1 <7)
            m_i_talkCount++;
    }
    #endregion

    #region _ Coroutine_ guide's speaking image fade in
    //���̵���
    IEnumerator FaidIn()
    {
        for (float i = 0; i < 100f; i += .1f)
        {
            m_I_fade.color = new Color(255, 255, 255, i);
            yield return new WaitForSeconds(.3f);
        }
    }
    #endregion

    #region _ list of quset text . add
    void questListArr()
    {
        m_s_questList.Add("��  ����� ���󰡼���. ");
        m_s_questList.Add("��  ������ ȹ���ϼ���. ");
        m_s_questList.Add("��  ���⸦ �غ��ϼ���. ");
    }
    #endregion

    #region func bout print of quest Text
    private void PrintQuest()
    {
        G_questUI.SetActive(true);
        G_quest.text = m_s_questList[m_i_questCount];
        if(m_i_questCount+1 <3)
            m_i_questCount++;
    }
    #endregion

    #region _ guide's list of talk . add
    void talkList()
    {
        m_s_TalkList.Add(PlayerPrefs.GetString("Name") + "! �� �̸� �ͺ�! ū�ϳ���! ");
        m_s_TalkList.Add("���ڱ� �ΰ����� �̻��� ��踦 ���ͼ���\n�������� �� �μ��� ���� ������ ��ư���!");
        m_s_TalkList.Add("���� �ٸ� �ξ���� ���������� ����� �Ѿư� ���� ����.\n�װ� ������!");
        m_s_TalkList.Add("�Ǽ����� ������ �����ϱ�, ���Ѵ�� ����� �� ì���ڱ�!");
        m_s_TalkList.Add("�����!"); //4
        m_s_TalkList.Add("��ó�� �������ִ� �͵��� ��������.\n�����̳� ��������ⰰ���� ������ �ɰž�."); //5
        m_s_TalkList.Add("�ΰ����� �谰���� Ÿ�� ���°� �þ�.\n���踦 ã���� ������ ���� ���������ž�!");
        m_s_TalkList.Add("���̰��� �����ְ� ������ �̾���..\n�� ������!!"); //7  
    }
    #endregion
}
