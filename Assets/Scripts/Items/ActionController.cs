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
    }

    public void TryAction()
    {
        PredationObject();
        PickupItemsInExpandRange();
    }

    private void PickupItemsInExpandRange()
    {
        float expandRange = range * 2f;

        Collider[] hitColliders = Physics.OverlapSphere(slimeController.transform.position, expandRange, layerMask);

        foreach (var hitColl in hitColliders)
        {
            if (hitColl.CompareTag("Item"))
            {
                ItemPickUp itemPickUp = hitColl.GetComponent<ItemPickUp>();
                if (itemPickUp != null)
                {
                    Debug.Log(hitColl.transform.GetComponent<ItemPickUp>().item.itemName + " À§Àå º¸°ü¿Ï·á");
                    inventory.AcquireItem(itemPickUp.item);
                    Destroy(hitColl.gameObject);
                    InfoDisappear();
                }
            }
        }
    }

    private void PredationObject()
    {
        StartCoroutine(GrowAndShirink());
    }

    private IEnumerator GrowAndShirink()
    {
        slimeController.transform.localScale *= 2;

        float duration = 0.4f;
        float magnitude = 0.1f;

        Vector3 originalPosition = slimeController.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = originalPosition.x + UnityEngine.Random.Range(-0.8f, 0.8f) * magnitude;
            float y = originalPosition.y + UnityEngine.Random.Range(-0.8f, 0.8f) * magnitude;
            float z = originalPosition.z + UnityEngine.Random.Range(-0.8f, 0.8f) * magnitude;

            slimeController.transform.position = new Vector3(x, y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        slimeController.transform.position = originalPosition;

        yield return CoroutineHelper.WaitForSeconds(0.8f);
        slimeController.transform.localScale /= 2;
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
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " È¹µæ ";

    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
