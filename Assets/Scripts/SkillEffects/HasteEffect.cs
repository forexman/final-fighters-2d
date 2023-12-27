using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteEffect : ISkillEffect
{
    private int duration;

    public HasteEffect(int duration)
    {
        this.duration = duration;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            target.ApplyHaste(duration);
        }
    }
}
