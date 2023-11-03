using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; } //�׼� �� ����ü

    private void Awake()
    {
        InputActions = new PlayerInputActions();
        PlayerActions = InputActions.Player;
    }

    private void OnEnable() //���� ��ü�� Ȱ��ȭ���������� Ȱ��ȭ ��Ȱ��ȭ
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}

// Input System Action �ݹ� �ý���
// Input System���� Started / Performed / Canceled�� �ִ�. ȣ��Ǵ� �ñ�� ��� �ٸ���.
// Started => Ű�� �ѹ� ��������
// Performed => ��� ��������
// Canceld => Ű�� ���������