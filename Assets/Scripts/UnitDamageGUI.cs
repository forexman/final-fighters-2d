using System.Collections;
using TMPro;
using UnityEngine;


public class UnitDamageGUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] float lifetime = 2f, moveSpeed = 1f, textVibration = 0.1f;

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    void Update()
    {
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void SetValueGUI(int damageAmount, ISkillEffect effect)
    {
        bool isHealingEffect = effect is HealingEffect;
        if (isHealingEffect)
        {
            damageText.text = "<color=#006400><b>" + damageAmount + "</b></color>";
        }
        else
        {
            damageText.text = "<color=#880808><b>" + damageAmount + "</b></color>";
        }

        float jitter = Random.Range(-textVibration, +textVibration);
        transform.position += new Vector3(jitter, jitter, 0f);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
