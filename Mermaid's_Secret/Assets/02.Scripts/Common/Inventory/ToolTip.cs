using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;//툴팁 베이스

    [SerializeField]
    private Text txt_ItemName;//툴팁 아이템의 이름
    [SerializeField]
    private Text txt_ItemDesc;//툴팁 아이템의 설명
    
    
    

    public void ShowToolTip(Item_ _item, Vector3 _Pos)
    {
        go_Base.SetActive(true);//툴팁을 활성화
        go_Base.transform.position = _Pos;//툴팁 위치 조정

        txt_ItemName.text = "<" + _item.itemName + ">";// <아이템 이름>
        txt_ItemDesc.text = _item.itemDesc;//아이템 설명


       
    }   

    public void HideToolTip()
    {
        go_Base.SetActive(false);//툴팁 비활성화
    }
}
