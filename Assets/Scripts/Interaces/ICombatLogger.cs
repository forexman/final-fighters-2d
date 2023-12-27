public interface ICombatLogger
{
    void AddEventToCombatLog(string message, bool additive = true);
}