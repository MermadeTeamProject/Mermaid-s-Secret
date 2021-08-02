using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject m_G_slotsGroup; //슬롯들을 묶고 있는 부모 오브젝트

    private Slot[] m_S_slots;

    void Start()
    {
        m_S_slots = m_G_slotsGroup.GetComponentsInChildren<Slot>();
    }

    //아이템 획득 함수
   public void getItem(Item_ _item, int _count = 1)
    {
        if (Item_.ItemType.Equip != _item.itemType&& Item_.ItemType.Etc != _item.itemType)
        {
            for(int a=0; a<m_S_slots.Length; a++)
            {
                if (m_S_slots[a].item != null)
                {
                    if(m_S_slots[a].item.itemName == _item.itemName)
                    {
                        m_S_slots[a].SetSlotcount(_count);
                        return;
                    }
                }
            }
        }

        for(int a=0; a < m_S_slots.Length; a++)
        {
            if(m_S_slots[a].item == null)
            {
                m_S_slots[a].AddItem(_item, _count);
                return;
            }
        }
    }
}
