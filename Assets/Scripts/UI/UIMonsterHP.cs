using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMonsterHP : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Enemy enemy;
    [SerializeField] private AnimationCurve hpChangeCurve;
    [SerializeField] private float lerpSpeed = 3f;

    private Camera mainCamera;
    private float targetHpPercentage;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        targetHpPercentage = enemy.characterStatManager.currentHP / enemy.characterStatManager.currentMaxHP;
        slider.value = targetHpPercentage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            enemy.characterStatManager.currentHP -= 150f;
        }

        HandleHp();

        slider.value = Mathf.Lerp(slider.value, targetHpPercentage, lerpSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }

    private void HandleHp()
    {
        targetHpPercentage = enemy.characterStatManager.currentHP / enemy.characterStatManager.currentMaxHP;
    }
}
