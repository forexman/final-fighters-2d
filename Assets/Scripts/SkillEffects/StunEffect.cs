using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : ISkillEffect
{
    private int duration;
    private ICombatLogger combatLogger;

    public void SetDependencies(ICombatLogger combatLogger)
    {
        this.combatLogger = combatLogger;
    }
    
    public StunEffect(int duration)
    {
        this.duration = duration;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            // Apply a new instance of StunStatus to each target
            target.ApplyStatusEffect(new StunStatus(duration));
        }
    }
}
