using Cinemachine;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    //[SerializeField] private NPCConversation myConversation;
    //[SerializeField] private CinemachineVirtualCamera vcam;
    //[SerializeField] private Transform playerLook; // 플레이어에 붙어 있는 PlayerLook 오브젝트의 Transform 참조
    //[SerializeField] private Transform npcTransform; // NPC의 Transform 참조

    //private bool isPlayerInRange = false;
    //private Transform originalFollow;
    //private Transform originalLookAt;
    //private Vector3 originalTrackedObjectOffset;
    //private float originalCameraDistance;


    //private void Start()
    //{
    //    ConversationManager.OnConversationEnded += RestoreCameraState;

    //    // 원래의 카메라 상태를 저장합니다.
    //    if (vcam != null)
    //    {
    //        originalFollow = vcam.Follow;
    //        originalLookAt = vcam.LookAt;
    //        var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    //        if (framingTransposer != null)
    //        {
    //            originalTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    //            originalCameraDistance = framingTransposer.m_CameraDistance;
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        ConversationManager.Instance.StartConversation(myConversation);
    //        AdjustVirtualCamera(); // 버츄얼 카메라 조정 함수 호출
    //    }
    //}
    //private void OnDestroy()
    //{
    //    // 이벤트 리스너를 제거합니다.
    //    ConversationManager.OnConversationEnded -= RestoreCameraState;
    //}
    //private void RestoreCameraState()
    //{
    //    // 카메라를 원래 상태로 복원합니다.
    //    if (vcam != null)
    //    {
    //        vcam.Follow = originalFollow;
    //        vcam.LookAt = originalLookAt;
    //        var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    //        if (framingTransposer != null)
    //        {
    //            framingTransposer.m_TrackedObjectOffset = originalTrackedObjectOffset;
    //            framingTransposer.m_CameraDistance = originalCameraDistance;
    //        }
    //    }
    //}
    //private void AdjustVirtualCamera()
    //{
    //    if (vcam != null)
    //    {
    //        // 버츄얼 카메라의 'Follow'를 PlayerLook으로 설정
    //        vcam.Follow = playerLook;

    //        // 버츄얼 카메라의 'LookAt'을 NPC로 설정하여 NPC를 바라보게 함
    //        vcam.LookAt = npcTransform;

    //        // 카메라가 아래에서 위로 올려다보는 각도를 설정
    //        var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    //        if (framingTransposer != null)
    //        {
    //            framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 0.3f, 0);
    //            framingTransposer.m_CameraDistance = 3.0f;
    //        }
    //        else
    //        {
    //            Debug.LogError("CinemachineFramingTransposer component not found on the virtual camera.");
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInRange = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInRange = false;
    //    }
    //}


}
