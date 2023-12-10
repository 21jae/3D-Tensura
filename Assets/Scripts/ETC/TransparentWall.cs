using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    public Material transparentMaterial; // 반투명 머티리얼
    private Material originalMaterial; // 원래의 머티리얼

    private void Start()
    {
        // 건물의 원래 머티리얼을 저장합니다.
        originalMaterial = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {
            // 건물의 머티리얼을 반투명으로 변경
            GetComponent<Renderer>().material = transparentMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 영역을 벗어났는지 확인
        {
            // 건물의 머티리얼을 원래대로 복구
            GetComponent<Renderer>().material = originalMaterial;
        }
    }
}
