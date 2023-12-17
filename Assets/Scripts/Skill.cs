using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public int id;
    public string skillName;
    public string skillDescription;
    public int skillPower;
    public int skillCost;
    public int targetCount;
    public SkillType skillType;
    public DamageType skillDamageType;
    public SkillSFX skillSFX;
    public int[] unitID; // An array of class IDs that can use this skill

}

public enum SkillType
{
    BasicAttack,
    Damage,
    Healing
}

public enum DamageType
{
    Physical,
    Magical
}
