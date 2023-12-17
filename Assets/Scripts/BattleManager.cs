using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    // Serialized fields for customization in Unity Editor
    [SerializeField] private bool isBattleActive;
    [SerializeField] private List<UnitBase> activeUnits;
    [SerializeField] private UnitBase activeUnit;
    [SerializeField] private List<string> combatLog;

    [SerializeField] private GameObject battleScene;
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Transform[] enemyPositions;
    [SerializeField] private GameObject[] playerPrefabs, enemyPrefabs;
    [SerializeField] private int currentTurn;
    [SerializeField] private BattleMenuManager battleMenuManager;
    [SerializeField] private UnitDamageGUI dmgText;

    [SerializeField] private SkillDatabase skillDatabase;
    [SerializeField] private Skill basicAttack;

    // Properties for internal use
    private Skill selectedSkill;
    private UnitBase targetUnit;

    // Getters and setters
    public UnitBase ActiveUnit => activeUnit;
    public Skill SelectedSkill
    {
        get => selectedSkill;
        set => selectedSkill = value;
    }
    public BattleMenuManager BattleMenuManager
    {
        get => battleMenuManager;
        set => battleMenuManager = value;
    }
    public List<UnitBase> ActiveUnits
    {
        get => activeUnits;
        protected set => activeUnits = value;
    }
    public Skill BasicAttack
    {
        get => basicAttack;
        set => basicAttack = value;
    }

    // Singleton pattern to ensure only one instance of BattleManager exists
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        AddEventToCombatLog("Combat Starts!", false);
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
            battleMenuManager.AddUnitStatsPanel(playerUnit);
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
            battleMenuManager.AddUnitStatsPanel(enemyUnit);
        }
    }

    private void AddEventToCombatLog(string msg, bool add = true)
    {
        combatLog.Add(msg);
        battleMenuManager.UpdateCombatLog(msg, add);
    }

    public void EndBattle()
    {
        isBattleActive = false;
        currentTurn = 0;
        activeUnit = null;
        targetUnit = null;
        selectedSkill = null;
        BattleMenuManager.DisableCombatUI();
        // Cleanup and reset logic for the end of the battle
    }

    private void NextTurn()
    {
        if (!isBattleActive) return;
        if (activeUnit != null)
        {
            activeUnit.IsPlaying = false;
        }
        targetUnit = null;
        selectedSkill = null;

        currentTurn = (currentTurn + 1) % activeUnits.Count;
        activeUnit = activeUnits[currentTurn];

        activeUnit.IsPlaying = true;

        battleMenuManager.SetInitiativePanel(activeUnits);

        if (activeUnit.IsPlayerUnit)
        {
            battleMenuManager.PlayerTurn();
            battleMenuManager.PopulateMagicPanel(GetClassSpecificSkills(activeUnit.UnitID));
            StartCoroutine(PlayerMoveCoroutine());
        }
        else
        {
            battleMenuManager.EnemyTurn();
            StartCoroutine(EnemyAIMoveCoroutine());
        }
    }

    private IEnumerator EnemyAIMoveCoroutine()
    {
        EnemyAIManager.ExecuteAction(activeUnits[currentTurn]);
        yield return new WaitForSeconds(0.3f);
        NextTurn();
    }

    private IEnumerator PlayerMoveCoroutine()
    {
        while (selectedSkill == null || targetUnit == null)
        {
            yield return null;
        }
        ExecuteAction(selectedSkill, targetUnit);
        yield return new WaitForSeconds(.3f);
        NextTurn();
    }

    public List<Skill> GetClassSpecificSkills(int classID)
    {
        List<Skill> classSkills = new List<Skill>();

        foreach (Skill skill in skillDatabase.skills)
        {
            if (Array.Exists(skill.unitID, id => id == classID))
            {
                classSkills.Add(skill);
            }
        }

        return classSkills;
    }

    public void PlayerBasicAttack()
    {
        selectedSkill = basicAttack;
    }

    public void PlayerSelectTargetUnit(UnitBase unit)
    {
        if (selectedSkill != null && activeUnits.Contains(unit))
        {
            if (selectedSkill.skillType == SkillType.BasicAttack || selectedSkill.skillType == SkillType.Damage)
            {
                if (!activeUnit.IsUnitFriendly(unit))
                {
                    targetUnit = unit;
                }
            }
            else if (selectedSkill.skillType == SkillType.Healing)
            {
                if (activeUnit.IsUnitFriendly(unit))
                {
                    targetUnit = unit;
                }
            }
        }
    }

    public void ExecuteAction(Skill skill, UnitBase target)
    {
        selectedSkill = skill;
        targetUnit = target;
        // Action execution logic here
        ProcessSkillEffect(skill, target);
        UpdateBattleUI();
        CheckBattleStatus();
    }

    private void ProcessSkillEffect(Skill skill, UnitBase target)
    {
        int effectiveValue = 0;

        switch (skill.skillType)
        {
            case SkillType.BasicAttack:
                effectiveValue = target.TakeDamage(activeUnit.BasicAttack(), skill.skillDamageType);
                AddEventToCombatLog($"{activeUnit.UnitName} uses its Basic Attack and deals {effectiveValue} {skill.skillDamageType} damage to {target.UnitName}!");
                break;
            case SkillType.Damage:
                effectiveValue = target.TakeDamage((int)(skill.skillPower * activeUnit.SkillMultiplier()), skill.skillDamageType);
                AddEventToCombatLog($"{activeUnit.UnitName} uses {skill.skillName} and deals {effectiveValue} {skill.skillDamageType} damage to {target.UnitName}!");
                break;
            case SkillType.Healing:
                effectiveValue = (int)(skill.skillPower * activeUnit.SkillMultiplier());
                target.Heal(effectiveValue);
                AddEventToCombatLog($"{activeUnit.UnitName} uses {skill.skillName} and restores {effectiveValue} health to {target.UnitName}!");
                break;
        }
        // Reduce MP of the active unit if the skill requires it
        activeUnit.ReduceMP(skill.skillCost);
        // Display damage or healing effects on the target unit
        DisplaySkillEffect(target, effectiveValue, skill);
    }

    private void UpdateBattleUI()
    {
        // Update UI elements related to the active and target units
        battleMenuManager.UpdateUnitStatsPanelText(activeUnit);
        battleMenuManager.UpdateUnitStatsPanelText(targetUnit);
    }

    private void CheckBattleStatus()
    {
        bool allEnemiesAreDead = true;
        bool allPlayerAreDead = true;

        for (int i = 0; i < activeUnits.Count; i++)
        {
            if (activeUnits[i].IsDead)
            {
                AddEventToCombatLog(activeUnits[i].UnitName + " is unconscious!");
                // Destroy(activeUnits[i].gameObject);
                activeUnits.Remove(activeUnits[i]);
            }
            else
            {
                if (activeUnits[i].IsPlayerUnit) allPlayerAreDead = false;
                else allEnemiesAreDead = false;
            }
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
        unitDamageText.SetDamage(effectiveValue, skill.skillType);
        Instantiate(skill.skillSFX, new Vector3(target.transform.position.x, target.transform.position.y + 0.6f, target.transform.position.z), target.transform.rotation);
    }
}
