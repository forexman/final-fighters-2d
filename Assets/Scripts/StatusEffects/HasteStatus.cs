using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteStatus : IStatusEffect
{
    public string Type => "Haste";
    public int Duration { get; set; }
    private IBattleManager battleManager;

    public void SetDependencies(IBattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    
    public HasteStatus(int duration)
    {
        Duration = duration;
    }

    public void ApplyStatus(UnitBase unit)
    {
        battleManager.AddEventToCombatLog($"{unit.UnitName} is Hastened!");
        unit.HasHaste = true;
        unit.ActionsPerTurn++;
    }

    public void RemoveStatus(UnitBase unit)
    {
        battleManager.AddEventToCombatLog($"{unit.UnitName} is no longer under the effects of Haste.");
        unit.HasHaste = false;
        unit.ActionsPerTurn--;
    }

    public void UpdateStatus(UnitBase unit)
    {
        // Update logic, e.g., decrement duration
        if (Duration > 0)
            Duration--;
        else
            RemoveStatus(unit);
    }
}
