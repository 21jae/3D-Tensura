using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Quest
{
    [XmlArray("Nodes")]
    [XmlArrayItem("Node")]
    public List<QuestNode> Nodes;

    [XmlAttribute("QuestTitle")]
    public string QuestTitle;

    [System.NonSerialized]
    private QuestNode currentNode;

    [System.NonSerialized]
    private int position;

    public QuestNode ProgressQuest()
    {
        if (HasNextQuestNode())
        {
            currentNode = Nodes[position++];
            return currentNode;
        }
        return null;
    }

    private bool HasNextQuestNode()
    {
        return Nodes != null && position < Nodes.Count;
    }

    public static Quest LoadQuest(string fileName)
    {
        var serializer = new XmlSerializer(typeof(Quest));
        var stream = new FileStream(fileName, FileMode.Open);
        var quest = serializer.Deserialize(stream) as Quest;
        stream.Close();

        return quest;
    }
}