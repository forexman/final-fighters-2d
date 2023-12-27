using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : ISkillEffect
{
    private string buffType;
    private int amount;
    private int duration;

    public BuffEffect(string buffType, int amount, int duration)
    {
        this.buffType = buffType;
        this.amount = amount;
        this.duration = duration;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        Debug.Log("called");
        foreach (var target in targets)
        {
            target.ApplyStatusEffect(new BuffStatus(buffType, amount, duration));
        }
    }
}