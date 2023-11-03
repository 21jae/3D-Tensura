using Cinemachine;
using System;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float defaultDistance = 6f;   //카메라 대상간의 기본 거리
    [SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;   //최소 거리
    [SerializeField] [Range(0f, 10f)] private float maximumDistance = 6f;   //최대 거리

    //거리 Lerp를 평활화 하는 값
    [SerializeField][Range(0f, 10f)] private float smoothing = 4f;          //줌 부드럽게
    [SerializeField][Range(0f, 10f)] private float zoomSensitivity = 1f;    //줌 속도 조절

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineInputProvider inputProvider;

    private float currentTargetDistance;    

    private void Awake()
    {
        //Body는 Chinemachine의 구성요소의 일부기 때문에 GetComponent CinemachineVirtualCamera.
        framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        inputProvider = GetComponent<CinemachineInputProvider>();
        currentTargetDistance = defaultDistance;
    }

    private void Update()
    {
        Zoom();   
    }

    private void Zoom()
    {
        float ZoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;  //GetAxisValue(2) : 입력장치 세 번째 축인 마우스 휠 값에 zoomSensitivity 곱해 타겟 거리 조절

        currentTargetDistance = Mathf.Clamp(currentTargetDistance + ZoomValue, minimumDistance, maximumDistance);   //타겟 거리 mimum ~ maximum 사이로 제한

        float currentDistance = framingTransposer.m_CameraDistance;

        if (currentDistance == currentTargetDistance)   //목표 거리 도달시 아무것도 작동X
        {
            return;
        }

        float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime); //현재 카메라와 타겟 거리 사이를 부드럽게 보간
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}
