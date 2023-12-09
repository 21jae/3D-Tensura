using Cinemachine;
using DialogueEditor;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Transform playerLook; // 플레이어에 붙어 있는 PlayerLook 오브젝트의 Transform 참조
    [SerializeField] private Transform npcTransform; // NPC의 Transform 참조

    private bool isPlayerInRange = false;
    private Transform originalFollow;
    private Transform originalLookAt;
    private Vector3 originalTrackedObjectOffset;
    private float originalCameraDistance;


    private void Start()
    {
        ConversationManager.OnConversationEnded += RestoreCameraState;

        // 원래의 카메라 상태를 저장합니다.
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

    private void OnDestroy()
    {
        ConversationManager.OnConversationEnded -= RestoreCameraState;
    }
    private void RestoreCameraState()
    {
        // 카메라를 원래 상태로 복원합니다.
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
            vcam.Follow = playerLook;
            vcam.LookAt = npcTransform;

            var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 1.1f, 0);
                framingTransposer.m_CameraDistance = 2.8f;
            }
            else
                Debug.LogError("CinemachineFramingTransposer component not found on the virtual camera.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ConversationManager.Instance.StartConversation(myConversation);
                AdjustVirtualCamera();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}
