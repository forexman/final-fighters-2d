using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedEffect : ISkillEffect
{
    private List<ISkillEffect> effects;

    public CombinedEffect(params ISkillEffect[] effects)
    {
        this.effects = new List<ISkillEffect>(effects);
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var effect in effects)
        {
            effect.ApplyEffect(source, targets, skill);
        }
    }
}