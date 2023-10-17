using UnityEngine;

public class WeaponEvent : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;

    public void Start()
    {
        weaponCollider.enabled = false; //������ ��Ȱ��ȭ
    }

    public void StartSwing()
    {
        weaponCollider.enabled = true;  //�ֵθ� �� Ȱ��ȭ
    }

    public void EndSwing()
    {
        weaponCollider.enabled = false; //������ ������ ��Ȱ��ȭ
    }

}
