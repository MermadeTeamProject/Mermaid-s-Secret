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

    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;
}

