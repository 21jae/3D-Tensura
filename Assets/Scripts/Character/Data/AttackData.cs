using UnityEngine;

[System.Serializable]
public class AttackData
{
    [field: SerializeField] public int comboStack = 0;
    [field: SerializeField] public float lastButtonPressTime = 0f;
    [field: SerializeField] public bool isButtonPressed = false;
}
