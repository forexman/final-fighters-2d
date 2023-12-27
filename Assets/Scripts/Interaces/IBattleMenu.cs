using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleMenu
{
    void EnableUnitPanel();
    void EnableMagicPanel();
    void DisableCombatUI();
    void AddUnitStatsPanel(UnitBase unit);
    void UpdateUnitStatsPanelText(UnitBase unit);
    void PlayerTurn();
    void EnemyTurn();
    void PopulateMagicPanel(List<Skill> classSkills);
    void SetDescription(string message);
    void BasicAttack();
    void MouseOverSkill(Skill skill, bool isButtonMouseOvered);
    void SetInitiativePanel(List<UnitBase> activeUnits);
}