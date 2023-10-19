using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;
    public GameObject TestPrefab;


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null )
        {
            Debug.Log("��Ʈ�ѷ� ����");
        }
    }

    public void ActivateSkill()
    {
        Instantiate(TestPrefab, transform.position, Quaternion.identity);

    }

}
