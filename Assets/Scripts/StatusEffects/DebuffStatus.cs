public class DebuffStatus : IStatusEffect
{
    public string Type => debuffType;
    private string debuffType;
    private int amount;
    public int Duration { get; set; }

    public DebuffStatus(string debuffType, int amount, int duration)
    {
        this.debuffType = debuffType;
        this.amount = amount;
        Duration = duration;
    }

    public void ApplyStatus(UnitBase unit)
    {
        // Logic to apply the debuff to the unit
        // This might involve reducing a stat or applying a negative effect
        // Example: unit.DecreaseAttribute(debuffType, amount);
        switch (debuffType)
        {
            case "EvasionDown":
                unit.evasion -= amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion went down.");
                break;
            case "AccuracyDown":
                unit.accuracy -= amount;
                break;
            case "DefenseDown":
                unit.damageReduction -= amount;
                break;
                // Add more cases as needed
        }
    }

    public void RemoveStatus(UnitBase unit)
    {
        // Logic to remove the debuff from the unit
        // Example: unit.RestoreAttribute(debuffType, amount);
        switch (debuffType)
        {
            case "EvasionDown":
                unit.evasion += amount;
                BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion is back to normal.");
                break;
            case "AccuracyDown":
                unit.accuracy += amount;
                break;
                // Add more cases as needed
        }
    }

    public void UpdateStatus(UnitBase unit)
    {
        // Logic to update the debuff status; typically reduce duration
        if (Duration > 0)
            Duration--;
        else
            RemoveStatus(unit);
    }

}
