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

    public void ClickButtonAttack()
    {
        _playerController.OnClick();
    }

    public void OnPointerDown()
    {
        isPointerDown = true;
        StartCoroutine(ClickCoroutine());
    }

    public void OnPointerUp()
    {
        isPointerDown = false;
        StopCoroutine(ClickCoroutine());
    }

    IEnumerator ClickCoroutine()
    {
        while (isPointerDown)
        {
            Debug.Log("Å¬¸¯Áß");
            _playerController.OnClick();
            yield return new WaitForSeconds(clickInterval);
        }
    }
}
