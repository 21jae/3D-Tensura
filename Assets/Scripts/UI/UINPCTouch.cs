using DialogueEditor;
using UnityEngine;

public class UINPCTouch : MonoBehaviour
{
    //���콺 ��ġ�� Dialog���
    [SerializeField] private NPCConversation myConversation;
    public NPCAnim npcAnim;

    public void OnDialogText()
    {
        ConversationManager.Instance.StartConversation(myConversation);
        npcAnim.DialogAnimation();
    }
}
