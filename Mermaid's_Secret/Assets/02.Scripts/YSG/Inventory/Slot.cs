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

    [SerializeField] private GameObject m_G_count;  //아이템 갯수 표시하는 텍스트 (SetActive 이용을 위해 게임 오브젝트로 접근)
    [SerializeField] private Text m_T_count;    //아이템 갯수 표시하는 텍스트

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
                    SetSlotCount(-1);
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
    public void AddNewItem(Item_ _item, int _count = 1)    //아이템의 아이템 스크립트와 아이템 획득 개수(int형)을 인수로 받음
    {
        item = _item;   //인수로 들어온 아이템을 슬롯의 아이템에 조장
        i_itemCount = _count;   //슬롯의 아이템 갯수를 해당 함수의 인수로 들어온 아이템 획득 갯수(기본 1)로 설정
        I_itemImage.sprite = item.itemImage;

        if (item.itemType == Item_.ItemType.Use)   //획득한 아이템이 소비 아이템일 경우
        {
            m_G_count.SetActive(true);  //아이템 개수 표시 텍스트 활성화
            m_T_count.text = i_itemCount.ToString();    //표시할 텍스트를 현재 슬롯 내의 아이템 개수로 변경
        }
        else   //획득한 아이템이 장비 아이템이거나 기타(힌트) 아이템일 경우
        {
            m_T_count.text = "0";   //표시할 텍스트를 0으로 설정 후
            m_G_count.SetActive(false); //아이템 개수 표시 텍스트 비활성화(장비 및 기타 아이템은 여러 개 얻을 수 없으므로)
        }

        SetAlpha(1);    //슬롯의 아이템 아이콘 스프라이트 알파값을 1로 조정 (아이콘이 보이도록 조정)
    }


    //아이템 갯수 업데이트 함수
    //(아이템 획득 시 슬롯에 이미 중복된 아이템이 있거나, 슬롯에서 아이템을 사용하여 개수를 감소시킬 때 등)
    public void SetSlotCount(int _count)
    {
        i_itemCount += _count;  //슬롯의 아이템 개수를 _count만큼 증감시킨 후
        m_T_count.text = i_itemCount.ToString();    //아이템 개수 표시 텍스트를 현재 슬롯 내의 아이템 개수로 변경

        if (i_itemCount <= 0)   //슬롯 내의 아이템 개수가 0보다 적거나 같을 시
        {
            i_itemCount = 0;    //슬롯 내의 아이템 개수를 0으로 초기화
            ClearSlot();    //슬롯 내 아이템 정보 삭제
        }
    }

    //슬롯 내 아이템 정보 삭제 함수
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
