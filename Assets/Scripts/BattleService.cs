using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleService
{
    private IBattleMenu battleMenu;
    private ICombatLogger combatLogger;
    private IAIManager aiManager;
    private BattleManager battleManager;
    private List<UnitBase> activeUnits;
    public List<UnitBase> ActiveUnits => activeUnits;
    private bool isBattleActive;
    private int currentTurn;
    private UnitBase activeUnit;
    public UnitBase ActiveUnit => activeUnit;
    private Skill selectedSkill;
    public Skill SelectedSkill => selectedSkill;
    private List<UnitBase> targetUnits;
    public List<UnitBase> TargetUnits => targetUnits;
    private int actionsTakenThisTurn;

    public BattleService(IBattleMenu battleMenu, ICombatLogger combatLogger, IAIManager aiManager, BattleManager battleManager)
    {
        this.battleMenu = battleMenu;
        this.combatLogger = combatLogger;
        this.aiManager = aiManager;
        this.battleManager = battleManager;
    }

    public void StartBattle(int playerIDs, int enemyIDs)
    {
        isBattleActive = true;
        activeUnits = new List<UnitBase>();
        CreatePlayerUnits(playerIDs);
        CreateEnemyUnits(enemyIDs);

        activeUnits.Sort((unitA, unitB) => unitB.RollInitiative().CompareTo(unitA.RollInitiative()));
        currentTurn = -1;
        combatLogger.AddEventToCombatLog("Combat Starts!", false);
        NextTurn();
    }

    private void CreatePlayerUnits(int playerIDs)
    {
        for (int i = 0; i < playerIDs; i++)
        {
            UnitBase playerUnit = battleManager.CreatePlayerUnit(i);
            battleMenu.AddUnitStatsPanel(playerUnit);
            activeUnits.Add(playerUnit);
        }
    }

    private void CreateEnemyUnits(int enemyIDs)
    {
        for (int i = 0; i < enemyIDs; i++)
        {
            UnitBase enemyUnit = battleManager.CreateEnemyUnit(i);
            battleMenu.AddUnitStatsPanel(enemyUnit);
            activeUnits.Add(enemyUnit);
        }
    }

    public void NextTurn()
    {
        if (!isBattleActive) return;
        EndUnitTurn();
        HandleUnitElimination();
        MoveToNextUnit();
    }

    public void EndBattle()
    {
        isBattleActive = false;
        currentTurn = 0;
        activeUnit = null;
        battleMenu.DisableCombatUI();
    }

    public void EndUnitTurn()
    {
        if (activeUnit != null)
        {
            activeUnit.IsPlaying = false;
            activeUnit.UpdateStatusEffects();
            battleMenu.UpdateUnitStatsPanelText(activeUnit);
        }
    }

    public void HandleUnitElimination()
    {
        for (int i = activeUnits.Count - 1; i >= 0; i--)
        {
            if (activeUnits[i].IsMarkedForElimination)
            {
                combatLogger.AddEventToCombatLog(activeUnits[i].UnitName + " is unconscious!");
                activeUnits.RemoveAt(i);
            }
        }
    }

    public void MoveToNextUnit()
    {
        if (!isBattleActive) return;

        targetUnits = new List<UnitBase>();
        selectedSkill = null;
        actionsTakenThisTurn = 0;

        currentTurn = (currentTurn + 1) % activeUnits.Count;
        activeUnit = activeUnits[currentTurn];
        activeUnit.IsPlaying = true;
        combatLogger.AddEventToCombatLog($"It is {activeUnit.UnitName}'s turn.");
        battleMenu.SetInitiativePanel(activeUnits);

        if (activeUnit.IsStunned)
        {
            combatLogger.AddEventToCombatLog($"{activeUnit.UnitName} is stunned and loses its turn.");
            NextTurn();
        }
        else
        {
            if (activeUnit.IsPlayerUnit)
            {
                battleMenu.PlayerTurn();
                battleMenu.PopulateMagicPanel(SkillManager.Instance.GetClassSpecificSkills(activeUnit.UnitID));
                battleManager.PlayerMove();
            }
            else
            {
                battleMenu.EnemyTurn();
                battleManager.EnemyAIMove(activeUnit);
            }
        }
    }

    public void PlayerBasicAttack()
    {
        selectedSkill = SkillManager.Instance.SkillList[0];
    }

    public void PlayerSelectTargetUnit(UnitBase unit)
    {
        if (selectedSkill != null && activeUnits.Contains(unit))
        {
            if (Skill.SkillTargetHostile(selectedSkill))
            {
                if (!activeUnit.IsUnitFriendly(unit))
                {
                    targetUnits.Add(unit);
                }
            }
            else if (Skill.SkillTargetFriendly(selectedSkill))
            {
                if (activeUnit.IsUnitFriendly(unit))
                {
                    targetUnits.Add(unit);
                }
            }
            else
            {
                Debug.Log("Skill Target Selection Error");
            }
        }
    }

    public void SkillSelection(Skill skill)
    {
        selectedSkill = skill;
        if (selectedSkill != null)
        {
            if (Skill.SkillTargetSelf(selectedSkill)) targetUnits.Add(activeUnit);
            if (selectedSkill.TargetCount > 1)
            {
                if (Skill.SkillTargetHostile(selectedSkill))
                {
                    targetUnits = activeUnits.FindAll(member => !activeUnit.IsUnitFriendly(member));
                }
                else if (Skill.SkillTargetFriendly(selectedSkill))
                {
                    targetUnits = activeUnits.FindAll(member => activeUnit.IsUnitFriendly(member));
                }
                else
                {
                    Debug.Log("Skill Target Selection Error");
                }
            }
        }
    }

    public void ExecuteAction(Skill skill, IEnumerable<UnitBase> targets)
    {
        ProcessSkillEffect(activeUnit, skill, targets);
        UpdateBattleUI();
        CheckBattleStatus();

        actionsTakenThisTurn++;
        if (actionsTakenThisTurn >= activeUnit.ActionsPerTurn)
        {
            NextTurn();
        }
        else
        {
            combatLogger.AddEventToCombatLog(activeUnit.UnitName + " is hastened, and acts again!");
            selectedSkill = null;
            targetUnits = new List<UnitBase>();

            // Continue with the current unit's turn
            if (activeUnit.IsPlayerUnit)
            {
                battleManager.PlayerMove();
            }
            else
            {
                battleManager.EnemyAIMove(activeUnit);
            }
        }
    }

    public void ProcessSkillEffect(UnitBase sourceUnit, Skill skill, IEnumerable<UnitBase> targets)
    {
        foreach (var target in targets)
        {
            if (CheckSkillHit(sourceUnit, skill, target))
            {
                // Apply all the skill's effects and status effects to the target
                skill.Activate(sourceUnit, new List<UnitBase> { target }, skill, battleManager);
            }
            else
            {
                battleManager.DisplaySkillEffect(target, "Miss");
                combatLogger.AddEventToCombatLog($"{sourceUnit.UnitName} uses {skill.SkillName} and misses {target.UnitName}!");
            }
        }

        // Reduce MP of the active unit if the skill requires it
        sourceUnit.ReduceMP(skill.SkillCost);

        // Display effects on the target unit (if applicable)
        // battleManager.DisplaySkillEffect(target, skill);
    }

    public void AddEventToCombatLog(string message)
    {
        combatLogger.AddEventToCombatLog(message);
    }

    public bool CheckSkillHit(UnitBase sourceUnit, Skill skill, UnitBase target)
    {
        if (Skill.SkillTargetFriendly(skill)) return true;
        float hitChance = sourceUnit.accuracy + skill.SkillAccuracy - target.evasion;
        return UnityEngine.Random.Range(0f, 100f) <= hitChance;
    }

    public void UpdateBattleUI()
    {
        // Update UI elements related to the active and target units
        battleMenu.UpdateUnitStatsPanelText(activeUnit);
        foreach (var target in targetUnits)
        {
            battleMenu.UpdateUnitStatsPanelText(target);
        }
    }

    public void CheckBattleStatus()
    {
        bool allEnemiesAreDead = true;
        bool allPlayerAreDead = true;

        for (int i = 0; i < activeUnits.Count; i++)
        {
            if (activeUnits[i].IsPlayerUnit) allPlayerAreDead = false;
            else allEnemiesAreDead = false;
        }

        if (allEnemiesAreDead || allPlayerAreDead)
        {
            string resultMessage = allEnemiesAreDead ? "VICTORY" : "DEFEAT";
            Debug.Log(resultMessage);
            EndBattle();
        }
    }
}
