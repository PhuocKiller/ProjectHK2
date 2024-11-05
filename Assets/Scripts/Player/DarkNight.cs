using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNight : PlayerController
{
    [SerializeField] Transform lightningShieldTransform;
    [SerializeField] GameObject lightningShieldVFXPrefab;
    TickTimer timerSkill2;
    public override void NormalAttack()
    {
        base.NormalAttack();
        Runner.Spawn(basicAttackObject, basicAttackTransform.position, inputAuthority: Object.InputAuthority
     , onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
     {
         obj.GetComponent<BasicAttackObject>().SetDirection(transform.forward);
     }
                        );
    }
    public override void Skill_2()
    {
        base.Skill_2();
        NetworkObject obj=Runner.Spawn(lightningShieldVFXPrefab, lightningShieldTransform.position, Quaternion.identity, Object.InputAuthority,
            onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
            {
                obj.transform.SetParent(lightningShieldTransform);
            });
    }
}
