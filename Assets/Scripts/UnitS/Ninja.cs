using UnityEngine;

public class Ninja : UnitBase
{
    protected override void Awake()
    {
        base.Awake();
        UnitName = "Ninja";
        UnitID = 2;
        MaxHP = 120;
        CurrentHP = MaxHP;
        MaxMP = 40;
        CurrentMP = MaxMP;
        Strength = 16;
        Dexterity = 22;
        Constitution = 12;
        Intelligence = 10;
        criticalChance = 100;
        criticalDamageMultiplier = 25;
        damageReduction = 0;
        evasion = 20;
        accuracy = 100;
        damageBonus = 0;
    }

    public override int MainAttributeValue
    {
        get { return Dexterity; }
    }
}
