public interface IStatusEffect
{
    // Unique identifier for the type of status effect
    string Type { get; }

    // Duration of the status effect
    int Duration { get; set; }
    void ApplyStatus(UnitBase unit);
    void RemoveStatus(UnitBase unit);
    void UpdateStatus(UnitBase unit);
    void SetDependencies(IBattleManager battleManager);
}
