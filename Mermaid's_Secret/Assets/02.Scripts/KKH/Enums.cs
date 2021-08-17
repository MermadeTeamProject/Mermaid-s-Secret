using UnityEngine;

public class Enums : MonoBehaviour
{
    private static Enums m_EN_instance = null;
    public static Enums instance
    {
        get
        {
            if (m_EN_instance == null)
            {
                return null;
            }
            return m_EN_instance;
        }
    }

    public enum Particle
    {
        ON,
        OFF,
    }

    public enum GuideCurr
    {
        STAY,
        FIRSTMOVE,
        NEXTMOVE,
        PAUSE,
    }

    public enum NextQuest
    {
        ON,
        OFF,
    }

    public enum Limit_Quest
    {
        NOTHING,
        LIMITTEXT,
        QUESTNOTI,
    }
    public Particle pc = Particle.ON;
    public GuideCurr gc = GuideCurr.STAY;
    public NextQuest nq = NextQuest.OFF;
    public Limit_Quest lq = Limit_Quest.NOTHING;
    private void Awake()
    {
        if (m_EN_instance == null)
        {
            m_EN_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
