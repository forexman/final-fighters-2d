using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Responsibility: Its primary responsibility is to set up the scene, instantiate necessary prefabs, and establish the initial connections between them.
    SOLID Principles: It adheres to the Single Responsibility Principle by focusing only on setting up the battle scene.
*/
public class BattleSetup : MonoBehaviour
{
    public BattleManager battleManagerPrefab;
    public BattleMenu battleMenuPrefab;
    public CombatLog combatLogPrefab;
    public AIManager aiManagerPrefab;

    private void Awake()
    {
        // var battleMenu = Instantiate(battleMenuPrefab);
        // var combatLog = Instantiate(combatLogPrefab);
        // var aiManager = Instantiate(aiManagerPrefab);
        // var battleManager = Instantiate(battleManagerPrefab);

        var battleMenu = battleMenuPrefab;
        var combatLog = combatLogPrefab;
        var aiManager = aiManagerPrefab;
        var battleManager = battleManagerPrefab;
        var skillManager = SkillManager.Instance;
        skillManager.Initialize(combatLog);
        battleManager.Initialize(battleMenu, combatLog, aiManager);
        battleMenu.Initialize(battleManagerPrefab);
        aiManager.Initialize(battleManagerPrefab);
    }
}
