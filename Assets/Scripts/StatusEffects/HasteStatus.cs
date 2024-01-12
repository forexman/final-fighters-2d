using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteStatus : IStatusEffect
{
    public string Type => "Haste";
    public int Duration { get; set; }
    private BattleManager battleManager;

    public void SetDependencies(BattleManager battleManager)
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
        unit.ActionsPerTurn++;
    }

    public void RemoveStatus(UnitBase unit)
    {
        battleManager.AddEventToCombatLog($"{unit.UnitName} is no longer under the effects of Haste.");
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
