using Cinemachine;
using DialogueEditor;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Transform playerLook; // �÷��̾ �پ� �ִ� PlayerLook ������Ʈ�� Transform ����
    [SerializeField] private Transform npcTransform; // NPC�� Transform ����

    private bool isPlayerInRange = false;
    private Transform originalFollow;
    private Transform originalLookAt;
    private Vector3 originalTrackedObjectOffset;
    private float originalCameraDistance;


    private void Start()
    {
        ConversationManager.OnConversationEnded += RestoreCameraState;

        // ������ ī�޶� ���¸� �����մϴ�.
        if (vcam != null)
        {
            originalFollow = vcam.Follow;
            originalLookAt = vcam.LookAt;
            var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                originalTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
                originalCameraDistance = framingTransposer.m_CameraDistance;
            }
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            ConversationManager.Instance.StartConversation(myConversation);
            AdjustVirtualCamera(); // ����� ī�޶� ���� �Լ� ȣ��
        }
    }
    private void OnDestroy()
    {
        // �̺�Ʈ �����ʸ� �����մϴ�.
        ConversationManager.OnConversationEnded -= RestoreCameraState;
    }
    private void RestoreCameraState()
    {
        // ī�޶� ���� ���·� �����մϴ�.
        if (vcam != null)
        {
            vcam.Follow = originalFollow;
            vcam.LookAt = originalLookAt;
            var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset = originalTrackedObjectOffset;
                framingTransposer.m_CameraDistance = originalCameraDistance;
            }
        }
    }
    private void AdjustVirtualCamera()
    {
        if (vcam != null)
        {
            // ����� ī�޶��� 'Follow'�� PlayerLook���� ����
            vcam.Follow = playerLook;

            // ����� ī�޶��� 'LookAt'�� NPC�� �����Ͽ� NPC�� �ٶ󺸰� ��
            vcam.LookAt = npcTransform;

            // ī�޶� �Ʒ����� ���� �÷��ٺ��� ������ ����
            var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 0.3f, 0);
                framingTransposer.m_CameraDistance = 3.0f;
            }
            else
            {
                Debug.LogError("CinemachineFramingTransposer component not found on the virtual camera.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }


}