using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SkillMetadata
{
    public int Id;
    public string Name;
    public string Description;
    public int Power;
    public int Cost;
    public int Duration;
    public SkillTargetType SkillTarget;
    public int TargetCount; // Number of targets the skill can affect
    public DamageType DamageType;
    public List<EffectMetadata> Effects; // List of effects
    public List<StatusEffectMetadata> StatusEffects; // Status effects like stun, haste
    public int[] UnitID; // Array of class IDs that can use this skill
    public float CriticalChance;
    public float CriticalDamageMultiplier;
    public float SkillAccuracy;

    // Constructor to quickly create new SkillMetadata instances
    public SkillMetadata(int id, string name, string description, int power, int duration, SkillTargetType skillTarget,
                         int cost, int targetCount, DamageType damageType, List<EffectMetadata> effects, List<StatusEffectMetadata> statusEffects, int[] unitId,
                         float criticalChance, float criticalDamageMultiplier, float skillAccuracy)
    {
        Id = id;
        Name = name;
        Description = description;
        Duration = duration;
        Power = power;
        Cost = cost;
        SkillTarget = skillTarget;
        TargetCount = targetCount;
        DamageType = damageType;
        Effects = effects;
        StatusEffects = statusEffects;
        UnitID = unitId;
        CriticalChance = criticalChance;
        CriticalDamageMultiplier = criticalDamageMultiplier;
        SkillAccuracy = skillAccuracy;
    }

    // You can add additional methods or properties if needed
}

[Serializable]
public class EffectMetadata
{
    public string Type;
    public int Power;
    public DamageType DamageType;
}

[Serializable]
public class StatusEffectMetadata
{
    public string Type;
    public int Power;
    public int Duration;
}
