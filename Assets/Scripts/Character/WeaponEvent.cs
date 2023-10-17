using UnityEngine;

public class WeaponEvent : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;

    public void Start()
    {
        weaponCollider.enabled = false; //시작은 비활성화
    }

    public void StartSwing()
    {
        weaponCollider.enabled = true;  //휘두를 떄 활성화
    }

    public void EndSwing()
    {
        weaponCollider.enabled = false; //동작이 끝나면 비활성화
    }

}
