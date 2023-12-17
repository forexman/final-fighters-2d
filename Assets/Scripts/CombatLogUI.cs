using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLogUI : MonoBehaviour
{
    [SerializeField] TMP_Text combatLog;
    
    public void UpdateText(string message, bool add)
    {
        if(add) combatLog.text += "\n" + message;
        else combatLog.text = message;
    }
}
