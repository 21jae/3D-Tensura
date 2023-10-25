using UnityEngine;

public class MegidoRay : MonoBehaviour
{
    public void MoveToWards(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
