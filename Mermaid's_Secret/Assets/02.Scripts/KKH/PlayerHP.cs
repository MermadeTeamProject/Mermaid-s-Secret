using UnityEngine;
using UnityEngine.UI;

//?Œë ˆ?´ì–´ ?¼ê²©???¬ìš´??ëª¨ì…˜ ì¶”ê?? ê±°ë©??¬ê¸° ì¶”ê??˜ê¸°
//?´ì•„?ˆëŠ” ?Œë ˆ?´ì–´???˜ì¹˜ë³€??ê´€???¤í¬ë¦½íŠ¸
public class PlayerHP : WhenAlive
{
    [Header("Player HP Slider")]
    public Slider S_hpSlider;
    private Rigidbody m_R_rb;

    private Animator m_A_anim;

    private void Awake()
    {
        m_R_rb = this.GetComponent<Rigidbody>();
        m_A_anim = this.GetComponent<Animator>();
    }

    //ì²??œì„±?”ë•Œ ?íƒœì´ˆê¸°??
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    //ì²´ë ¥?Œë³µ
    public override void RecoveryHP(float healHP)
    {
        base.RecoveryHP(healHP);
    }
    
    //HP-- ì²˜ë¦¬
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
    }

    //?¬ë§ì²´í¬ &ì²˜ë¦¬
    public override void WhenDie()
    {
        base.WhenDie();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            f_currHP -= 20f;
        }
    }


}


