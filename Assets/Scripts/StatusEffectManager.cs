using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager
{
    private UnitBase unit;
    private List<IStatusEffect> statusEffects;

    public StatusEffectManager(UnitBase unit)
    {
        this.unit = unit;
        statusEffects = new List<IStatusEffect>();
    }

    public void Apply(IStatusEffect effect)
    {
        // Apply logic
    }

    public void Remove(IStatusEffect effect)
    {
        // Apply logic
    }

    // Other methods...
}
