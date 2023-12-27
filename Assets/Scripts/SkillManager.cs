using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillMetadata[] skillMetadataList;
    [SerializeField] private List<Skill> skillList = new List<Skill>();
    public static SkillManager Instance { get; private set; }

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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSkillsFromJSON();
            InitializeSkills();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

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
            skillList.Add(SkillFactory.CreateSkill(metadata));
        }
    }
}

[System.Serializable]
public class SkillData
{
    public SkillMetadata[] skills;
}