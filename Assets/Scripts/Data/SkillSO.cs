using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SkillSystem/Create Skill Data")]
public class SkillSO : ScriptableObject
{
    public float timerTrigger;
    public float cooldownTime;
    public Sprite skillIcon;
    public AudioClip triggerSoundFX;

}
