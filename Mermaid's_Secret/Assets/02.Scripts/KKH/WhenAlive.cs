using System;
using UnityEngine;

//?�구???�아?�다�? ?�레?�어?� ?�이 공통?�로 가지???�치?�입?�다.
public class WhenAlive : MonoBehaviour, IDamageInterface
{
    public float f_startingHP = 100f; //?�작체력
    public float f_attackPower { get; protected set; } //공격??
    public float f_currHP { get; protected set; } //?�재체력
    public bool b_isDead { get; protected set; } //죽었?��??
    public event Action E_ifDead; //?�망??발동???�벤??

    //?�명체�? ?�성???�었?�때 ?�태 초기???�팅
    protected virtual void OnEnable()
    {
        b_isDead = false;
        f_currHP = f_startingHP;
    }

    //?�격??HP--
    public virtual void OnDamage(float damage)
    {
        f_currHP -= damage;
        if (f_currHP <= 0 && !b_isDead)
            WhenDie();
    }

    //체력 ?�복 HP++
    public virtual void RecoveryHP(float healHP)
    {
        if (b_isDead)
            return;
        f_currHP += healHP;
    }

    //?�망?��? 체크 & 체크 ???�펙???�전????
    public virtual void WhenDie()
    {
        if (E_ifDead != null) //?�망 ???�전?�이??�??�거?�으�??�행
            E_ifDead();
        b_isDead = true;
    }
    
}
