using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; } //액션 맵 구조체

    private void Awake()
    {
        InputActions = new PlayerInputActions();
        PlayerActions = InputActions.Player;
    }

    private void OnEnable() //게임 개체가 활성화되지않으면 활성화 비활성화
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}

// Input System Action 콜백 시스템
// Input System에는 Started / Performed / Canceled가 있다. 호출되는 시기는 모두 다르다.
// Started => 키를 한번 눌렀을때
// Performed => 계속 눌렀을때
// Canceld => 키를 떼어냈을때