using System;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactory
{
    public static Skill CreateSkill(SkillMetadata metadata)
    {
        var effects = CreateEffectsBasedOnMetadata(metadata.Effects);
        var statusEffects = CreateStatusEffectsBasedOnMetadata(metadata.StatusEffects);

        return new Skill(
            metadata.Id, metadata.Name, metadata.Description, metadata.Power, metadata.Cost, metadata.SkillTarget,
            metadata.TargetCount, metadata.Duration, effects, statusEffects, metadata.UnitID,
            metadata.CriticalChance, metadata.CriticalDamageMultiplier, metadata.SkillAccuracy
        );
    }
    private static List<ISkillEffect> CreateEffectsBasedOnMetadata(List<EffectMetadata> effectMetadata)
    {
        var effects = new List<ISkillEffect>();

        foreach (var effect in effectMetadata)
        {
            switch (effect.Type)
            {
                case "Damage":
                    effects.Add(new DamageEffect(effect.Power, effect.DamageType));
                    break;
                case "Healing":
                    effects.Add(new HealingEffect(effect.Power));
                    break;
                    // Handle other effect types
            }
        }

        return effects;
    }

    private static List<StatusEffectMetadata> CreateStatusEffectsBasedOnMetadata(List<StatusEffectMetadata> statusEffectMetadata)
    {
            return statusEffectMetadata;
    }
}
