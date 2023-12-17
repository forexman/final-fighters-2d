using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Database", menuName = "Skill Database")]
public class SkillDatabase : ScriptableObject
{
    public List<Skill> skills = new List<Skill>();
}
