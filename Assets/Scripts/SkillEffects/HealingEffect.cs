using System.Collections.Generic;

public class HealingEffect : ISkillEffect
{
    private int healingAmount;

    public HealingEffect(int healingAmount)
    {
        this.healingAmount = healingAmount;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            target.Heal(healingAmount);
        }
    }
}