using UnityEngine;
using UnityEngine.UI;

//?�레?�어 ?�격???�운??모션 추�??�거�??�기 추�??�기
//?�아?�는 ?�레?�어???�치변??관???�크립트
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

    //�??�성?�때 ?�태초기??
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    //체력?�복
    public override void RecoveryHP(float healHP)
    {
        base.RecoveryHP(healHP);
    }
    
    //HP-- 처리
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
    }

    //?�망체크 &처리
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


