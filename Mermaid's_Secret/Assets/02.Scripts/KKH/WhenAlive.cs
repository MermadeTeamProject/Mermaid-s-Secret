using System;
using UnityEngine;

//?„êµ¬???´ì•„?ˆë‹¤ë©? ?Œë ˆ?´ì–´?€ ?ì´ ê³µí†µ?¼ë¡œ ê°€ì§€???˜ì¹˜?¤ì…?ˆë‹¤.
public class WhenAlive : MonoBehaviour, IDamageInterface
{
    public float f_startingHP = 100f; //?œì‘ì²´ë ¥
    public float f_attackPower { get; protected set; } //ê³µê²©??
    public float f_currHP { get; protected set; } //?„ì¬ì²´ë ¥
    public bool b_isDead { get; protected set; } //ì£½ì—ˆ?”ê??
    public event Action E_ifDead; //?¬ë§??ë°œë™???´ë²¤??

    //?ëª…ì²´ê? ?œì„±???˜ì—ˆ?„ë•Œ ?íƒœ ì´ˆê¸°???¸íŒ…
    protected virtual void OnEnable()
    {
        b_isDead = false;
        f_currHP = f_startingHP;
    }

    //?¼ê²©??HP--
    public virtual void OnDamage(float damage)
    {
        f_currHP -= damage;
        if (f_currHP <= 0 && !b_isDead)
            WhenDie();
    }

    //ì²´ë ¥ ?Œë³µ HP++
    public virtual void RecoveryHP(float healHP)
    {
        if (b_isDead)
            return;
        f_currHP += healHP;
    }

    //?¬ë§?¬ë? ì²´í¬ & ì²´í¬ ???´í™???¬ì „????
    public virtual void WhenDie()
    {
        if (E_ifDead != null) //?¬ë§ ???¬ì „?˜ì´??ë­?? ê±°?ˆìœ¼ë©??¤í–‰
            E_ifDead();
        b_isDead = true;
    }
    
}
