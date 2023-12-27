using UnityEngine;

public class Wizard : UnitBase
{
    void Awake()
    {
        UnitName = "Wizard";
        UnitID = 3;
        MaxHP = 80;
        CurrentHP = MaxHP;
        MaxMP = 120;
        CurrentMP = MaxMP;
        Strength = 6;
        Dexterity = 14;
        Constitution = 10;
        Intelligence = 20;
        criticalChance = 10;
        criticalDamageMultiplier = 10;
        damageReduction = 0;
        evasion = 5;
        accuracy = 100;
        damageBonus = 0;
    }

    public override int MainAttributeValue
    {
        get { return Intelligence; }
    }
}
