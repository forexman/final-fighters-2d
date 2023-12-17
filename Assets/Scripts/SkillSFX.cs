using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSFX : MonoBehaviour
{
    [SerializeField] float effectDuration = 1f;
    [SerializeField] int idSoundSFX;

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.PlaySFX(idSoundSFX);
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(effectDuration);
        Destroy(gameObject);
    }
}
