using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLog : MonoBehaviour
{
    [SerializeField] private List<string> combatLogFile;
    [SerializeField] TMP_Text combatLogText;

    public void AddEventToCombatLog(string msg, bool add = true)
    {
        combatLogFile.Add(msg);
        UpdateText(msg, add);
    }

    public void UpdateText(string message, bool add)
    {
        if(add) combatLogText.text += "\n" + message;
        else combatLogText.text = message;
    }

}