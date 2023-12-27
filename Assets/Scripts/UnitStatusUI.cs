using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnitStatusUI : MonoBehaviour
{
    [SerializeField] private TMP_Text unitStatuses;

    // Start is called before the first frame update
    public void UpdateStatusBar(UnitBase unit)
    {
        unitStatuses.text = string.Join("\n", unit.StatusEffects.Select(effect => $"{effect.Type} ({effect.Duration})"));
    }
}
