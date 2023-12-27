using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill
{
    public int Id;
    public string SkillName;
    public string SkillDescription;
    public int SkillPower;
    public int SkillDuration;
    public int SkillCost;
    public SkillTargetType SkillTarget;
    public int TargetCount; // Number of targets this skill can affect
    public List<ISkillEffect> Effects; // List of effects
    public List<StatusEffectMetadata> StatusEffectMetadata; // Status effects like stun, haste
    public int[] UnitID; // Array of class IDs that can use this skill

    public float CriticalChance { get; private set; }
    public float CriticalDamageMultiplier { get; private set; }
    public float SkillAccuracy { get; private set; }

    // Constructor to initialize the skill
    public Skill(int id, string name, string description, int power, int cost, SkillTargetType skillTarget, int targetCount, int duration,
                 List<ISkillEffect> effects, List<StatusEffectMetadata> statusEffectMetadata, int[] unitId,
                 float criticalChance = 0, float criticalDamageMultiplier = 0, float skillAccuracy = 0)
    {
        Id = id;
        SkillName = name;
        SkillDescription = description;
        SkillPower = power;
        SkillCost = cost;
        SkillTarget = skillTarget;
        SkillDuration = duration;
        Effects = effects;
        StatusEffectMetadata = statusEffectMetadata;
        TargetCount = targetCount;
        UnitID = unitId;
        CriticalChance = criticalChance;
        CriticalDamageMultiplier = criticalDamageMultiplier;
        SkillAccuracy = skillAccuracy;
    }

    // Activate method applies all the skill's effects and status effects
    public void Activate(UnitBase source, IEnumerable<UnitBase> targets, Skill skill, IBattleManager battleManager)
    {
        foreach (var target in targets)
        {
            foreach (var effect in Effects)
            {
                // Apply each effect to the target
                effect.ApplyEffect(source, new List<UnitBase> { target }, skill);
            }

            foreach (var statusEffectMetadata in StatusEffectMetadata)
            {
                // We need them instanced, as there might be duplicates
                IStatusEffect statusEffect = CreateStatusEffect(statusEffectMetadata);
                statusEffect.SetDependencies(battleManager);
                target.ApplyStatusEffect(statusEffect);
            }
        }
    }

    public static bool SkillTargetHostile(Skill skill)
    {
        return skill.SkillTarget == SkillTargetType.Hostile;
    }

    public static bool SkillTargetFriendly(Skill skill)
    {
        return skill.SkillTarget == SkillTargetType.Friendly;
    }

    public static bool SkillTargetSelf(Skill skill)
    {
        return skill.SkillTarget == SkillTargetType.Self;
    }

    public static bool IsSkillHealing(Skill skill)
    {
        return skill.Effects.Exists(e => e is HealingEffect);
    }

    private IStatusEffect CreateStatusEffect(StatusEffectMetadata metadata)
    {
        switch (metadata.Type)
        {
            case "Stun":
                return new StunStatus(metadata.Duration);
            case "Poison":
                return new DamageOverTimeStatus(metadata.Power, metadata.Duration);
            case "Haste":
                return new HasteStatus(metadata.Duration);
            case "EvasionUp":
                return new BuffStatus(metadata.Type, metadata.Power, metadata.Duration);
            case "DefenseUp":
                return new BuffStatus(metadata.Type, metadata.Power, metadata.Duration);
            case "AccuracyDown":
                return new DebuffStatus(metadata.Type, metadata.Power, metadata.Duration);

        }
        return null;
    }
}

public enum DamageType
{
    Physical,
    Magical,
    True,
    None
}

public enum SkillTargetType
{
    Hostile,
    Friendly,
    Self
}