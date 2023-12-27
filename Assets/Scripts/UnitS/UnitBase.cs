using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private int unitID;
    [SerializeField] private string unitName;
    [SerializeField] private int currentHP, maxHP, currentMP, maxMP;
    [SerializeField] private int strength, dexterity, constitution, intelligence;
    [SerializeField] public int criticalChance, criticalDamageMultiplier, damageReduction, damageBonus, evasion, accuracy;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isMarkedForElimination;

    [SerializeField] private bool isPlayerUnit;
    [SerializeField] private bool isPlaying;
    [SerializeField] private List<IStatusEffect> statusEffects = new List<IStatusEffect>();
    [SerializeField] public bool IsStunned;
    [SerializeField] public bool HasHaste;
    [SerializeField] public int ActionsPerTurn = 1;
    [SerializeField] public UnitStatusUI unitStatusUI;


    public int UnitID
    {
        get { return unitID; }
        set { unitID = value; }
    }
    public string UnitName
    {
        get { return unitName; }
        protected set { unitName = value; }
    }
    public int CurrentHP
    {
        get { return currentHP; }
        protected set { currentHP = Mathf.Max(0, value); }
    }
    public int MaxHP
    {
        get { return maxHP; }
        protected set { maxHP = Mathf.Max(0, value); }
    }
    public int CurrentMP
    {
        get { return currentMP; }
        protected set { currentMP = Mathf.Max(0, value); }
    }
    public int MaxMP
    {
        get { return maxMP; }
        protected set { maxMP = Mathf.Max(0, value); }
    }
    public int Strength
    {
        get { return strength; }
        protected set { strength = Mathf.Max(0, value); }
    }
    public int Dexterity
    {
        get { return dexterity; }
        protected set { dexterity = Mathf.Max(0, value); }
    }
    public int Constitution
    {
        get { return constitution; }
        protected set { constitution = Mathf.Max(0, value); }
    }
    public int Intelligence
    {
        get { return intelligence; }
        protected set { intelligence = Mathf.Max(0, value); }
    }
    public List<IStatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    public bool IsMarkedForElimination
    {
        get { return isMarkedForElimination; }
        set { isMarkedForElimination = value; }
    }
    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
        set { isPlayerUnit = value; }
    }
    public bool IsPlaying
    {
        get { return isPlaying; }
        set { isPlaying = value; }
    }

    public void Initialize(bool isPlayer)
    {
        IsPlayerUnit = isPlayer;
        UnitName = IsPlayerUnit ? UnitName : "Evil " + UnitName;
    }

    public virtual int MainAttributeValue
    {
        get { return 0; } // Default implementation, to be overridden
    }

    public int RollInitiative() => Random.Range(1, 21) + dexterity;

    public void TakeDamage(int dmg, DamageType dmgType)
    {
        CurrentHP -= dmg;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            isMarkedForElimination = true;
        }
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public void ReduceMP(int amount)
    {
        CurrentMP -= amount;
        if (CurrentMP <= 0)
        {
            CurrentMP = 0;
        }
    }

    public bool IsUnitFriendly(UnitBase otherUnit)
    {
        // Check if both units are players or both are not players
        return (IsPlayerUnit && otherUnit.IsPlayerUnit) || (!IsPlayerUnit && !otherUnit.IsPlayerUnit);
    }

    public bool CanUseSkill(Skill skill)
    {
        return currentMP >= skill.SkillCost;
    }

    public void ApplyHaste(int duration)
    {
        // Implement logic to handle Haste effect
        // This might involve flagging the unit for an extra turn or adjusting turn order
    }

    public void ApplyStatusEffect(IStatusEffect effect)
    {
        if (effect == null)
        {
            Debug.LogError("Tried to apply a null status effect.");
            return;
        }
        var existingEffect = statusEffects.FirstOrDefault(e => e.Type == effect.Type);
        if (existingEffect != null)
        {
            existingEffect.Duration = effect.Duration;
        }
        else
        {
            statusEffects.Add(effect);
            effect.ApplyStatus(this);
        }

        unitStatusUI.UpdateStatusBar(this);
    }

    public void RemoveStatusEffect(IStatusEffect effect)
    {
        if (statusEffects.Contains(effect))
        {
            effect.RemoveStatus(this);
            statusEffects.Remove(effect);
        }
    }

    public void UpdateStatusEffects()
    {
        // Create a list to hold effects that need to be removed
        var effectsToRemove = new List<IStatusEffect>();

        foreach (var effect in statusEffects)
        {
            effect.UpdateStatus(this);
            if (effect.Duration <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }

        // Remove expired effects
        foreach (var expiredEffect in effectsToRemove)
        {
            RemoveStatusEffect(expiredEffect);
        }

        unitStatusUI.UpdateStatusBar(this);
    }

    public void ExpiredEffectsCheck()
    {
        List<IStatusEffect> effectsToRemove = new List<IStatusEffect>();

        foreach (var effect in statusEffects)
        {
            // effect.UpdateStatus(this);
            // if (effect.IsEffectExpired()) // Assuming each effect has a method to check if it's expired
            // {
            //     effectsToRemove.Add(effect);
            // }
        }

        foreach (var expiredEffect in effectsToRemove)
        {
            RemoveStatusEffect(expiredEffect);
        }
    }
}
