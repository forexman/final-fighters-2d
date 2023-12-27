using System.Buffers.Text;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    // Chooses a skill for the unit, prioritizing healing if necessary
    public static Skill ChooseSkill(UnitBase unit)
    {
        List<Skill> availableSkills = BattleManager.instance.GetClassSpecificSkills(unit.UnitID);
        Skill preferredSkill = null;

        // Example Logic: Prioritize healing if necessary, else choose an attack skill
        preferredSkill = ChooseHealingSkillIfNecessary(unit, availableSkills)
            ?? ChooseAttackSkill(unit, availableSkills);
        Debug.Log($"AI Chose {preferredSkill.SkillName}");
        return preferredSkill ?? SkillManager.Instance.SkillList[0];
    }

    // Chooses an attack skill from available skills
    private static Skill ChooseAttackSkill(UnitBase unit, List<Skill> availableSkills)
    {
        availableSkills = availableSkills.FindAll(skill => IsSkillDamage(skill));
        Shuffle(availableSkills);

        foreach (Skill skill in availableSkills)
        {
            if (unit.CanUseSkill(skill))
            {
                return skill;
            }
        }

        return null;
    }

    // Chooses a healing skill if there is an effective one available
    private static Skill ChooseHealingSkillIfNecessary(UnitBase unit, List<Skill> availableSkills)
    {
        List<Skill> healingSkills = availableSkills.FindAll(skill => IsSkillHealing(skill));
        Shuffle(healingSkills);

        foreach (Skill skill in healingSkills)
        {
            if (unit.CanUseSkill(skill) && IsHealingEffective(unit, skill))
            {
                return skill;
            }
        }

        return null;
    }

    private static bool IsSkillHealing(Skill skill)
    {
        return skill.Effects.Exists(e => e is HealingEffect);
    }

    private static bool IsSkillDamage(Skill skill)
    {
        return skill.Effects.Exists(e => e is DamageEffect || e is DamageOverTimeEffect);
    }

    // Checks if healing is effective for any teammate
    private static bool IsHealingEffective(UnitBase unit, Skill skill)
    {
        return FindMostInjuredTeammate(unit, skill) != null;
    }

    // Chooses a target based on the skill type
    public static List<UnitBase> ChooseTargets(UnitBase unit, Skill skill)
    {
        List<UnitBase> targetUnits = new List<UnitBase>();
        if (skill.TargetCount == 1)
        {
            UnitBase targetUnit = IsSkillHealing(skill) ? FindMostInjuredTeammate(unit, skill) : GetRandomOpposingTeamUnit(unit);
            targetUnits.Add(targetUnit);
        }
        else
        {
            if (Skill.SkillTargetHostile(skill))
            {
                targetUnits = BattleManager.instance.ActiveUnits.FindAll(member => !unit.IsUnitFriendly(member));
            }
            else if (Skill.SkillTargetFriendly(skill))
            {
                targetUnits = BattleManager.instance.ActiveUnits.FindAll(member => unit.IsUnitFriendly(member));
            }
            else
            {
                Debug.Log("Skill Target Selection Error");
            }
        }

        return targetUnits;
    }

    // Finds the most injured teammate for healing
    private static UnitBase FindMostInjuredTeammate(UnitBase unit, Skill skill)
    {
        int healingAmount = (int)(skill.SkillPower);
        List<UnitBase> teamMembers = BattleManager.instance.ActiveUnits.FindAll(member =>
            unit.IsUnitFriendly(member) && healingAmount <= member.MaxHP - member.CurrentHP);

        if (teamMembers.Count == 0)
            return null;

        teamMembers.Sort((a, b) => (a.CurrentHP / a.MaxHP).CompareTo(b.CurrentHP / b.MaxHP));
        return teamMembers[0];
    }

    // Gets a random unit from the opposing team
    private static UnitBase GetRandomOpposingTeamUnit(UnitBase unit)
    {
        List<UnitBase> opposingTeam = GetOpposingTeamUnits(unit);
        return opposingTeam.Count > 0 ? opposingTeam[Random.Range(0, opposingTeam.Count)] : null;
    }

    // Retrieves all units from the opposing team
    private static List<UnitBase> GetOpposingTeamUnits(UnitBase unit)
    {
        return BattleManager.instance.ActiveUnits.FindAll(otherUnit => !unit.IsUnitFriendly(otherUnit));
    }

    // Shuffles a list to randomize elements
    private static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
