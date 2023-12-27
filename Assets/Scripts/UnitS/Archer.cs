using UnityEngine;

public class Archer : UnitBase
{
    void Awake()
    {
        UnitName = "Archer";
        UnitID = 1;
        MaxHP = 100;
        CurrentHP = MaxHP;
        MaxMP = 50;
        CurrentMP = MaxMP;
        Strength = 12;
        Dexterity = 18;
        Constitution = 10;
        Intelligence = 12;
        criticalChance = 10;
        criticalDamageMultiplier = 10;
        damageReduction = 0;
        evasion = 10;
        accuracy = 100;
        damageBonus = 0;
    }

    public override int MainAttributeValue
    {
        get { return Dexterity; }
    }
}
