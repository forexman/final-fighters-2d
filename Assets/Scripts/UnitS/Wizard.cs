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
        Defense = 8;
        MagicDefense = 15;
        Power = 18;
        Evasion = 12;
        Accuracy = 15;
        // isPlayerUnit = true; // Set based on unit type
    }

    public override int BasicAttack()
    {
        BaseDamage = (int) (Intelligence * 0.25);
        return BaseDamage;
    }

    public override float SkillMultiplier()
    {
        BaseSkillMultiplier = 1 + (Intelligence / 16);
        return BaseSkillMultiplier;
    }
}
