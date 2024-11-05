using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNight : PlayerController
{
    TickTimer timerSkill2;
    public override void NormalAttack(GameObject VFXEffect)
    {
        base.NormalAttack(VFXEffect);
        Runner.Spawn(VFXEffect, normalAttackTransform.position, Quaternion.identity, inputAuthority: Object.InputAuthority
     , onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
     {
         obj.gameObject.GetComponent<BasicAttackObject>().timer = TickTimer.CreateFromSeconds(Runner, 100f);
         obj.transform.SetParent(normalAttackTransform);
     }
                        );
    }
    public override void Skill_2(GameObject VFXEffect)
    {
        base.Skill_2(VFXEffect);
        NetworkObject obj=Runner.Spawn(VFXEffect, skill_2Transform.position, Quaternion.identity, Object.InputAuthority,
            onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
            {
                obj.transform.SetParent(skill_2Transform);
            });
    }
}
