using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIManager
{
    Skill AIChooseSkill(UnitBase unit);
    List<UnitBase> AIChooseTargets(UnitBase unit, Skill skill);
}
