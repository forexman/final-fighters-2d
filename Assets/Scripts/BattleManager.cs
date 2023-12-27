using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour, IBattleManager
{
    // Serialized fields for customization in Unity Editor
    [SerializeField] private bool isBattleActive;
    [SerializeField] private List<UnitBase> activeUnits;
    [SerializeField] private UnitBase activeUnit;
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Transform[] enemyPositions;
    [SerializeField] private GameObject[] playerPrefabs, enemyPrefabs;
    [SerializeField] private int currentTurn;
    [SerializeField] private UnitDamageGUI dmgText;
    [SerializeField] private Skill selectedSkill;
    [SerializeField] private List<UnitBase> targetUnits;
    [SerializeField] private int actionsTakenThisTurn;
    private CombatLog combatLog;
    private IBattleMenu battleMenu;
    private IAIManager aiManager;

    // Getters and setters
    public UnitBase ActiveUnit => activeUnit; //read-only, turn orders are only handled here
    public Skill SelectedSkill
    {
        get => selectedSkill;
        set => selectedSkill = value;
    }
    public List<UnitBase> ActiveUnits
    {
        get => activeUnits;
        protected set => activeUnits = value;
    }


    public void Initialize(IBattleMenu battleMenu, CombatLog combatLog, IAIManager aiManager)
    {
        this.battleMenu = battleMenu;
        this.combatLog = combatLog;
        this.aiManager = aiManager;
    }

    private void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        isBattleActive = true;

        activeUnits.Clear();
        CreatePlayerUnits();
        CreateEnemyUnits();

        activeUnits.Sort((unitA, unitB) => unitB.RollInitiative().CompareTo(unitA.RollInitiative()));
        currentTurn = -1;
        combatLog.AddEventToCombatLog("Combat Starts!", false);
        NextTurn();
    }

    private void CreatePlayerUnits()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            GameObject playerInstance = Instantiate(playerPrefabs[i], playerPositions[i].position, Quaternion.identity);
            UnitBase playerUnit = playerInstance.GetComponent<UnitBase>();
            playerUnit.Initialize(true);
            activeUnits.Add(playerUnit);
            SetupUnitSpriteRenderer(playerInstance);
            battleMenu.AddUnitStatsPanel(playerUnit);
        }
    }

    private void SetupUnitSpriteRenderer(GameObject unitInstance)
    {
        SpriteRenderer spriteRenderer = unitInstance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void CreateEnemyUnits()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemyInstance = Instantiate(enemyPrefabs[i], enemyPositions[i].position, Quaternion.identity);
            UnitBase enemyUnit = enemyInstance.GetComponent<UnitBase>();
            enemyUnit.Initialize(false);
            activeUnits.Add(enemyUnit);
            battleMenu.AddUnitStatsPanel(enemyUnit);
        }
    }

    public void EndBattle()
    {
        isBattleActive = false;
        currentTurn = 0;
        activeUnit = null;
        targetUnits = new List<UnitBase>();
        selectedSkill = null;
        battleMenu.DisableCombatUI();
        // Cleanup and reset logic for the end of the battle
    }

    private void NextTurn()
    {
        if (!isBattleActive) return;
        EndUnitTurn();
        HandleUnitElimination();
        MoveToNextUnit();
    }

    private void EndUnitTurn()
    {
        if (activeUnit != null)
        {
            activeUnit.IsPlaying = false;
            // Update status effects for the unit that just completed its turn
            activeUnit.UpdateStatusEffects();
            battleMenu.UpdateUnitStatsPanelText(activeUnit);
        }
    }

    private void HandleUnitElimination()
    {
        for (int i = activeUnits.Count - 1; i >= 0; i--)
        {
            if (activeUnits[i].IsMarkedForElimination)
            {
                combatLog.AddEventToCombatLog(activeUnits[i].UnitName + " is unconscious!");
                activeUnits.RemoveAt(i);
            }
        }
    }

    private void MoveToNextUnit()
    {
        if (!isBattleActive) return;

        targetUnits = new List<UnitBase>();
        selectedSkill = null;
        actionsTakenThisTurn = 0;

        currentTurn = (currentTurn + 1) % activeUnits.Count;
        activeUnit = activeUnits[currentTurn];
        activeUnit.IsPlaying = true;
        combatLog.AddEventToCombatLog($"It is {activeUnit.UnitName}'s turn.");
        battleMenu.SetInitiativePanel(activeUnits);

        if (activeUnit.IsStunned)
        {
            combatLog.AddEventToCombatLog($"{activeUnit.UnitName} is stunned and loses its turn.");
            NextTurn();
        }
        else
        {
            if (activeUnit.IsPlayerUnit)
            {
                battleMenu.PlayerTurn();
                battleMenu.PopulateMagicPanel(SkillManager.Instance.GetClassSpecificSkills(activeUnit.UnitID));
                StartCoroutine(PlayerMoveCoroutine());
            }
            else
            {
                battleMenu.EnemyTurn();
                StartCoroutine(EnemyAIMoveCoroutine());
            }
        }

    }

    private IEnumerator EnemyAIMoveCoroutine()
    {
        SkillSelection(aiManager.AIChooseSkill(activeUnit));
        if (targetUnits.Count == 0) targetUnits = aiManager.AIChooseTargets(activeUnit, selectedSkill);
        ExecuteAction(selectedSkill, targetUnits);
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator PlayerMoveCoroutine()
    {
        while (selectedSkill == null || targetUnits.Count == 0)
        {
            yield return null;
        }
        ExecuteAction(selectedSkill, targetUnits);
        yield return new WaitForSeconds(.3f);
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
            combatLog.AddEventToCombatLog(activeUnit.UnitName + " is hastened, and acts again!");
            selectedSkill = null;
            targetUnits = new List<UnitBase>();
            // Continue with the current unit's turn
            if (activeUnit.IsPlayerUnit)
            {
                StartCoroutine(PlayerMoveCoroutine());
            }
            else
            {
                StartCoroutine(EnemyAIMoveCoroutine());
            }
        }
    }

    private void ProcessSkillEffect(UnitBase sourceUnit, Skill skill, IEnumerable<UnitBase> targets)
    {
        foreach (var target in targets)
        {
            if (CheckSkillHit(sourceUnit, skill, target))
            {
                // Apply all the skill's effects and status effects to the target
                skill.Activate(sourceUnit, new List<UnitBase> { target }, skill, this);
            }
            else
            {
                combatLog.AddEventToCombatLog($"{sourceUnit.UnitName} uses {skill.SkillName} and misses {target.UnitName}!");
            }
        }


        // Reduce MP of the active unit if the skill requires it
        sourceUnit.ReduceMP(skill.SkillCost);

        // Display effects on the target unit (if applicable)
        // DisplaySkillEffect(target, skill);
    }

    public void AddEventToCombatLog(string message)
    {
        combatLog.AddEventToCombatLog(message);
    }

    private bool CheckSkillHit(UnitBase sourceUnit, Skill skill, UnitBase target)
    {
        if (Skill.SkillTargetFriendly(skill)) return true;
        float hitChance = sourceUnit.accuracy + skill.SkillAccuracy - target.evasion;
        return UnityEngine.Random.Range(0f, 100f) <= hitChance;
    }



    private void UpdateBattleUI()
    {
        // Update UI elements related to the active and target units
        battleMenu.UpdateUnitStatsPanelText(activeUnit);
        foreach (var target in targetUnits)
        {
            battleMenu.UpdateUnitStatsPanelText(target);
        }
    }

    private void CheckBattleStatus()
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

    private void DisplaySkillEffect(UnitBase target, int effectiveValue, Skill skill)
    {
        UnitDamageGUI unitDamageText = Instantiate(dmgText, new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z), target.transform.rotation);
        unitDamageText.SetValueGUI(effectiveValue, skill.Effects[0]);
        //Instantiate(skill.skillSFX, new Vector3(target.transform.position.x, target.transform.position.y + 0.6f, target.transform.position.z), target.transform.rotation);
    }
}
