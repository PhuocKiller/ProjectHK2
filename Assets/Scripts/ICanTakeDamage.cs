using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanTakeDamage
{
    public void ApplyDamage(float damage, PlayerRef player, Action callback = null);

}
