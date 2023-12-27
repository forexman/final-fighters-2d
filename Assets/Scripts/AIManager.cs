using System.Buffers.Text;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour, IAIManager
{
    private IBattleManager battleManager;

    public void Initialize(IBattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    // Chooses a skill for the unit, prioritizing healing if necessary
    public Skill AIChooseSkill(UnitBase unit)
    {
        List<Skill> availableSkills = SkillManager.Instance.GetClassSpecificSkills(unit.UnitID);
        Skill preferredSkill = null;

        // Example Logic: Prioritize healing if necessary, else choose an attack skill
        preferredSkill = ChooseHealingSkillIfNecessary(unit, availableSkills)
            ?? ChooseAttackSkill(unit, availableSkills);
        return preferredSkill;
    }

    // Chooses an attack skill from available skills
    private static Skill ChooseAttackSkill(UnitBase unit, List<Skill> availableSkills)
    {
        availableSkills = availableSkills.FindAll(skill => Skill.SkillTargetHostile(skill));
        Shuffle(availableSkills);

        foreach (Skill skill in availableSkills)
        {
            if (unit.CanUseSkill(skill))
            {
                return skill;
            }
        }
        return SkillManager.Instance.SkillList[0];
    }

    // Chooses a healing skill if there is an effective one available
    private Skill ChooseHealingSkillIfNecessary(UnitBase unit, List<Skill> availableSkills)
    {
        List<Skill> healingSkills = availableSkills.FindAll(skill => Skill.IsSkillHealing(skill));
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

    // Checks if healing is effective for any teammate
    private bool IsHealingEffective(UnitBase unit, Skill skill)
    {
        return FindMostInjuredTeammate(unit, skill) != null;
    }

    // Chooses a target based on the skill type
    public List<UnitBase> AIChooseTargets(UnitBase unit, Skill skill)
    {
        List<UnitBase> targetUnits = new List<UnitBase>();
        if (skill.TargetCount == 1)
        {
            UnitBase targetUnit = Skill.IsSkillHealing(skill) ? FindMostInjuredTeammate(unit, skill) : GetRandomOpposingTeamUnit(unit);
            targetUnits.Add(targetUnit);
        }
        else
        {
            if (Skill.SkillTargetHostile(skill))
            {
                targetUnits = battleManager.ActiveUnits.FindAll(member => !unit.IsUnitFriendly(member));
            }
            else if (Skill.SkillTargetFriendly(skill))
            {
                targetUnits = battleManager.ActiveUnits.FindAll(member => unit.IsUnitFriendly(member));
            }
            else
            {
                Debug.Log("Skill Target Selection Error");
            }
        }

        return targetUnits;
    }

    // Finds the most injured teammate for healing
    private UnitBase FindMostInjuredTeammate(UnitBase unit, Skill skill)
    {
        int healingAmount = (int)(skill.SkillPower);
        List<UnitBase> teamMembers = battleManager.ActiveUnits.FindAll(member =>
            unit.IsUnitFriendly(member) && healingAmount <= member.MaxHP - member.CurrentHP);

        if (teamMembers.Count == 0)
            return null;

        teamMembers.Sort((a, b) => (a.CurrentHP / a.MaxHP).CompareTo(b.CurrentHP / b.MaxHP));
        return teamMembers[0];
    }

    // Gets a random unit from the opposing team
    private UnitBase GetRandomOpposingTeamUnit(UnitBase unit)
    {
        List<UnitBase> opposingTeam = GetOpposingTeamUnits(unit);
        return opposingTeam.Count > 0 ? opposingTeam[Random.Range(0, opposingTeam.Count)] : null;
    }

    // Retrieves all units from the opposing team
    private List<UnitBase> GetOpposingTeamUnits(UnitBase unit)
    {
        return battleManager.ActiveUnits.FindAll(otherUnit => !unit.IsUnitFriendly(otherUnit));
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
