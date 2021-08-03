using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject m_G_slotsGroup; //슬롯들을 묶고 있는 부모 오브젝트
    [SerializeField] private GameObject m_T_warning;

    private Slot[] m_S_slots;
    public bool m_b_canGet = true;

    void Start()
    {
        m_S_slots = m_G_slotsGroup.GetComponentsInChildren<Slot>();
    }

    //아이템 획득 함수
   public IEnumerator getItem(Item_ _item, int _count = 1)
    {
        m_b_canGet = true;
        if (_item.itemType == Item_.ItemType.Use) //획득한 아이템이 소비 아이템인 경우
        {
            for (int a = 0; a < m_S_slots.Length; a++)  //모든 슬롯을 체크할 동안 for문을 돌리고
            {
                if (m_S_slots[a].item != null && m_S_slots[a].item.itemName == _item.itemName)  //아이템이 들어있는 슬롯 중 획득한 아이템과 같은 아이템을 가진 슬롯이 있다면
                {
                    if (m_S_slots[a].i_itemCount < _item.maxCount)  //슬롯에 들어있는 아이템의 갯수가 최대 소지 가능 갯수보다 적을 경우
                    {
                        m_S_slots[a].SetSlotCount(_count);  //슬롯의 아이템 갯수를 1 증가시킨다
                        yield break; // 그 후 함수 종료
                    }
                    else  //혹은, 슬롯에 들어있는 아이템의 갯수가 최대 소지 가능 갯수를 달성했을 경우
                    {
                        m_b_canGet = false; //아이템 획득 금지
                        m_T_warning.GetComponent<Text>().text = _item.itemName + "은/는 " + _item.maxCount + "개 이상 소지할 수 없습니다.";
                        m_T_warning.SetActive(true);   //경고 문구를 표시함
                        yield return new WaitForSeconds(2f);    //2초 후
                        m_T_warning.SetActive(false);    //경고 문구 비활성화
                        yield break;
                    }
                }
            }
        }
        for (int a = 0; a < m_S_slots.Length&&m_b_canGet; a++)  //획득한 아이템이 소비 아이템이 아니거나, 소비 아이템이지만 슬롯에 들어있지 않을 경우 모든 슬롯을 체크할 동안 for문 작동
        {
            if (m_S_slots[a].item == null)  //슬롯 중 비어있는 슬롯을 찾은 후
            {
                m_S_slots[a].AddNewItem(_item, _count);    //슬롯에 새로운 아이템 추가
                yield break; //함수 종료
            }
        }

    }
}
