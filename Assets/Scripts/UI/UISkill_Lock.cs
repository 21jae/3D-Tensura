using DialogueEditor;
using System;
using UnityEngine;

public class UISkill_Lock : MonoBehaviour
{
    public GameObject skill01;

    public bool isDeterioration = true;

    public void UnLockSkills()
    {
        if (isDeterioration == true)
        {
            skill01.SetActive(true);
        }
    }
}
