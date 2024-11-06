using Cinemachine;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillButton : NetworkBehaviour
{
    [SerializeField] Image m_skillIcon;
    [SerializeField] Image m_CooldownOverlay;
    [SerializeField] Image m_timeTriggerFilled;

    [SerializeField] Text m_amountTxt;
    [SerializeField] Text m_cooldownTxt;
    [SerializeField] Button m_btnComp;


    SkillController m_skillController;
    int m_currentAmount;
    public SkillButtonTypes[] m_skillButtonTypes;
    public SkillButtonTypes skillButtonType;
    public SkillTypes skillType;
    public GameObject VfxEffect;
    [SerializeField] SkillName m_skillName;
    public Action Skill_Trigger;
    [SerializeField] float timerTrigger;
    [SerializeField] AudioClip triggerSoundFX;
    [SerializeField] float[] levelDamages;
    [SerializeField] bool isPhysicDamage;
    [SerializeField] bool isMakeStun;
    [SerializeField] bool isMakeSlow;
    [SerializeField] bool isMakeSilen;
    [SerializeField] float timeEffect;
    [SerializeField] int levelSkill = 1;
    [SerializeField] float damageSkill;
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
    public void Initialize(SkillName skillName)
    {
        m_skillName = skillName;
        m_skillController = FindObjectOfType<SkillManager>().GetSkillController(skillName);
        skillType = m_skillController.skillType;
        VfxEffect = m_skillController.skillStat.VfxEffect;
        levelDamages = new float[6];
        for (int i = 0; i < m_skillController.skillStat.levelDamages.Length; i++)
        {
            levelDamages[i] = m_skillController.skillStat.levelDamages[i];
        }
        isPhysicDamage = m_skillController.skillStat.isPhysicDamage;
        isMakeStun = m_skillController.skillStat.isMakeStun;
        isMakeSlow = m_skillController.skillStat.isMakeSlow;
        isMakeSilen = m_skillController.skillStat.isMakeSilen;
        timeEffect = m_skillController.skillStat.timeEffect;
        timerTrigger= m_skillController.skillStat.timerTrigger;
        triggerSoundFX = m_skillController.skillStat.triggerSoundFX;
        if (levelDamages.Length > 0)
        {
            damageSkill = levelDamages[levelSkill];
        };
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
            m_skillIcon.sprite = m_skillController.skillStat.skillIcon;
        UpdateAmountTxt();
        UpdateCooldown();
        //UpdateTimerTrigger();
        bool canActiveMe = m_currentAmount > 0 || m_skillController.IsCooldowning;
        gameObject.SetActive(canActiveMe);
    }

    private void UpdateTimerTrigger()
    {
        if (m_skillController == null || m_timeTriggerFilled == null) return;
        float triggerProgress = m_skillController.triggerProgress;
        m_timeTriggerFilled.fillAmount = triggerProgress;
        m_timeTriggerFilled.transform.parent.gameObject.SetActive(m_skillController.IsTriggered);
    }

    private void UpdateCooldown()
    {
        if (m_cooldownTxt)
            m_cooldownTxt.text = m_skillController.CooldownTime.ToString(m_skillController.CooldownTime >= 1 ? "f0" : "f1");
        float cooldownProgress = m_skillController.cooldownProgress;
        if (m_CooldownOverlay)
        {
            m_CooldownOverlay.fillAmount = cooldownProgress;
            m_CooldownOverlay.gameObject.SetActive(m_skillController.IsCooldowning);
        }
    }

    private void UpdateAmountTxt()
    {
        m_currentAmount = FindObjectOfType<SkillManager>().GetSkillAmount(m_skillName);
        if (m_amountTxt)
        {
            m_amountTxt.text = $"x {m_currentAmount}";
        }
    }

    void TriggerSkill()
    {
        if (m_skillController == null || m_skillController.IsCooldowning) return;
        Singleton<PlayerManager>.Instance.CheckPlayer(out int? state, out PlayerController player);
        if (state == 0)
        {
            if (skillButtonType == SkillButtonTypes.Jump)
            {
                player.Jump(VfxEffect);
            }
            if (skillButtonType == SkillButtonTypes.NormalAttack)
            {
                player.NormalAttack(VfxEffect, damageSkill, isPhysicDamage, timeTrigger: timerTrigger);
            }
            if (skillButtonType == SkillButtonTypes.Ultimate)
            {
                player.Ultimate(VfxEffect, damageSkill, isPhysicDamage, timeTrigger: timerTrigger);
            }
            if (skillButtonType == SkillButtonTypes.Skill_2)
            {
                player.Skill_2(VfxEffect, damageSkill, isPhysicDamage, timeTrigger: timerTrigger);
            }
            if (skillButtonType == SkillButtonTypes.Skill_1)
            {
                player.Skill_1(VfxEffect, damageSkill, isPhysicDamage, timeTrigger: timerTrigger);
            }
            m_skillController.Trigger();
        }



        //play âm thanh ở đây
    }
    private void OnDestroy()
    {
        UnRegisterEvent();
    }
    public void PointerDown() //khóa camera khi giữ chuột trái tại skill
    {
        Singleton<PlayerManager>.Instance.CheckPlayer(out int? state, out PlayerController player);
        //FindObjectOfType<CinemachineFreeLook>().enabled = false;

        if (skillType == SkillTypes.Direction_Active)
        {
            if (state != 0 || m_skillController.IsCooldowning) return;
            player.state = 5;
            player.gameObject.GetComponent<SkillDirection>().GetMouseDown();
            return;
        }

    }
    public void PointDrag()
    {
        Singleton<PlayerManager>.Instance.CheckPlayer(out int? state, out PlayerController player);
        if (state == 5)
        {
            player.gameObject.GetComponent<SkillDirection>().GetMouse();
            //  Camera.main.transform.rotation= Quaternion.AngleAxis(player.transform.rotation.eulerAngles.y,Vector3.up);

        }

    }
    public void PointerUp() //mở camera khi nhả chuột trái
    {
        Singleton<PlayerManager>.Instance.CheckPlayer(out int? state, out PlayerController player);
        StartCoroutine(DelayCameraActiveAgain(0.5f));

        if (skillType == SkillTypes.Direction_Active)
        {
            if (state == 5)
            {
                player.gameObject.GetComponent<SkillDirection>().GetMouseUp();
                player.state = 0;
               m_btnComp.onClick.Invoke();
              //  Camera.main.transform.rotation = Quaternion.LookRotation(player.gameObject.GetComponent<SkillDirection>().directionNormalize);*/
            }
        }

    }
    IEnumerator DelayCameraActiveAgain(float time)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<CinemachineFreeLook>().enabled = true;
        Singleton<CameraController>.Instance.StartTransition();
    }


}

