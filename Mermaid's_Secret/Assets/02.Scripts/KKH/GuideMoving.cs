using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GuideMoving : MonoBehaviour
{
    #region # variables
    public float speed = 5f;
    private Rigidbody m_R_rb;

    //find near Item and move
    private Vector3 m_V_arrvalPosi;
    //short distance
    public float shortDis;
    public float _shortDis;
    //near item position
    private GameObject nearBottle;
    public GameObject nearBranch;

    //anim
    private Animator anim;
    #endregion

    public float dis;
    private void Start()
    {
        init();
        FindNearItem();
    }
    private void Update()
    {
        IsArrival();
        if (Enums.instance.gc != Enums.GuideCurr.STAY && Enums.instance.gc != Enums.GuideCurr.PAUSE)
        {
            Enums.instance.pc = Enums.Particle.ON;
            Move();
            m_R_rb.velocity = transform.forward * speed;
        }
    }

    private void init()
    {
        m_R_rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    #region _ find nearest bottle cuz for going there
    private void FindNearItem()
    {
        if (ItemManager.WasCreat == true)
        {
            var shortDis = Vector3.Distance(this.transform.position, ItemManager.G_nearBottleArr[0].transform.position);
            foreach (GameObject bottle in ItemManager.G_nearBottleArr)
            {
                float dis = Vector3.Distance(this.transform.position, bottle.transform.position);
                if (dis < shortDis)
                {
                    nearBottle = bottle;
                    shortDis = dis;
                }
            }
        }
    }
    #endregion

    #region _ Move
    private void Move()
    {       
        if (Enums.instance.gc == Enums.GuideCurr.STAY || Enums.instance.gc == Enums.GuideCurr.PAUSE)
            anim.SetBool("Move", false);
        else
            anim.SetBool("Move", true);
        if (Enums.instance.gc == Enums.GuideCurr.FIRSTMOVE)
        {
            if (nearBottle != null)
                m_V_arrvalPosi = nearBottle.transform.position;
        }
        else
        {
            if(nearBranch != null)
                m_V_arrvalPosi = nearBranch.transform.position;
        }

        var posi = m_V_arrvalPosi;
        posi.y += 1.5f;

        this.transform.position = Vector3.MoveTowards(transform.position, posi, .02f);
    }
    #endregion
    private void IsArrival()
    {
        dis = Vector3.Distance(this.transform.position, m_V_arrvalPosi);
        if (dis < 2.5f)
        {
            Enums.instance.gc = Enums.GuideCurr.PAUSE;       
        }
    }
}
