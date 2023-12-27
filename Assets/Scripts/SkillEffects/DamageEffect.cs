using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageEffect : ISkillEffect
{
    private int power;
    private DamageType damageType;

    public DamageEffect(int power, DamageType type)
    {
        this.power = power;
        this.damageType = type;
    }

    public void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(CalculateDamage(source, target, skill), damageType);
        }
    }

    private int CalculateDamage(UnitBase attacker, UnitBase defender, Skill skill)
    {
        int baseDamage = power;
        baseDamage += (int)(baseDamage * attacker.damageBonus / 100);
        baseDamage += (int)(baseDamage * attacker.MainAttributeValue / 100);
        int finalDamage = baseDamage - (int)(baseDamage * defender.damageReduction / 100);

        if (IsCriticalHit(attacker, skill))
        {
            finalDamage += (int)(finalDamage * (attacker.criticalDamageMultiplier + skill.CriticalDamageMultiplier) / 100);
            ServiceLocator.Instance.GetService<ICombatLogger>().AddEventToCombatLog($"CRITICAL HIT! {attacker.UnitName} uses {skill.SkillName} and deals {Mathf.Max(0, finalDamage)} {damageType} damage to {defender.UnitName}.");
        }
        else
        {
            ServiceLocator.Instance.GetService<ICombatLogger>().AddEventToCombatLog($"{attacker.UnitName} uses {skill.SkillName} and deals {Mathf.Max(0, finalDamage)} {damageType} damage to {defender.UnitName}.");
        }

        return Mathf.Max(0, finalDamage);
    }

    public bool IsCriticalHit(UnitBase attacker, Skill skill)
    {
        return UnityEngine.Random.Range(0f, 100f) <= attacker.criticalChance + skill.CriticalChance;
    }
}