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

    private void Start()
    {
        // IBattleManager battleManager = Instantiate(battleManagerPrefab);
        // IBattleMenu battleMenu = Instantiate(battleMenuPrefab);
        battleManagerPrefab.Initialize(battleMenuPrefab, combatLogPrefab, aiManagerPrefab);
        battleMenuPrefab.Initialize(battleManagerPrefab);
        aiManagerPrefab.Initialize(battleManagerPrefab);
        ServiceLocator.Instance.RegisterService<IBattleManager>(battleManagerPrefab);
        ServiceLocator.Instance.RegisterService<IBattleMenu>(battleMenuPrefab);
        ServiceLocator.Instance.RegisterService<ICombatLogger>(combatLogPrefab);
        ServiceLocator.Instance.RegisterService<IAIManager>(aiManagerPrefab);
    }
}
