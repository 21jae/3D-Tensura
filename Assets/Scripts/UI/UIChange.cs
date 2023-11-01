using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChange : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public Animator player1Animator;
    public Animator player2Animator;

    private void Start()
    {
        player1.SetActive(true);
        player2.SetActive(false);
    }

    public void ChangeCharacter()
    {
        if (player1.activeSelf)
        {
            player1Animator.SetTrigger("ChangeFrom");
            StartCoroutine(SwitchCharacter(player1,player2));

            //player2.transform.position = player1.transform.position;
            //player2.transform.rotation = player1.transform.rotation;

            //player1.SetActive(false);
            //player2.SetActive(true);
        }
        else if (player2.activeSelf)
        {
            player1Animator.SetTrigger("ChangeTo");
            StartCoroutine(SwitchCharacter(player2, player1));

            ////player2이라면 PlayerHumanController에 연결된 animator.SetTrigger("ChangeTo") 실행
            //player1.transform.position = player2.transform.position;
            //player1.transform.rotation = player2.transform.rotation;

            //player1.SetActive(true);
            //player2.SetActive(false);
        }
    }

    private IEnumerator SwitchCharacter(GameObject fromPlayer, GameObject toPlayer)
    {
        yield return new WaitForSeconds(1f);

        toPlayer.transform.position = fromPlayer.transform.position;
        toPlayer.transform.rotation = fromPlayer.transform.rotation;

        fromPlayer.SetActive(false);
        toPlayer.SetActive(true);
    }
}
