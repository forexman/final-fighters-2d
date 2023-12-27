using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a button in the UI for selecting a skill.
/// </summary>
public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI skillNameText;

    private Skill skill;

    /// <summary>
    /// Initializes the skill button with a given skill.
    /// </summary>
    /// <param name="newSkill">The skill to associate with this button.</param>
    public void Initialize(Skill newSkill)
    {
        skill = newSkill;
        skillNameText.text = newSkill.SkillName;

        // Configuring the button's click behavior
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(SelectSkillIfPossible);
    }

    /// <summary>
    /// Selects the skill if the active unit can use it.
    /// </summary>
    private void SelectSkillIfPossible()
    {
        if (BattleManager.instance.ActiveUnit.CanUseSkill(skill))
        {
            BattleManager.instance.SkillSelection(skill);
        }
    }

    /// <summary>
    /// Handles the mouse pointer entering the button area.
    /// </summary>
    /// <param name="eventData">Event data for the pointer event.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        BattleManager.instance.BattleMenuManager.MouseOverSkill(skill, true);
    }

    /// <summary>
    /// Handles the mouse pointer exiting the button area.
    /// </summary>
    /// <param name="eventData">Event data for the pointer event.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.instance.BattleMenuManager.MouseOverSkill(skill, false);
    }
}
