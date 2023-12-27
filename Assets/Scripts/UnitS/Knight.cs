using UnityEngine;

public class Knight : UnitBase
{
    void Awake()
    {
        UnitName = "Knight";
        UnitID = 0;
        MaxHP = 150;
        CurrentHP = MaxHP;
        MaxMP = 30;
        CurrentMP = MaxMP;
        Strength = 20;
        Dexterity = 10;
        Constitution = 18;
        Intelligence = 8;
        criticalChance = 10;
        criticalDamageMultiplier = 10;
        damageReduction = 20;
        evasion = 0;
        accuracy = 100;
        damageBonus = 0;
    }

    public override int MainAttributeValue
    {
        get { return Strength; }
    }
}
