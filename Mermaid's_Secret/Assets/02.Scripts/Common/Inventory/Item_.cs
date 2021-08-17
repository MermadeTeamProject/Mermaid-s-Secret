using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]
public class Item_ : ScriptableObject
{
   public enum ItemType
    {
        Equip,
        Use,
        Etc,
    }

    [TextArea]//여러 줄 가능해짐
    public string itemDesc; //아이템 설명

    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public int maxCount;
}

