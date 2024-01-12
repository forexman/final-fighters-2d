using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory
{
    private ICombatLogger combatLogger;

    public SkillFactory(ICombatLogger combatLogger)
    {
        this.combatLogger = combatLogger;
    }

    public Skill CreateSkill(SkillMetadata metadata)
    {
        var effects = CreateEffectsBasedOnMetadata(metadata.Effects);
        var statusEffects = CreateStatusEffectsBasedOnMetadata(metadata.StatusEffects);

        return new Skill(
            metadata.Id, metadata.Name, metadata.Description, metadata.Power, metadata.Cost, metadata.SkillTarget,
            metadata.TargetCount, metadata.Duration, effects, statusEffects, metadata.UnitID,
            metadata.CriticalChance, metadata.CriticalDamageMultiplier, metadata.SkillAccuracy
        );
    }
    private List<ISkillEffect> CreateEffectsBasedOnMetadata(List<EffectMetadata> effectMetadata)
    {
        var effects = new List<ISkillEffect>();

        foreach (var effect in effectMetadata)
        {
            switch (effect.Type)
            {
                case "Damage":
                    var damageEffect = new DamageEffect(effect.Power, effect.DamageType);
                    damageEffect.SetDependencies(combatLogger);
                    effects.Add(damageEffect);
                    break;
                case "Healing":
                    var healingEffect = new HealingEffect(effect.Power);
                    healingEffect.SetDependencies(combatLogger);
                    effects.Add(healingEffect);
                    break;
            }
        }

        return effects;
    }

    private List<StatusEffectMetadata> CreateStatusEffectsBasedOnMetadata(List<StatusEffectMetadata> statusEffectMetadata)
    {
        return statusEffectMetadata;
    }
}
