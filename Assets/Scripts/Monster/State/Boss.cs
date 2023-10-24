using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private SkillManager skillManager;

    private void Awake()
    {
        skillManager = FindObjectOfType<SkillManager>();

        if (skillManager == null)
        {
            Debug.LogError("SkillManager not found in the scene!");
            return;
        }

        StartCoroutine(ThunderSkillRoutine());
    }

    private IEnumerator ThunderSkillRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            skillManager.ActiveThunderSkill();
        }
    }

}
