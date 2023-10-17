using UnityEngine;

public class PredationSkill : MonoBehaviour
{
    public GameObject predationSkillPos;
    public GameObject predationPrefab;

    public void ActivatePredation()
    {
        Instantiate(predationPrefab, predationSkillPos.transform.position, transform.rotation);
    }
}
