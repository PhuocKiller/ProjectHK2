using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillSystem/Create Skill Data")]
public class SkillSO : ScriptableObject
{
    public float timerTrigger;
    public float cooldownTime;
    public Sprite skillIcon;
    public AudioClip triggerSoundFX;
    public float[] levelDamages;
    public bool isPhysicDamage;
    public bool isMakeStun;
    public bool isMakeSlow;
    public bool isMakeSilen;
    public float timeEffect;
    public GameObject VfxEffect;
}

