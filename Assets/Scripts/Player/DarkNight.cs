using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNight : PlayerController
{
    [SerializeField] public Transform normalAttackTransform, skill_1Transform, skill_2Transform, ultimateTransform;
    TickTimer timerSkill2;
    public override void NormalAttack(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        base.NormalAttack(VFXEffect, levelDamage, isPhysicDamage);
        StartCoroutine(DelaySpawnAttack(VFXEffect, levelDamage, isPhysicDamage));
    }
    IEnumerator DelaySpawnAttack(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false)
    {
        yield return new WaitForSeconds(0.5f);
        Runner.Spawn(VFXEffect, normalAttackTransform.transform.position, normalAttackTransform.rotation, inputAuthority: Object.InputAuthority
     , onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
     {

         obj.gameObject.GetComponent<DarkNight_Attack>().damage = levelDamage;
     }
                        );
    }
    public override void Skill_2(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        base.Skill_2(VFXEffect, levelDamage, isPhysicDamage,timeTrigger: timeTrigger);
        NetworkObject obj = Runner.Spawn(VFXEffect, skill_2Transform.position, Quaternion.identity, Object.InputAuthority,
            onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
            {
                obj.transform.SetParent(skill_2Transform);
            });
    }
}

