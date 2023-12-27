using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    public BattleManager battleManagerPrefab; // Set this in the Unity Editor
    public BattleMenuManager battleMenuManagerPrefab; // Set this in the Unity Editor

    private void Start()
    {
        IBattleManager battleManager = Instantiate(battleManagerPrefab);
        IBattleMenu battleMenuManager = Instantiate(battleMenuManagerPrefab);

        var dependentObject = FindObjectOfType<DependentObject>();
        dependentObject.Initialize(battleManager);

        // Do this for other dependencies as well
    }
}
