using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float maxMana { get; set; }
    public float currentMana { get; set; }
    public float damage { get; set; }

    public PlayerStat(float maxHealth, float maxMana, float damage)
    {
        this.maxHealth = maxHealth;
        currentHealth=maxHealth;
        this.maxMana = maxMana;
        this.currentMana = maxMana;
        this.damage = damage;
    }
}
