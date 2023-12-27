using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HighlightPlus;
using TMPro;
using UnityEngine;

public class UnitStatMenu : MonoBehaviour
{
    // Serialized fields for UI text components
    [SerializeField] private TMP_Text unitNameText, unitHPText, unitMPText;

    // Reference to the unit this menu is associated with
    private UnitBase _unit;

    /// <summary>
    /// Updates the UI text elements with the unit's information.
    /// </summary>
    /// <param name="unit">The unit whose information is to be displayed.</param>
    public void UpdateText(UnitBase unit)
    {
        _unit = unit;
        unitNameText.text = unit.UnitName;
        unitHPText.text = $"{unit.CurrentHP}/\t{unit.MaxHP}";

        // Update MP text only for player units
        if (unit.IsPlayerUnit)
        {
            unitMPText.text = unit.CurrentMP.ToString();
        }
    }

    /// <summary>
    /// Highlights the associated unit.
    /// </summary>
    public void HighlightUnit()
    {
        HighlightManager.instance.SelectObject(_unit.transform);
    }

    /// <summary>
    /// Removes highlight from the associated unit.
    /// </summary>
    public void RemoveHighlightUnit()
    {
        HighlightManager.instance.UnselectObject(_unit.transform);
    }

    /// <summary>
    /// Sets the associated unit as the target for the active player's selected skill.
    /// </summary>
    public void SelectSkillTargetUnit()
    {
        // Only allow player units to select a target unit
        if (ServiceLocator.Instance.GetService<IBattleManager>().ActiveUnit.IsPlayerUnit)
        {
            ServiceLocator.Instance.GetService<IBattleManager>().PlayerSelectTargetUnit(_unit);
        }
    }
}
