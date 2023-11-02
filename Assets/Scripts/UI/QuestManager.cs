using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Sprite> iconDict;

    [SerializeField]
    private Sprite[] icons;

    [SerializeField]
    private GameObject questDialog;

    public int range = 3;
    public GameObject npcPosition;
    public GameObject myPosition;

    private string SampleQuest = "Assets/Resources/Quests/SampleQuest.xml";
    void Start()
    {
        questDialog.SetActive(false);
        LoadIconsFromSprites();
    }

    private void LoadIconsFromSprites()
    {
        iconDict = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in icons)
        {
            iconDict.Add(sprite.name, sprite);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && Vector3.Distance(myPosition.transform.position, npcPosition.transform.position) < range)
        {
            StartQuest(SampleQuest);
        }
    }

    public void StartQuest(string questFilePath)
    {
        questDialog.SetActive(true);
        questDialog.GetComponent<QuestDisplayComponent>().Initialize(Quest.LoadQuest(questFilePath));
    }

    public Sprite GetIcon(string iconName)
    {
        if (iconDict[iconName] != null)
            return iconDict[iconName];
        else
            return null;
    }
}
