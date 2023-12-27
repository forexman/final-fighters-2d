using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStatus : IStatusEffect
{
    private string buffType;
    private int amount;
    public int Duration { get; set; }
    public string Type => buffType;

    public BuffStatus(string buffType, int amount, int duration)
    {
        this.buffType = buffType;
        this.amount = amount;
        Duration = duration;
    }

    public void ApplyStatus(UnitBase unit)
    {
        switch (buffType)
        {
            case "EvasionUp":
                unit.evasion += amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion went up!");
                break;
            case "AccuracyUp":
                unit.accuracy += amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Accuracy went up!");
                break;
            case "DefenseUp":
                unit.damageReduction += amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Defense went up!");
                break;
                // Add more cases as needed
        }
    }

    public void RemoveStatus(UnitBase unit)
    {
        switch (buffType)
        {
            case "EvasionUp":
                unit.evasion -= amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion is back to normal.");
                break;
            case "AccuracyUp":
                unit.accuracy -= amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion is back to normal.");
                break;
            case "DefenseUp":
                unit.damageReduction -= amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Defense is back to normal.");
                break;
        }
    }

    public void UpdateStatus(UnitBase unit)
    {
        if (Duration > 0) Duration--;
        else RemoveStatus(unit);
    }
}
