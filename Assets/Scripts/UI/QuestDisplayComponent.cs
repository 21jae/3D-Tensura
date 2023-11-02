using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplayComponent : MonoBehaviour
{
    public Quest Quest;

    [SerializeField]
    private Image nodeImage;
    [SerializeField]
    private Text nodeTitle;
    [SerializeField]
    private Text nodeText;
    [SerializeField]
    private QuestManager questManager;

    private QuestNode currentNode;

    public void Initialize(Quest quest)
    {
        Quest = quest;
        ProgressNode();
    }

    public void ProgressNode()
    {
        currentNode = Quest.ProgressQuest();

        if (currentNode == null)
        {
            gameObject.SetActive(false);
            return;
        }

        nodeImage.sprite = questManager.GetIcon(currentNode.NodeImage);

        nodeTitle.text = currentNode.NodeTitle;
        nodeText.text = currentNode.NodeText;

        if (nodeImage.sprite == null)
        {
            nodeImage.gameObject.SetActive(false);
        }
    }
}