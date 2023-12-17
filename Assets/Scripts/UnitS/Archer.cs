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
        Defense = 12;
        MagicDefense = 10;
        Power = 16;
        Evasion = 15;
        Accuracy = 20;
    }

    public override int BasicAttack()
    {
        BaseDamage = (int) (Dexterity * 0.25);
        return BaseDamage;
    }

    public override float SkillMultiplier()
    {
        BaseSkillMultiplier = 1 + (Dexterity / 16);
        return BaseSkillMultiplier;
    }
}
