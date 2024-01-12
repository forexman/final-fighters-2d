using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillMetadata[] skillMetadataList;
    [SerializeField] private List<Skill> skillList = new List<Skill>();
    private SkillFactory skillFactory;
    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Handle the case where the instance doesn't exist
                // This can be creating a new GameObject with SkillManager attached
                // or handling it some other way
            }
            return instance;
        }
    }

    public SkillMetadata[] SkillMetadataList
    {
        get { return skillMetadataList; }
        protected set { skillMetadataList = value; }
    }

    public List<Skill> SkillList
    {
        get { return skillList; }
        protected set { skillList = value; }
    }

    // void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //         LoadSkillsFromJSON();
    //         InitializeSkills();
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Initialize(ICombatLogger combatLogger)
    {
        skillFactory = new SkillFactory(combatLogger);
        LoadSkillsFromJSON();
        InitializeSkills();
    }

    private void LoadSkillsFromJSON()
    {
        TextAsset skillData = Resources.Load<TextAsset>("skills");
        SkillData loadedData = JsonUtility.FromJson<SkillData>(skillData.text);
        skillMetadataList = loadedData.skills;
    }

    private void InitializeSkills()
    {
        foreach (var metadata in skillMetadataList)
        {
            skillList.Add(skillFactory.CreateSkill(metadata));
        }
    }

    public List<Skill> GetClassSpecificSkills(int classID)
    {
        List<Skill> classSkills = new List<Skill>();
        foreach (Skill skill in SkillManager.Instance.SkillList)
        {
            if (Array.Exists(skill.UnitID, id => id == classID) && skill.Id != 0)
            {
                classSkills.Add(skill);
            }
        }
        return classSkills;
    }
}

[System.Serializable]
public class SkillData
{
    public SkillMetadata[] skills;
}
