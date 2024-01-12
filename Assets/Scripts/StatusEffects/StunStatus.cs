public class StunStatus : IStatusEffect
{

    public string Type => "Stun";
    public int Duration { get; set; }
    private BattleManager battleManager;

    public void SetDependencies(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public StunStatus(int duration)
    {
        Duration = duration;
    }

    public void ApplyStatus(UnitBase unit)
    {
        // Apply stun logic
        unit.IsStunned = true;
        battleManager.AddEventToCombatLog($"{unit.UnitName} is Stunned!");
    }

    public void RemoveStatus(UnitBase unit)
    {
        // Remove stun logic
        unit.IsStunned = false;
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
