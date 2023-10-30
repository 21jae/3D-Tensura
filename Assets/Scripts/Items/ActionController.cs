using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Text actionText;
    [SerializeField] private UIInventory inventory;
    public PlayerSlimeController slimeController;
    private bool pickupActivated;
    private RaycastHit hitInfo;


    private void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PredationObject();
            PickupItemsInExpandRange();
            CheckItem();
            CanPickUp();
        }
    }

    private void PickupItemsInExpandRange()
    {
        //float expandRange = range * 2f;
    }

    private void PredationObject()
    {
        StartCoroutine(GrowAndShirink());
    }

    private IEnumerator GrowAndShirink()
    {
        //포식시 플레이어의 크기가 2배 증가
        slimeController.transform.localScale *= 2;

        float duration = 0.5f;
        float magnitude = 0.1f;

        Vector3 originalPosition = slimeController.transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = originalPosition.x + UnityEngine.Random.Range(-0.6f, 0.6f) * magnitude;
            float y = originalPosition.y + UnityEngine.Random.Range(-0.6f, 0.6f) * magnitude;
            float z = originalPosition.z + UnityEngine.Random.Range(-0.6f, 0.6f) * magnitude;

            slimeController.transform.position = new Vector3(x, y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        slimeController.transform.position = originalPosition;

        yield return new WaitForSeconds(0.8f);
        slimeController.transform.localScale /= 2;
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "위장 보관완료");
                inventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }

        else
        {
            InfoDisappear();
        }
    }


    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 ";

    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }


}
