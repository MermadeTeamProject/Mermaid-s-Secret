using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPointerClickHandler
{
    public Item_ item;  //획득한 아이템
    public int i_itemCount; //획득한 아이템의 개수
    public Image I_itemImage;   //슬롯에 표시할 아이템의 이미지

    [SerializeField] private GameObject m_G_count;
    [SerializeField] private Text m_T_count;

    private ItemEffectDatabase itemEffect;


    private void Awake()
    {
        itemEffect = FindObjectOfType<ItemEffectDatabase>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                itemEffect.UseItem(item);

                if (item.itemType == Item_.ItemType.Use)
                {
                    SetSlotcount(-1);
                }
            }
        }
    }

    //슬롯에 띄울 이미지의 투명도 조절
    void SetAlpha(float alpha)
    {
        Color color = I_itemImage.color;
        color.a = alpha;
        I_itemImage.color = color;
    }

    //인벤토리에 새로운 아이템 추가
    public void AddItem(Item_ _item, int _count = 1)
    {
        item = _item;
        i_itemCount = _count;
        I_itemImage.sprite = item.itemImage;

        if (item.itemType != Item_.ItemType.Equip&&item.itemType != Item_.ItemType.Etc)
        {
            m_G_count.SetActive(true);
            m_T_count.text = i_itemCount.ToString();
        }
        else
        {
            m_T_count.text = "0";
            m_G_count.SetActive(false);
        }

        SetAlpha(1);
    }

    //슬롯의 아이템 갯수 업데이트
    public void SetSlotcount(int _count)
    {
        i_itemCount += _count;
        m_T_count.text = i_itemCount.ToString();

        if (i_itemCount <= 0)
            ClearSlot();
    }

    //아이템 개수가 0일 시 슬롯 삭제
    void ClearSlot()
    {
        item = null;
        i_itemCount = 0;
        I_itemImage.sprite = null;
        SetAlpha(0);

        m_T_count.text = "0";
        m_G_count.SetActive(false);
    }
}
