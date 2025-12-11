using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    bool dialogueisActive = false;
    string nameOfCharacter;
    Dialogue[] dialogues;
    int nbDialogues;
    int currentDialogueIndex = 0;
    bool waitingForUserInput = false;
    GameObject dialogueBox, dialoguePanel;

    void Start()
    {
        dialogueBox = GameObject.Find("dialogueBox");
        dialoguePanel = GameObject.Find("dialoguePanel");

        GameObject imageObject = GameObject.Find("dialogueImage");
        Texture2D characterTexture = Resources.Load<Texture2D>(gameObject.name) as Texture2D;

        GameObject.Find("dialogueImage").GetComponent<RawImage>().texture = Resources.Load<Texture2D>(gameObject.name) as Texture2D;
        dialoguePanel.SetActive(false);

        nameOfCharacter = gameObject.name;
        nbDialogues = calculateNbDialogues();
        dialogues = new Dialogue[nbDialogues];
        
        loadDialogue();
    }

    void Update()
    {
        if (dialogueisActive)
        {
            if (!waitingForUserInput)
            {
                dialoguePanel.SetActive(true);
                if (currentDialogueIndex != -1) displayDialogue2();
                else
                {
                    dialogueisActive = false;
                    dialoguePanel.SetActive(false);
                    waitingForUserInput = false;
                    GameObject.Find("Player").GetComponent<ControlPlayer>().EndTalking();

                    currentDialogueIndex = 0;

                    GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.talk_to, nameOfCharacter);
                }
                waitingForUserInput = true;
            }
            else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                int nextIndex = dialogues[currentDialogueIndex].targetForResponse[0];
                if (nextIndex == -1)
                {
                    currentDialogueIndex = -1;
                    waitingForUserInput = false;
                }
                else
                {
                    currentDialogueIndex = nextIndex;
                    waitingForUserInput = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                int nextIndex = dialogues[currentDialogueIndex].targetForResponse[1];
                if (nextIndex == -1)
                {
                    currentDialogueIndex = -1;
                    waitingForUserInput = false;
                }
                else
                {
                    currentDialogueIndex = nextIndex;
                    waitingForUserInput = false;
                }
            }
        }
        }
    }

    public void loadDialogue()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        int dialogueIndex = 0;

        foreach (XmlNode character in doc.SelectNodes("dialogues/character"))
        {
            if (character.Attributes.GetNamedItem("name").Value == nameOfCharacter)
            {
                dialogueIndex = 0;

                foreach (XmlNode dialogueFromXml in character.SelectNodes("dialogue"))
                {
                    dialogues[dialogueIndex] = new Dialogue();
                    dialogues[dialogueIndex].message = dialogueFromXml.Attributes.GetNamedItem("content").Value;

                    int choiceIndex = 0;

                    dialogues[dialogueIndex].response = new string[2];
                    dialogues[dialogueIndex].targetForResponse = new int[2];

                    foreach (XmlNode choice in dialogueFromXml)
                    {
                        dialogues[dialogueIndex].response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;
                        dialogues[dialogueIndex].targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value);

                        choiceIndex++;
                    }

                    dialogueIndex++;
                }
            }
        }
    }

    public int calculateNbDialogues()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);

        int dialogueIndex = 0;

        foreach (XmlNode character in doc.SelectNodes("dialogues/character"))
        {
            if (character.Attributes.GetNamedItem("name").Value == nameOfCharacter)
            {
                foreach (XmlNode dialogueFromXML in character.SelectNodes("dialogue"))
                {
                    dialogueIndex++;
                }
            }
        }

        return dialogueIndex;
    }

    public void displayDialogue1()
    {
        print(dialogues[currentDialogueIndex].message);
        print("[A]>" + dialogues[currentDialogueIndex].response[0]);
        print("[B]>" + dialogues[currentDialogueIndex].response[1]);

    }

    public void startDialogue()
    {
        currentDialogueIndex = 0;
        waitingForUserInput = false;
        dialogueisActive = true;
    }

    public void displayDialogue2()
    {
        string textToDisplay = "[" + gameObject.name + "]" + "" + dialogues[currentDialogueIndex].message + "\n[A]>" + dialogues[currentDialogueIndex].response[0] + "\n[B]>" + dialogues[currentDialogueIndex].response[1];
        GameObject.Find("dialogueBox").GetComponent<Text>().text = textToDisplay;
    }
}
