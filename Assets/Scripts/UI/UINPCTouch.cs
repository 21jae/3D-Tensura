using DialogueEditor;
using UnityEngine;

public class UINPCTouch : MonoBehaviour
{
    //마우스 터치시 Dialog출력
    [SerializeField] private NPCConversation myConversation;
    public NPCAnim npcAnim;

    public void OnDialogText()
    {
        ConversationManager.Instance.StartConversation(myConversation);
        npcAnim.DialogAnimation();
    }
}
