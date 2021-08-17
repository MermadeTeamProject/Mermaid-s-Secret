using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem :  MonoBehaviour , IItem
{
    private string m_s_name = "HPItem";
    private string m_s_info = "This is Heal Item";
    private float m_i_addHP = 50f;

    public void Use(GameObject target)
    {
        WhenAlive life = target.GetComponent<WhenAlive>();
        if(life !=null)
        {
            life.RecoveryHP(m_i_addHP);
        }
        //if used Item, u need to destroy self;
    }
}
