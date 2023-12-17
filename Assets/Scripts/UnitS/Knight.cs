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
        Defense = 15;
        MagicDefense = 10;
        Power = 10;
        Evasion = 10;
        Accuracy = 12;
    }

    public override int BasicAttack()
    {
        BaseDamage = (int) (Strength * 0.25);
        return BaseDamage;
    }

    public override float SkillMultiplier()
    {
        BaseSkillMultiplier = 1 + (Strength / 16);
        return BaseSkillMultiplier;
    }
}
