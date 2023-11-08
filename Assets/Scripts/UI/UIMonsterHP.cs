using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMonsterHP : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private FieldEnemy enemy;
    [SerializeField] private AnimationCurve hpChangeCurve;
    [SerializeField] private float lerpSpeed = 3f;
    [SerializeField] private GameObject damagePrefab;
    [SerializeField] private Transform canvasTransform;

    private Camera mainCamera;
    private float targetHpPercentage;
    public static UIMonsterHP Instance { get; private set; }  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;
    }

    private void Start()
    {
        targetHpPercentage = enemy.characterStatManager.currentData.currentHP / enemy.characterStatManager.currentData.currentMaxHP;
        slider.value = targetHpPercentage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            enemy.characterStatManager.currentData.currentHP -= 150f;
        }

        HandleHp();

        slider.value = Mathf.Lerp(slider.value, targetHpPercentage, lerpSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }

    public Vector3 MonsterPosition => enemy.transform.position;

    private void HandleHp()
    {
        targetHpPercentage = enemy.characterStatManager.currentData.currentHP / enemy.characterStatManager.currentData.currentMaxHP;
    }

    public void CreateDamagePopup(float damageAmount, Vector3 monsterWorldPosition)
    {
        Vector3 monsterScreenPosition = Camera.main.WorldToScreenPoint(monsterWorldPosition);
        monsterScreenPosition.y += 50;

        RectTransform canvasRectTransform = canvasTransform as RectTransform;
        float canvasHalfWidth = canvasRectTransform.sizeDelta.x * 0.5f;
        float canvasHalfHeight = canvasRectTransform.sizeDelta.y * 0.5f;

        monsterScreenPosition.x -= canvasHalfWidth;
        monsterScreenPosition.y -= canvasHalfHeight;

        GameObject damagePopupInstance = Instantiate(damagePrefab, canvasTransform);
        damagePopupInstance.GetComponent<RectTransform>().anchoredPosition = monsterScreenPosition;
        damagePopupInstance.GetComponent<TextMeshProUGUI>().text = damageAmount.ToString();

        StartCoroutine(MoveAndFadeOut(damagePopupInstance));
    }

    private IEnumerator MoveAndFadeOut(GameObject popupInstance)
    {
        const float DURATION = 1f; // �� ȿ�� ���� �ð�
        float timer = 0;

        Vector3 initialPosition = popupInstance.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 0.3f, 0); // ���� 50 ���� �����Դϴ�

        TextMeshProUGUI textComponent = popupInstance.GetComponent<TextMeshProUGUI>();
        Color initialColor = textComponent.color;

        while (timer < DURATION)
        {
            float progress = timer / DURATION;

            // �˾� ��ġ ������Ʈ
            popupInstance.transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);

            // �ؽ�Ʈ �÷� ���İ�(����) ������Ʈ
            Color currentColor = textComponent.color;
            currentColor.a = Mathf.Lerp(initialColor.a, 0, progress);
            textComponent.color = currentColor;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(popupInstance);
    }
}
