using Cinemachine;
using System;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float defaultDistance = 6f;   //ī�޶� ����� �⺻ �Ÿ�
    [SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;   //�ּ� �Ÿ�
    [SerializeField] [Range(0f, 10f)] private float maximumDistance = 6f;   //�ִ� �Ÿ�

    //�Ÿ� Lerp�� ��Ȱȭ �ϴ� ��
    [SerializeField][Range(0f, 10f)] private float smoothing = 4f;          //�� �ε巴��
    [SerializeField][Range(0f, 10f)] private float zoomSensitivity = 1f;    //�� �ӵ� ����

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineInputProvider inputProvider;

    private float currentTargetDistance;    

    private void Awake()
    {
        //Body�� Chinemachine�� ��������� �Ϻα� ������ GetComponent CinemachineVirtualCamera.
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
        float ZoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;  //GetAxisValue(2) : �Է���ġ �� ��° ���� ���콺 �� ���� zoomSensitivity ���� Ÿ�� �Ÿ� ����

        currentTargetDistance = Mathf.Clamp(currentTargetDistance + ZoomValue, minimumDistance, maximumDistance);   //Ÿ�� �Ÿ� mimum ~ maximum ���̷� ����

        float currentDistance = framingTransposer.m_CameraDistance;

        if (currentDistance == currentTargetDistance)   //��ǥ �Ÿ� ���޽� �ƹ��͵� �۵�X
        {
            return;
        }

        float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime); //���� ī�޶�� Ÿ�� �Ÿ� ���̸� �ε巴�� ����
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}
