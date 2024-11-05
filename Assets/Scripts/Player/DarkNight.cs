using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNight : PlayerController
{
    [SerializeField] GameObject swordObject;
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
    IEnumerator DeactiveSword()
    {
        yield return new WaitForSeconds(1);
        swordObject.SetActive(false);
    }
}
