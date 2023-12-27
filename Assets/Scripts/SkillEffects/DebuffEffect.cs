using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffEffect : ISkillEffect
{
    private string debuffType;
    private int amount;
    private int duration;

    public DebuffEffect(string debuffType, int amount, int duration)
    {
        this.debuffType = debuffType;
        this.amount = amount;
        this.duration = duration;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            target.ApplyStatusEffect(new DebuffStatus(debuffType, amount, duration));
        }
    }
}
