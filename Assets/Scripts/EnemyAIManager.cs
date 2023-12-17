using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    // Executes an action for a given unit based on selected skill and target
    public static void ExecuteAction(UnitBase unit)
    {
        Skill chosenSkill = ChooseSkill(unit);
        UnitBase targetUnit = ChooseTarget(unit, chosenSkill);

        if (chosenSkill != null && targetUnit != null)
        {
            BattleManager.instance.ExecuteAction(chosenSkill, targetUnit);
        }
    }

    // Chooses a skill for the unit, prioritizing healing if necessary
    private static Skill ChooseSkill(UnitBase unit)
    {
        List<Skill> availableSkills = BattleManager.instance.GetClassSpecificSkills(unit.UnitID);
        Skill healingSkill = ChooseHealingSkillIfNecessary(unit, availableSkills);

        return healingSkill ?? ChooseAttackSkill(unit, availableSkills) ?? BattleManager.instance.BasicAttack;
    }

    // Chooses an attack skill from available skills
    private static Skill ChooseAttackSkill(UnitBase unit, List<Skill> availableSkills)
    {
        availableSkills = availableSkills.FindAll(skill => skill.skillType == SkillType.Damage);
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
        List<Skill> healingSkills = availableSkills.FindAll(skill => skill.skillType == SkillType.Healing);
        Shuffle(healingSkills);

        foreach (Skill skill in healingSkills)
        {
            int effectiveHealing = (int)(skill.skillPower * unit.SkillMultiplier());
            if (unit.CanUseSkill(skill) && IsHealingEffective(unit, effectiveHealing))
            {
                return skill;
            }
        }

        return null;
    }

    // Checks if healing is effective for any teammate
    private static bool IsHealingEffective(UnitBase unit, int healingAmount)
    {
        return FindMostInjuredTeammate(unit, healingAmount) != null;
    }

    // Chooses a target based on the skill type
    private static UnitBase ChooseTarget(UnitBase unit, Skill skill)
    {
        return skill.skillType == SkillType.Healing 
            ? FindMostInjuredTeammate(unit, (int)(skill.skillPower * unit.SkillMultiplier()))
            : GetRandomOpposingTeamUnit(unit);
    }

    // Finds the most injured teammate for healing
    private static UnitBase FindMostInjuredTeammate(UnitBase unit, int healingAmount)
    {
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
