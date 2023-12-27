using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLog : MonoBehaviour, ICombatLogger
{
    [SerializeField] private List<string> combatLogFile;
    [SerializeField] TMP_Text combatLogText;

    public void AddEventToCombatLog(string msg, bool additive = true)
    {
        combatLogFile.Add(msg);
        UpdateText(msg, additive);
    }

    private void UpdateText(string message, bool additive)
    {
        if(additive) combatLogText.text += "\n" + message;
        else combatLogText.text = message;
    }

}