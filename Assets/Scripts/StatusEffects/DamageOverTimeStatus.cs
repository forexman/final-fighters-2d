public class DamageOverTimeStatus : IStatusEffect
{
    private int damagePerTurn;
    public int Duration { get; set; }
    public string Type => "Poison";

    public DamageOverTimeStatus(int power, int duration)
    {
        this.damagePerTurn = power;
        Duration = duration;
    }

    public void ApplyStatus(UnitBase unit)
    {
        // Logic for applying DoT effect
        // This might include setting up initial state or marking the unit as affected by DoT
        BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName} is poisoned!");
    }

    public void RemoveStatus(UnitBase unit)
    {
        // Logic for removing DoT effect
        // Cleanup or state reset if needed
        BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName} is no longer poisoned!");

    }

    public void UpdateStatus(UnitBase unit)
    {
        // Logic to apply damage each turn and decrement duration
        if (Duration > 0)
        {
            unit.TakeDamage(damagePerTurn, DamageType.Physical);
            BattleManager.instance.combatLogManager.AddEventToCombatLog($"{unit.UnitName} takes {damagePerTurn} {DamageType.Physical} damage from poison.");
            Duration--;
        }
        else
        {
            RemoveStatus(unit);
        }
    }
}
