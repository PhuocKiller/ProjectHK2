using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillController : MonoBehaviour
{
    public SkillType type;
    public SkillSO skillStat;
    protected bool m_isTriggered, m_isCooldowning;
    protected float m_cooldownTime;
    protected float m_triggerTime;
    public UnityEvent OnTriggerEnter, OnSKillUpdate, OnCooldown, OnStop,OnCooldownStop;
    public UnityEvent<SkillType> OnStopWithType;
    

    public float cooldownProgress
    {
        get => CooldownTime / skillStat.cooldownTime;
    }
    public float triggerProgress
    {
        get => m_triggerTime / skillStat.timerTrigger;
    }
    public bool IsTriggered { get => m_isTriggered; }
    public bool IsCooldowning { get => m_isCooldowning; }
    public float CooldownTime { get => m_cooldownTime; }
    public virtual void LoadStat()
    {
        if (skillStat == null) return;
        m_cooldownTime = skillStat.cooldownTime;
        m_triggerTime = skillStat.timerTrigger;
    }
    public void Trigger()
    {
        if (m_isTriggered || m_isCooldowning) return;
        m_isCooldowning = true;
        m_isTriggered = true;
    }
}
