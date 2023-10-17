using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private Transform target = null;
    private GameObject targetPlayer;
    [SerializeField] private float moveSpeed = 0.5f;


    void Start()
    {
        targetPlayer = GameObject.FindWithTag("Player");

        if (targetPlayer != null)
        {
            target = targetPlayer.transform;
        }
    }

    private void LateUpdate()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);

        Vector3 changePosition = new Vector3(transform.position.x, target.position.y + 1, transform.position.z);

        transform.position = changePosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //효과음 추가
            Destroy(this.gameObject);
        }
    }
}
