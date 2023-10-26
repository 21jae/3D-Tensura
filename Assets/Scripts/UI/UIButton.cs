using System.Collections;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    private PlayerController _playerController;

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
