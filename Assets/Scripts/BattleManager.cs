using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Serialized fields for customization in Unity Editor
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Transform[] enemyPositions;
    [SerializeField] private GameObject[] playerPrefabs, enemyPrefabs;
    [SerializeField] private GameObject floatingDamageTextPrefab;
    private IBattleMenu battleMenu;
    private IAIManager aiManager;
    private ICombatLogger combatLogger;
    private BattleService battleService;

    public void Initialize(IBattleMenu battleMenu, ICombatLogger combatLogger, IAIManager aiManager)
    {
        this.battleMenu = battleMenu;
        this.combatLogger = combatLogger;
        this.aiManager = aiManager;
        battleService = new BattleService(battleMenu, combatLogger, aiManager, this);
    }

    public UnitBase GetActiveUnit()
    {
        return battleService.ActiveUnit;
    }
    public List<UnitBase> GetActiveUnits()
    {
        return battleService.ActiveUnits;
    }

    private void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        battleService.StartBattle(playerPrefabs.Length, enemyPrefabs.Length);
    }

    public UnitBase CreatePlayerUnit(int playerPrefabIndex)
    {
        GameObject playerInstance = Instantiate(playerPrefabs[playerPrefabIndex], playerPositions[playerPrefabIndex].position, Quaternion.identity);
        UnitBase playerUnit = playerInstance.GetComponent<UnitBase>();
        playerUnit.Initialize(true, this);
        SetupUnitSpriteRenderer(playerInstance);
        return playerUnit;
    }

    public UnitBase CreateEnemyUnit(int enemyPrefabIndex)
    {
        GameObject enemyInstance = Instantiate(enemyPrefabs[enemyPrefabIndex], enemyPositions[enemyPrefabIndex].position, Quaternion.identity);
        UnitBase enemyUnit = enemyInstance.GetComponent<UnitBase>();
        enemyUnit.Initialize(false, this);
        return enemyUnit;
    }

    public void SetupUnitSpriteRenderer(GameObject unitInstance)
    {
        SpriteRenderer spriteRenderer = unitInstance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.flipX = true;
    }

    public void EndBattle()
    {
        Debug.Log("Called BattleManager");
        battleService.EndBattle();
    }

    private void NextTurn()
    {
        Debug.Log("Called BattleManager");
        battleService.NextTurn();
    }

    public void EndUnitTurn()
    {
        Debug.Log("Called BattleManager");
        battleService.EndUnitTurn();
    }

    private void HandleUnitElimination()
    {
        Debug.Log("Called BattleManager");
        battleService.HandleUnitElimination();
    }

    private void MoveToNextUnit()
    {
        Debug.Log("Called BattleManager");
        battleService.HandleUnitElimination();
    }

    public void PlayerMove()
    {
        StartCoroutine(PlayerMoveCoroutine());
    }

    public void EnemyAIMove(UnitBase activeUnit)
    {
        StartCoroutine(EnemyAIMoveCoroutine(activeUnit));
    }

    public IEnumerator PlayerMoveCoroutine()
    {
        while (battleService.SelectedSkill == null || battleService.TargetUnits.Count == 0)
        {
            yield return null;
        }
        battleService.ExecuteAction(battleService.SelectedSkill, battleService.TargetUnits);
        yield return new WaitForSeconds(.3f);
    }

    public IEnumerator EnemyAIMoveCoroutine(UnitBase activeUnit)
    {
        battleService.SkillSelection(aiManager.AIChooseSkill(activeUnit));
        battleService.ExecuteAction(battleService.SelectedSkill, aiManager.AIChooseTargets(activeUnit, battleService.SelectedSkill));
        yield return new WaitForSeconds(0.3f);
    }

    public void PlayerBasicAttack()
    {
        battleService.PlayerBasicAttack();
    }

    public void PlayerSelectTargetUnit(UnitBase unit)
    {
        battleService.PlayerSelectTargetUnit(unit);
    }

    public void SkillSelection(Skill skill)
    {
        battleService.SkillSelection(skill);
    }

    public void AddEventToCombatLog(string message)
    {
        battleService.AddEventToCombatLog(message);
    }

    public void DisplaySkillEffect(UnitBase target, string effectiveValue)
    {
        UnitDamageGUI unitDamageText = Instantiate(floatingDamageTextPrefab.GetComponent<UnitDamageGUI>(), new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z), target.transform.rotation);
        unitDamageText.SetValueGUI(effectiveValue);
    }

    public void DisplaySkillEffect(UnitBase target, string effectiveValue, ISkillEffect effect)
    {
        UnitDamageGUI unitDamageText = Instantiate(floatingDamageTextPrefab.GetComponent<UnitDamageGUI>(), new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z), target.transform.rotation);
        unitDamageText.SetValueGUI(effectiveValue, effect);
    }

    public void DisplaySkillEffect(UnitBase target, string effectiveValue, IStatusEffect effect)
    {
        UnitDamageGUI unitDamageText = Instantiate(floatingDamageTextPrefab.GetComponent<UnitDamageGUI>(), new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z), target.transform.rotation);
        unitDamageText.SetValueGUI(effectiveValue, effect);
    }

}
