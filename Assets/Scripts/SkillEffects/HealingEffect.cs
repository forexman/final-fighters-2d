using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : ISkillEffect
{
    private int healingAmount;
    private ICombatLogger combatLogger;

    public void SetDependencies(ICombatLogger combatLogger)
    {
        this.combatLogger = combatLogger;
    }
    
    public HealingEffect(int healingAmount)
    {
        this.healingAmount = healingAmount;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            int baseHealing = healingAmount;
            baseHealing += (int)(baseHealing * source.damageBonus / 100);
            baseHealing += (int)(baseHealing * source.MainAttributeValue / 100);
            int effectiveHealing = Mathf.Min(baseHealing, target.MaxHP - target.CurrentHP);
            target.Heal(effectiveHealing, this);
            combatLogger.AddEventToCombatLog($"{source.UnitName} uses {skill.SkillName} and restores {effectiveHealing} hitpoints to {target.UnitName}.");
        }
    }
}
