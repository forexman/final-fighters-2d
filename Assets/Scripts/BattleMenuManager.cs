using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleMenuManager : MonoBehaviour
{
    // Serialized fields for UI elements
    [SerializeField] private GameObject bottomPanel, combatLogPanel, actionPanel, descriptionPanel;
    [SerializeField] private GameObject partyPanels, playerPartyPanel, enemyPartyPanel;
    [SerializeField] private GameObject unitStatsPanel, enemyUnitStatsPanel, initiativePanel;
    [SerializeField] private GameObject magicPanel, skillPanel, MPPanel, unitCurrentMP, skillMPCost;
    [SerializeField] private GameObject skillButtonPrefab;

    // Dictionary to keep track of unit stats panels
    private Dictionary<UnitBase, GameObject> unitStatsPanels = new Dictionary<UnitBase, GameObject>();

    private void Start()
    {
        EnableUnitPanel();
    }

    // Activates the unit panel UI elements
    public void EnableUnitPanel()
    {
        SetPanelActiveStates(action: true, magic: false, description: false, unitPanel: true, bottom: true, log: true, initiative: true);
    }

    // Activates the magic panel UI elements
    public void EnableMagicPanel()
    {
        SetPanelActiveStates(action: false, magic: true, description: true, unitPanel: false, bottom: true, log: true, initiative: true);
        UpdateMPDisplay();
        skillMPCost.GetComponent<TextMeshProUGUI>().text = " ";
    }

    // Disables Combat UI
    public void DisableCombatUI()
    {
        SetPanelActiveStates(action: false, magic: false, description: false, unitPanel: false, bottom: false, log: false, initiative: false);
    }

    // Adds a unit stats panel for a given unit
    public void AddUnitStatsPanel(UnitBase unit)
    {
        GameObject statsPanelPrefab = unit.IsPlayerUnit ? unitStatsPanel : enemyUnitStatsPanel;
        GameObject uStatsPanel = Instantiate(statsPanelPrefab);
        uStatsPanel.GetComponent<UnitStatMenu>().UpdateText(unit);

        Transform parentPanel = unit.IsPlayerUnit ? playerPartyPanel.transform : enemyPartyPanel.transform;
        uStatsPanel.transform.SetParent(parentPanel, false);

        unitStatsPanels[unit] = uStatsPanel;
    }

    // Updates the text in a unit's stats panel
    public void UpdateUnitStatsPanelText(UnitBase unit)
    {
        GameObject statsPanelPrefab = unit.IsPlayerUnit ? unitStatsPanel : enemyUnitStatsPanel;

        if (unitStatsPanels.TryGetValue(unit, out GameObject uStatsPanel))
        {
            uStatsPanel.GetComponent<UnitStatMenu>().UpdateText(unit);
        }
        else
        {
            Debug.LogError($"No {statsPanelPrefab.name} found for the specified unit.");
        }
    }

    // Configures UI for player's turn
    public void PlayerTurn()
    {
        EnableUnitPanel();
        actionPanel.SetActive(true);
    }

    // Configures UI for enemy's turn
    public void EnemyTurn()
    {
        EnableUnitPanel();
        actionPanel.SetActive(false);
    }

    // Populates the magic panel with skills
    public void PopulateMagicPanel(List<Skill> classSkills)
    {
        ClearChildren(skillPanel.transform);

        foreach (Skill skill in classSkills)
        {
            GameObject skillButton = Instantiate(skillButtonPrefab, skillPanel.transform);
            skillButton.GetComponent<SkillButton>().Initialize(skill);
        }
    }

    // Sets the description text
    public void SetDescription(string message)
    {
        descriptionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
    }

    // Initiates a basic attack
    public void BasicAttack()
    {
        BattleManager.instance.PlayerBasicAttack();
    }

    // Updates skill description and MP cost display based on mouse hover
    public void MouseOverSkill(Skill skill, bool isButtonMouseOvered)
    {
        TextMeshProUGUI skillMPCostText = skillMPCost.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = descriptionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (isButtonMouseOvered)
        {
            skillMPCostText.text = skill.SkillCost.ToString();
            descriptionText.text = skill.SkillDescription;
        }
        else
        {
            skillMPCostText.text = " ";
            descriptionText.text = " ";
        }
    }

    // Sets the initiative panel with the names of active units
    internal void SetInitiativePanel(List<UnitBase> activeUnits)
    {
        TextMeshProUGUI initiativeText = initiativePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        initiativeText.text = FormatInitiativeText(activeUnits);
    }

    // Helper function to set active states of various panels
    private void SetPanelActiveStates(bool action, bool magic, bool description, bool unitPanel, bool bottom, bool log, bool initiative)
    {
        actionPanel.SetActive(action);
        magicPanel.SetActive(magic);
        descriptionPanel.SetActive(description);
        partyPanels.SetActive(unitPanel);
        bottomPanel.SetActive(bottom);
        combatLogPanel.SetActive(log);
        initiativePanel.SetActive(initiative);
    }

    // Updates the MP display
    private void UpdateMPDisplay()
    {
        UnitBase activeUnit = BattleManager.instance.ActiveUnit;
        unitCurrentMP.GetComponent<TextMeshProUGUI>().text = $"{activeUnit.CurrentMP}/{activeUnit.MaxMP}";
    }

    // Clears all child elements of a given transform
    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    // Formats the initiative text for display
    private string FormatInitiativeText(List<UnitBase> activeUnits)
    {
        string initiativeText = "";
        foreach (UnitBase unit in activeUnits)
        {
            initiativeText += unit.IsPlaying ? $"<color=#ff0000><b>{unit.UnitName}</b></color>, " : $"{unit.UnitName}, ";
        }
        // Removing trailing comma and space
        initiativeText = initiativeText.TrimEnd(',', ' ');
        return initiativeText;
    }
}
