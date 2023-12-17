using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private int unitID;
    [SerializeField] private string unitName;
    [SerializeField] private int currentHP, maxHP, currentMP, maxMP;
    [SerializeField] private int strength, dexterity, constitution, intelligence, defense, magicDefense, power, evasion, accuracy;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isPlayerUnit;
    [SerializeField] private bool isPlaying;
    private int baseDamage;
    private float baseSkillMultiplier;

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
    public int Defense
    {
        get { return defense; }
        protected set { defense = Mathf.Max(0, value); }
    }
    public int MagicDefense
    {
        get { return magicDefense; }
        protected set { magicDefense = Mathf.Max(0, value); }
    }
    public int Power
    {
        get { return power; }
        protected set { power = Mathf.Max(0, value); }
    }
    public int Evasion
    {
        get { return evasion; }
        protected set { evasion = Mathf.Max(0, value); }
    }
    public int Accuracy
    {
        get { return accuracy; }
        protected set { accuracy = Mathf.Max(0, value); }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
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

    public int BaseDamage
    {
        get { return baseDamage; }
        protected set { baseDamage = Mathf.Max(0, value); }
    }

    public float BaseSkillMultiplier
    {
        get { return baseSkillMultiplier; }
        protected set { baseSkillMultiplier = Mathf.Max(0, value); }
    }

    public void Initialize(bool isPlayer)
    {
        IsPlayerUnit = isPlayer;
        UnitName = IsPlayerUnit ? UnitName : "Evil " + UnitName;
    }

    public int TakeDamage(int dmg, DamageType dmgType)
    {
        int totalDamage = dmg;
        // Implement Defense at some point
        CurrentHP -= totalDamage;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            isDead = true;
        }
        return totalDamage;
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

    public int RollInitiative() => Random.Range(1, 21) + dexterity;

    public virtual int BasicAttack()
    {
        return baseDamage;
    }

    public virtual float SkillMultiplier()
    {
        return baseSkillMultiplier;
    }

    public bool IsUnitFriendly(UnitBase otherUnit)
    {
        // Check if both units are players or both are not players
        return (IsPlayerUnit && otherUnit.IsPlayerUnit) || (!IsPlayerUnit && !otherUnit.IsPlayerUnit);
    }

    public bool CanUseSkill(Skill skill)
    {
        return currentMP >= skill.skillCost;
    }
}
