﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Image m_skillIcon;
    [SerializeField] Image m_CooldownOverlay;
    [SerializeField] Image m_timeTriggerFilled;

    [SerializeField] Text m_amountTxt;
    [SerializeField] Text m_cooldownTxt;
    [SerializeField] Button m_btnComp;

    SkillType m_skillType;
    SkillController m_skillController;
    int m_currentAmount;
    #region EVENTS
    void RegisterEvent()
    {
        if (m_skillController == null) return;
        m_skillController.OnCooldown.AddListener(UpdateCooldown);
        m_skillController.OnSkillUpdate.AddListener(UpdateTimerTrigger);
        m_skillController.OnCooldownStop.AddListener(UpdateUI);
    }
    void UnRegisterEvent()
    {
        if (m_skillController == null) return;
        m_skillController.OnCooldown.RemoveListener(UpdateCooldown);
        m_skillController.OnSkillUpdate.RemoveListener(UpdateTimerTrigger);
        m_skillController.OnCooldownStop.RemoveListener(UpdateUI);
    }
    #endregion
    public void Initialize(SkillType skillType)
    {
        m_skillType= skillType;
        m_skillController=FindObjectOfType<SkillManager>().GetSkillController(skillType);
        m_timeTriggerFilled.transform.parent.gameObject.SetActive(false);
        UpdateUI();
        if (m_btnComp != null)
        {
            m_btnComp.onClick.RemoveAllListeners();
            m_btnComp.onClick.AddListener(TriggerSkill);
        }
        RegisterEvent();
    }

    private void UpdateUI()
    {
        if (m_skillController == null) return;
        if (m_skillIcon)
        m_skillIcon.sprite=m_skillController.skillStat.skillIcon;
        UpdateAmountTxt();
        UpdateCooldown();
        UpdateTimerTrigger();
        bool canActiveMe = m_currentAmount > 0 || m_skillController.IsCooldowning;
        gameObject.SetActive(canActiveMe);
    }

    private void UpdateTimerTrigger()
    {
        if (m_skillController==null ||m_timeTriggerFilled==null) return;
        float triggerProgress=m_skillController.triggerProgress;
        m_timeTriggerFilled.fillAmount= triggerProgress;
        m_timeTriggerFilled.transform.parent.gameObject.SetActive(m_skillController.IsTriggered);
    }

    private void UpdateCooldown()
    {
        if (m_cooldownTxt)
        m_cooldownTxt.text= m_skillController.CooldownTime.ToString("f1");
        float cooldownProgress= m_skillController.cooldownProgress;
        if (m_CooldownOverlay)
        {
            m_CooldownOverlay.fillAmount= cooldownProgress;
            m_CooldownOverlay.gameObject.SetActive(m_skillController.IsCooldowning);
        }
    }

    private void UpdateAmountTxt()
    {
        m_currentAmount= FindObjectOfType<SkillManager>().GetSkillAmount(m_skillType);
        if (m_amountTxt)
        {
            m_amountTxt.text = $"x {m_currentAmount}";
        }
    }

    void TriggerSkill()
    {
        if (m_skillController==null) return;
        m_skillController.Trigger();
        //play âm thanh ở đây
    }
    private void OnDestroy()
    {
        UnRegisterEvent();
    }
    
}