using UnityEngine;

public class Ninja : UnitBase
{
    void Awake()
    {
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
        Defense = 10;
        MagicDefense = 10;
        Power = 20;
        Evasion = 18;
        Accuracy = 20;
    }

    public override int BasicAttack()
    {
        Debug.Log(Dexterity * 0.25);
        BaseDamage = (int) (Dexterity * 0.25);
                Debug.Log(BaseDamage);
        return BaseDamage;
    }

    public override float SkillMultiplier()
    {
        BaseSkillMultiplier = 1 + (Dexterity / 16);
        return BaseSkillMultiplier;
    }
}
