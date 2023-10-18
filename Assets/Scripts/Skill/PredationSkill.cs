using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    public GameObject predationSkillPos;
    public GameObject predationPrefab;

    public void ActivateSkill()
    {
        ActivatePredation();
    }

    private void ActivatePredation()
    {
        Instantiate(predationPrefab, predationSkillPos.transform.position, transform.rotation);
    }
}
