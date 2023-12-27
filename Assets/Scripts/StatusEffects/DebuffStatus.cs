public class DebuffStatus : IStatusEffect
{
    public string Type => debuffType;
    private string debuffType;
    private int amount;
    public int Duration { get; set; }
    private IBattleManager battleManager;

    public void SetDependencies(IBattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    
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
                battleManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion went down.");
                break;
            case "AccuracyDown":
                unit.accuracy -= amount;
                battleManager.AddEventToCombatLog($"{unit.UnitName}'s Accuracy went down.");
                break;
            case "DefenseDown":
                unit.damageReduction -= amount;
                battleManager.AddEventToCombatLog($"{unit.UnitName}'s Defense went down.");
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
                battleManager.AddEventToCombatLog($"{unit.UnitName}'s Evasion is back to normal.");
                break;
            case "AccuracyDown":
                unit.accuracy += amount;
                battleManager.AddEventToCombatLog($"{unit.UnitName}'s Accuracy is back to normal.");
                break;
            case "DefenseDown":
                unit.damageReduction += amount;
                battleManager.AddEventToCombatLog($"{unit.UnitName}'s Defense is back to normal.");
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
