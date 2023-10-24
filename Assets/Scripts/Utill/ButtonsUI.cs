using System.Collections;
using UnityEngine;

public class ButtonsUI : MonoBehaviour
{
    private PlayerController _playerController;
    private bool isPointerDown;
    private float clickInterval = 0.2f;

    private void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnPointerDown()
    {
        _playerController.StartButtonPress();
    }

    public void OnPointerUp()
    {
        _playerController.EndButtonPress();
    }
}
