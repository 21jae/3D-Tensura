using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    public Material transparentMaterial; // ������ ��Ƽ����
    private Material originalMaterial; // ������ ��Ƽ����

    private void Start()
    {
        // �ǹ��� ���� ��Ƽ������ �����մϴ�.
        originalMaterial = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� �浹�ߴ��� Ȯ��
        {
            // �ǹ��� ��Ƽ������ ���������� ����
            GetComponent<Renderer>().material = transparentMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ������ ������� Ȯ��
        {
            // �ǹ��� ��Ƽ������ ������� ����
            GetComponent<Renderer>().material = originalMaterial;
        }
    }
}
