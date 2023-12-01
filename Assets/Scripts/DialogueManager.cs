using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class DialogueData
{
    public Dialogue[] dialogues;
}

[Serializable]
public class Dialogue
{
    public int id;
    public string speaker;
    public string text;
    public Choice[] choices;
    public int mood; // 0 = normal, 1 = shocked, 2 = sad, 3 = Happy
}

[Serializable]
public class Choice
{
    public int id;
    public string text;
    public int nextDialogueID;
}

[Serializable]
public enum Mood
{
    Normal,
    Shocked,
    Sad,
    Happy
}

[Serializable]
public class MoodSystem
{
    public Mood mood;
    public Texture2D portrait;
}

[Serializable]
public class SpeakerData
{
    public string nameInDialogue;
    public List<MoodSystem> portrait;
    public string nameToPrint;
}

public class DialogueManager : UIParent
{
    public static event Action OnEndOfDialogue; 
    
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Texture2D backgroundDialogue;
    [SerializeField] private string dialogueFileName;
    [SerializeField] private string labelTextname;
    [SerializeField] private string[] namesButtons;
    [SerializeField] private SpeakerData[] dataSpeakers;

    private DialogueData dialogueData;
    private VisualElement _root;
    private VisualElement backgroundDialogueElement;
    private int currentDialogueIndex;
    private List<Button> choiceButtons = new();
    private Label dialogueText;
    private Dictionary<string, SpeakerData> speakers = new();
    private VisualElement portrait;
    private Label speakerName;
    private bool skipPrint = false;


    public void Init()
    {
        currentDialogueIndex = 0;
        InitPanels();
        InitButtons();
        InitOthers();
        LoadDialogueData();
        StartCoroutine(DisplayDialogue());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipPrint = true;
        }
    }

    void LoadDialogueData()
    {
        string filePath = Application.dataPath + "/" + dialogueFileName + ".json";

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            dialogueData = JsonUtility.FromJson<DialogueData>(dataAsJson);
        }
        else
        {
            Debug.LogError("Fichier JSON introuvable !");
        }
    }

    IEnumerator DisplayDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogError("DialogueData est null !");
            yield break;
        }

        skipPrint = false;

        if (currentDialogueIndex < dialogueData.dialogues.Length)
        {
            Dialogue currentDialogue = dialogueData.dialogues[currentDialogueIndex];
            if (currentDialogue.text.Contains("%Login%"))
            {
                currentDialogue.text = currentDialogue.text.Replace("%Login%", Options.login);
            }

            portrait.style.backgroundImage = new StyleBackground(speakers[currentDialogue.speaker].portrait
                .Find(x => x.mood == (Mood)currentDialogue.mood).portrait);
            speakerName.text = speakers[currentDialogue.speaker].nameToPrint;

            foreach (var t in choiceButtons)
            {
                t.style.display = DisplayStyle.None;
            }

            string[] words = currentDialogue.text.Split(' ');
            dialogueText.text = "";
            foreach (var word in words)
            {
                dialogueText.text += word + " ";
                if (!skipPrint)
                    yield return new WaitForSeconds(0.1f);
            }
            //dialogueText.text = currentDialogue.text;

            for (int i = 0; i < currentDialogue.choices.Length; i++)
            {
                choiceButtons[i].style.display = DisplayStyle.Flex;
                choiceButtons[i].text = currentDialogue.choices[i].text;

                int index = i;
                choiceButtons[i].UnregisterCallback<ClickEvent, int>(OnChoiceSelected);
                choiceButtons[i].RegisterCallback<ClickEvent, int>(OnChoiceSelected, index);
            }
        }

        else
        {
            Debug.Log("Fin du dialogue");
        }
    }

    void OnChoiceSelected(ClickEvent evt, int choiceIndex)
    {
        Dialogue currentDialogue = dialogueData.dialogues[currentDialogueIndex];
        int nextDialogueID = currentDialogue.choices[choiceIndex].nextDialogueID;
        currentDialogueIndex = -1;
        for (int i = 0; i < dialogueData.dialogues.Length; i++)
        {
            if (dialogueData.dialogues[i].id == nextDialogueID)
            {
                currentDialogueIndex = i;
                break;
            }
        }

        if (currentDialogueIndex == -1)
        {
            EndOfDialogue();
            return;
        }
        StartCoroutine(DisplayDialogue());
    }
    
    private void EndOfDialogue()
    {
        Debug.Log("Fin du dialogue");
        _root.style.display = DisplayStyle.None;
        OnEndOfDialogue?.Invoke();
    }

    protected override void InitPanels()
    {
        _root = _uiDocument.rootVisualElement;
        dialogueText = _root.Q<Label>(labelTextname);
        backgroundDialogueElement = _root.Q<VisualElement>("Background");
        backgroundDialogueElement.style.backgroundImage = new StyleBackground(backgroundDialogue);
    }

    protected override void InitButtons()
    {
        foreach (var btn in namesButtons)
        {
            var button = _root.Q<Button>(btn);
            choiceButtons.Add(button);
        }
    }

    protected override void InitOthers()
    {
        foreach (var speaker in dataSpeakers)
        {
            speakers.Add(speaker.nameInDialogue, speaker);
        }

        portrait = _root.Q<VisualElement>("PictureSpeaker");
        speakerName = _root.Q<Label>("SpeakerName");
    }

    public override void UpdateLanguage(string language)
    {
    }
}