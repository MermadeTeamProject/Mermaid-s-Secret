using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템 이름(키값으로 사용)
    [Tooltip("HP,HUNGRY만 가능")]
    public string part;   //아이템 효과가 작용할 스탯
    public int value;   //아이템 효과 수치
}


public class ItemEffectDatabase : MonoBehaviour
{
    PlayerCtrl player;


    [SerializeField] private ItemEffect[] itemEffects;

    private const string HP = "HP", HUNGRY = "HUNGRY";

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    public void UseItem(Item_ _item)
    {
        if(_item.itemType == Item_.ItemType.Use)
        {
            for (int a=0; a < itemEffects.Length; a++)
            {
                if (itemEffects[a].itemName == _item.itemName)
                {
                    switch (itemEffects[a].part)
                    {
                        case HP:
                            player.healHP(itemEffects[a].value);
                            break;

                        case HUNGRY:
                            player.healSatiety(itemEffects[a].value);
                            break;

                        default:
                            print("잘못된 스탯입니다. HP,HUNGRY 만 가능합니다.");
                            break;
                    }
                    print(_item.itemName + "을 사용했습니다.");
                }
            }
            return;
        }
        
    }

   

}
