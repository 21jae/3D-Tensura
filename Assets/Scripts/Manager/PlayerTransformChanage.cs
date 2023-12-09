using System;
using System.Collections;
using UnityEngine;

public class PlayerTransformChanage : MonoBehaviour
{
    [SerializeField] private Transform dungeon;
    [SerializeField] private GameObject player;

    //dengeon위치로 이동
    private void MoveToDungeon()
    {
        if (dungeon != null)
        {
            player.transform.position = dungeon.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("태그");
            StartCoroutine(ChangeMapAndBGM());
        }
    }

    private IEnumerator ChangeMapAndBGM()
    {
        SoundManager.Instance.StopMusic();
        MoveToDungeon();

        yield return CoroutineHelper.WaitForSeconds(0.5f);
        SoundManager.Instance.PlayBGMBattleSound();
    }
}
