using DialogueEditor;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ConversationManager.Instance.SetInt("Clear", 10);
        Destroy(gameObject);
    }
}
