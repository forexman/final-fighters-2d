using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeEffect : ISkillEffect
{
    private int power;
    private int duration;
    private ICombatLogger combatLogger;

    public void SetDependencies(ICombatLogger combatLogger)
    {
        this.combatLogger = combatLogger;
    }
    
    public DamageOverTimeEffect(int power, int duration)
    {
        this.power = power;
        this.duration = duration;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            target.ApplyStatusEffect(new DamageOverTimeStatus(power, duration));
        }
    }
}
