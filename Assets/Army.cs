using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Army
{
    public string name;
    public Unit[] units;

    public Army(string n, Unit[] uns)
    {
        this.name = n;
        this.units = uns;
    }

}

public struct Unit
{
    public int amount;
    public int health;
    public int attack;
    public int armor;
    public string type;

    public Unit(int amt, int hp, int atk, int arm, string t)
    {
        this.amount = amt;
        this.health = hp;
        this.attack = atk;
        this.armor = arm;
        this.type = t;
    }
}