using System.Collections.Generic;

public interface IBattleManager
{
    UnitBase ActiveUnit { get; }
    Skill SelectedSkill { get; set; }
    List<UnitBase> ActiveUnits { get; }

    void StartBattle();
    void EndBattle();
    void ExecuteAction(Skill skill, IEnumerable<UnitBase> targets);
    void PlayerBasicAttack();
    void PlayerSelectTargetUnit(UnitBase unit);
    void SkillSelection(Skill skill);
    void AddEventToCombatLog(string message);
}