using System.Collections.Generic;

public interface ISkillEffect
{
    void ApplyEffect(UnitBase source, IEnumerable<UnitBase> targets, Skill skill);
    void SetDependencies(ICombatLogger combatLogger);
}
