using UnityEngine;

public class LayerCollisionManager : MonoBehaviour
{
    void Start()
    {
        int layer1 = LayerMask.NameToLayer("Enemy");
        int layer2 = LayerMask.NameToLayer("Enemy");

        Physics.IgnoreLayerCollision(layer1, layer2, true);

        Debug.Log("충돌 방지 시작");
    }

}
