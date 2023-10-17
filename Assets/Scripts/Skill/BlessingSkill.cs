using UnityEngine;

public class BlessingSkill : MonoBehaviour
{
    public GameObject blessingPrefab;

    public void ActivateBless()
    {
        Instantiate(blessingPrefab, transform.position, Quaternion.identity);
    }
}
