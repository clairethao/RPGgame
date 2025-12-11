using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml;
using UnityEngine.SceneManagement;

public class QuestSystem : MonoBehaviour
{
    int currentStage;
    bool timerStarted;
    float timer;
    List<string> actions, targets, xps;
    List<bool> objectiveAchieved;

    string stageTitle, stageDescription, stageObjectives, startingpointForPlayer;
    bool panelDisplayed;
    float displayTimer;
    bool startDisplayTimer;

    int nbObjectivesAchieved = 0;
    int nbObjectivesToAchieve;
    int XPAchieved;

    public enum possibleActions { do_nothing = 0, talk_to = 1, acquire_a = 2, destroy_one = 3, enter_place_called = 4 };
    List<possibleActions> actionsForQuest;

    public GameObject stagePanel, stageTitleText, stageDescriptionText, stageObjectivesText;
    [SerializeField] GameObject player;

    void Start()
    {
        Debug.Log(">>> QuestSystem.Start() called");
        Init();
        MovePlayerToStartingPoint();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) { panelDisplayed = !panelDisplayed; Display(panelDisplayed); }
        if (startDisplayTimer)
        {
            displayTimer += Time.deltaTime;
            if (displayTimer >= 2)
            {
                displayTimer = 0.0f;
                startDisplayTimer = false;
                GameObject.Find("UserMessage").GetComponent<Text>().text = "";
            }
        }
    }

    void Display(bool display)
    {

        stagePanel.SetActive(display);
    }

    public void Init()
    {
        stageTitleText = GameObject.Find("stageTitle");
        stageObjectivesText = GameObject.Find("stageObjectives");
        stageDescriptionText = GameObject.Find("stageDescription");
        stagePanel = GameObject.Find("stagePanel");

        currentStage = GetComponent<GameManager>().GetStage();
        nbObjectivesAchieved = 0;

        actions = new List<string>();
        targets = new List<string>();
        xps = new List<string>();
        objectiveAchieved = new List<bool>();
        actionsForQuest = new List<possibleActions>();
        //LoadQuest();
        LoadQuest2();
        DisplayQuestInfo();
    }

    public void LoadQuest()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("quest");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        stageObjectives = "For this stage, you need to:\n";

        foreach (XmlNode stage in doc.SelectNodes("quest/stage"))
        {
            if (stage.Attributes.GetNamedItem("id").Value == "" + currentStage)
            {
                stageTitle = stage.Attributes.GetNamedItem("name").Value;
                stageDescription = stage.Attributes.GetNamedItem("description").Value;

                foreach (XmlNode results in stage)
                {
                    print("For this stage you need to:\n");
                    foreach (XmlNode result in results)
                    {
                        string action = result.Attributes.GetNamedItem("action").Value;
                        string target = result.Attributes.GetNamedItem("target").Value;
                        string xp = result.Attributes.GetNamedItem("xp").Value;

                        actions.Add(action);
                        targets.Add(target);
                        xps.Add(xp);
                        objectiveAchieved.Add(false);
                        print(action + " " + target + "[" + xp + "XP]");
                        stageObjectives += "\n -> " + action + " " + target + "[" + xp + "XP]";
                        nbObjectivesToAchieve++;
                    }


                }


            }


        }
    }

    public void LoadQuest2()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("quest");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        stageObjectives = "For this stage, you need to:\n";

        foreach (XmlNode stage in doc.SelectNodes("quest/stage"))
        {
            if (stage.Attributes.GetNamedItem("id").Value == "" + currentStage)
            {
                stageTitle = stage.Attributes.GetNamedItem("name").Value;
                stageDescription = stage.Attributes.GetNamedItem("description").Value;

                foreach (XmlNode results in stage)
                {
                    print("For this stage you need to:\n");
                    foreach (XmlNode result in results)
                    {
                        string action = result.Attributes.GetNamedItem("action").Value;
                        string target = result.Attributes.GetNamedItem("target").Value;
                        string xp = result.Attributes.GetNamedItem("xp").Value;
                        possibleActions actionForQuest = possibleActions.do_nothing;

                        if (action.IndexOf("Acquire") >= 0) actionForQuest = possibleActions.acquire_a;
                        else if (action.IndexOf("Talk") >= 0) actionForQuest = possibleActions.talk_to;
                        else if (action.IndexOf("Destroy") >= 0 && action.IndexOf("one") >= 0) actionForQuest = possibleActions.destroy_one;
                        else if (action.IndexOf("Enter") >= 0 && action.IndexOf("place") >= 0) actionForQuest = possibleActions.enter_place_called;

                        actionsForQuest.Add(actionForQuest);
                        targets.Add(target);
                        xps.Add(xp);
                        objectiveAchieved.Add(false);
                        print(action + " " + target + "[" + xp + "XP]");
                        stageObjectives += "\n -> " + action + " " + target + "[" + xp + "XP]";
                        nbObjectivesToAchieve++;
                    }


                }


            }


        }
    }

    void MovePlayerToStartingPoint()
    {
        GameObject p;

        if (SceneManager.GetActiveScene().name == "level1")
        {
            p = Instantiate(player);
            p.SetActive(true);
            Debug.Log("Player instantiated: " + p.name + ", active=" + p.activeSelf);
            p.name = "Player";

            p.transform.position = GameObject.Find("startingPoint").transform.position;
            p.transform.rotation = new Quaternion(0, 0, 0, 0);
            p.transform.rotation = Quaternion.identity;
            //p.transform.parent = gameObject.transform;
        }
        else
        {
            p = GameObject.Find("Player");
            p.transform.position = GameObject.Find("startingPoint").transform.position;
        }
        Camera cam = p.GetComponentInChildren<Camera>();
        if (cam != null)
            Debug.Log("Player camera found: " + cam.name);
        else
            Debug.LogError("No camera found on Player prefab!");
        Camera camm = p.GetComponentInChildren<Camera>();
        if (cam != null)
        {
            // Force this camera to be the active renderer
            camm.enabled = true;
            camm.tag = "MainCamera";       // make it the primary camera
            camm.targetDisplay = 0;         // Display 1
            camm.clearFlags = CameraClearFlags.Skybox; // or SolidColor for quick visibility
            camm.depth = 0;

            Debug.Log("Camera forced active: enabled=" + camm.enabled +
                      ", tag=" + camm.tag +
                      ", targetDisplay=" + camm.targetDisplay +
                      ", clearFlags=" + camm.clearFlags +
                      ", depth=" + camm.depth);
        }
        else
        {
            Debug.LogError("No camera found on Player prefab! Creating a fallback camera.");
            var camGO = new GameObject("FallbackCamera");
            var fallback = camGO.AddComponent<Camera>();
            fallback.tag = "MainCamera";
            fallback.targetDisplay = 0;
            fallback.clearFlags = CameraClearFlags.SolidColor;
            fallback.backgroundColor = Color.black;
            camGO.transform.position = p.transform.position + new Vector3(0, 2f, -6f);
            camGO.transform.LookAt(p.transform);

            Debug.Log("Fallback camera created and set as MainCamera.");
        }
    }

    void DisplayQuestInfo()
    {
        stageTitleText.GetComponent<Text>().text = stageTitle;
        stageDescriptionText.GetComponent<Text>().text = stageDescription;
        stageObjectivesText.GetComponent<Text>().text = stageObjectives + "\n Press H to Hide/Display this information";
    }

    public void Notify(possibleActions action, string target)
    {
        print("Notified: Action=" + actions + " Target=" + target);
        for (int i = 0; i < actionsForQuest.Count; i++)
        {
            if (action == actionsForQuest[i] && target == targets[i] && !objectiveAchieved[i])
            {
                DisplayMessage("+" + xps[i] + "XP");
                nbObjectivesAchieved++; XPAchieved += Int32.Parse(xps[i]);
                objectiveAchieved[i] = true;
            }
        }
        if (nbObjectivesAchieved >= 1) //for testing
        {
            DisplayMessage("Stage Complete");
            GetComponent<GameManager>().player.XP = CalculateTotalXPForLevel();
            Invoke("StageComplete", 2);
        }
    }

    int CalculateTotalXPForLevel()
    {
        int totalXP = 0;
        for (int i = 0; i < actionsForQuest.Count; i++)
        {
            totalXP += Int32.Parse(xps[i]);
        }
        return totalXP;
    }

    void DisplayMessage(string message)
    {
        GameObject.Find("UserMessage").GetComponent<Text>().text = message; startDisplayTimer = true;
    }

    void StageComplete()
    {
        var gm = GetComponent<GameManager>();
        gm.IncreaseStage(1);   // increment here

        if (SceneManager.GetActiveScene().name != "level3")
            SceneManager.LoadScene("levelComplete");
        else
            SceneManager.LoadScene("endScene");
    }


}

