using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public int health { get; set; }
    public int damage { get; set; }

    public PlayerStat(int health, int damage)
    {
        this.health = health;
        this.damage = damage;
    }
}
