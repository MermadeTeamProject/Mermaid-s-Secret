using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //아이템 매니저 싱글톤
    private static ItemManager m_I_instance = null;
    public static ItemManager instance
    {
        get
        {
            if (m_I_instance == null)
            {
                m_I_instance = new ItemManager();
                //DontDestroyOnLoad(); //나중에 생길, 유지해야 할 오브젝트 삽입
            }
            return m_I_instance;
        }
    }

    public enum Scene   //각 함수 및 변수를 씬별로 호출할 수 있도록 하는 함수(State와 유사) - YSG
    {
        UnderSea = 0,
        Island,
        Lab,
    }

    public Scene scene;    //enum함수에 접근하는 겸 현재 씬을 표시하는 변수 - YSG

    [Header("수중도시 맵 관련")]

    //아이템 관련
    [SerializeField]
    private int m_i_branch = 20; //생성할 나무가지 개수
    [SerializeField]
    private int m_i_bottle = 20; //생성할 물병 개수
                              
    public GameObject[] m_G_branches; //나무가지 게임오브젝트 배열
    public GameObject m_G_bottle;//물병프리팹
    [SerializeField]
    private GameObject m_G_itemParents;//아이템 프리팹이 생성될 부모 폴더


    //기능별 아이템 클래스별 정리
    public List<Branch> branch_List = new List<Branch>();
    public List<Water> water_List = new List<Water>();

    //인벤토리 보관
    public List<GameObject> inventory_List = new List<GameObject>();


    [Header("섬 맵 관련")]
    [SerializeField] GameObject T_shellSpawnPoints;    //조개가 생성 포인트 폴더
    [SerializeField] GameObject G_shell; //생성할 조개 프리팹
    [SerializeField] private int m_i_maxShell; //조개 최대 생성 갯수
    private List<GameObject> L_shellPool = new List<GameObject>();  //조개 오브젝트 풀 저장할 리스트
    


    private void Start()
    {
        Init();
    }

    //Start에서 실행될 매니저 함수 묶음
    public void Init()
    {
        switch (scene)
        {
            case Scene.UnderSea: //수중도시 씬
                makinBranch();
                makinBottle();
                break;

            case Scene.Island: //섬 씬
                spawnShell();
                break;
        }
        
    }

    /*수중도시 맵 내 함수 --------------------------------------------------------------------------------------------주석 단 사람: YSG*/

    //나무가지 랜덤 위치 생성
    //계속 생기는 오브젝트는 아니니까 굳이 풀링을 쓸필요는 없을것 같다
    void makinBranch()
    {
        GameObject tmp = m_G_branches[0];
        for (int i = 0; i < m_i_branch; i++)
        {
            int rand = Random.Range(0, 2);
            float ranX = Random.Range(50f, 120f);
            float ranZ = Random.Range(30f, 110f);
            m_G_branches[rand].transform.position = new Vector3(ranX, 1f, ranZ);
            GameObject b = Instantiate(m_G_branches[rand]);
            b.transform.parent = m_G_itemParents.transform;
            //branch_List.Add(b);
            //branch_List[i].
        }
    }

    //병 위치 랜덤생성
    void makinBottle()
    {
        for (int i = 0; i < m_i_bottle; i++)
        {
            float ranX = Random.Range(50f, 120f);
            float ranZ = Random.Range(30f, 100f);
            m_G_bottle.transform.position = new Vector3(ranX, .8f, ranZ);
            GameObject b = Instantiate(m_G_bottle);
            b.transform.parent = m_G_itemParents.transform;

        }
    }


    /*섬 맵 내 함수 --------------------------------------------------------------------------------------------주석 단 사람: YSG */

    void spawnShell()   //Start 시 조개 랜덤 위치 생성
    {
        for (int a = 0; a < m_i_maxShell+1;) 
        {
            Transform[] spawnPos = T_shellSpawnPoints.GetComponentsInChildren<Transform>();
            int spawnIndex = Random.Range(1, spawnPos.Length);
            if (spawnPos[spawnIndex].gameObject.tag != "Item"&& spawnPos[spawnIndex].childCount==0)
            {
                float rotate = Random.Range(0, 350);
                var shell = Instantiate(G_shell, spawnPos[spawnIndex].position, Quaternion.Euler(0, rotate,0), spawnPos[spawnIndex]);
                shell.name = "Shell_" + a.ToString("00");
                L_shellPool.Add(shell);
                a++;
            }
        }
    }

}
