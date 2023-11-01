using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class UIChange : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player1ModeUI;
    public GameObject player2ModeUI;

    public Animator playerAnimator;
    public CinemachineVirtualCamera virtualCamera;

    private const string EFFECT_TAG = "CharacterChangeEffect";


    private void Awake()
    {
    }

    private void Start()
    {
        player1.SetActive(true);
        player2.SetActive(false);
        //player1ModeUI.SetActive(true);
        //player2ModeUI.SetActive(false);
        UpdateUI(true);
    }

    private void UpdateUI(bool isPlayerAction)
    {
        player1ModeUI.SetActive(isPlayerAction);
        player2ModeUI.SetActive(!isPlayerAction);
    }

    public void ChangeCharacter()
    {
        if (player1.activeSelf)
        {
            StartCoroutine(SwitchCharacter(player1, player2));
            //player1ModeUI.SetActive(false);
            //player2ModeUI.SetActive(true);
        }
        else if (player2.activeSelf)
        {
            //player1ModeUI.SetActive(true);
            //player2ModeUI.SetActive(false);
            StartCoroutine(SwitchCharacter(player2, player1));
        }
    }

    private IEnumerator SwitchCharacter(GameObject fromPlayer, GameObject toPlayer)
    {
        GameObject effect = ObjectPool.instance.GetPooledObject(EFFECT_TAG);

        if (effect != null)
        {
            effect.transform.position = fromPlayer.transform.position;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);

            StartCoroutine(ReturnObjectToPool(effect, 1f));
        }

        yield return new WaitForSeconds(0.5f);

        toPlayer.transform.position = fromPlayer.transform.position;
        toPlayer.transform.rotation = fromPlayer.transform.rotation;

        fromPlayer.SetActive(false);
        toPlayer.SetActive(true);

        virtualCamera.Follow = toPlayer.transform;

        UpdateCameraSetting(toPlayer);

        UpdateUI(toPlayer == player1);

        playerAnimator.SetTrigger("ChangeFrom");

    }

    private IEnumerator ReturnObjectToPool(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        ObjectPool.instance.ReturnObjectToPool(EFFECT_TAG, effect);
    }

    private void UpdateCameraSetting(GameObject currentPlayer)
    {
        var framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (currentPlayer == player2)
        {
            framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 2f, 0f);
            framingTransposer.m_CameraDistance = 4.5f;
        }
        else if (currentPlayer == player1)
        {
            framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 0.3f, 0f);
            framingTransposer.m_CameraDistance = 3f;
        }
    }

}
