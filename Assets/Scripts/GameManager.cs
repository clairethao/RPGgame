using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static int currentStage = 0;
    public PlayerInfo player = new PlayerInfo(PlayerInfo.playerType.Akai);
    public GameObject gameUI;
    GameObject gameCanvas;

    private void Awake()
    {
        GameObject[] t = GameObject.FindGameObjectsWithTag("gameManager");
        if (t.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        //GameObject gameCanvas;
        if(SceneManager.GetActiveScene().name == "level1")
        {
            gameCanvas = Instantiate(gameUI);
            gameCanvas.SetActive(true);
            Debug.Log("Canvas created: " + gameCanvas.name);
            Debug.Log("Canvas active: " + gameCanvas.activeSelf);
            gameCanvas.name = "Canvas";
            //gameCanvas.transform.parent = gameObject.transform;
            gameCanvas.transform.SetParent(gameObject.transform, false);
        }
        else
        {
            {
                gameCanvas = GameObject.Find("Canvas");
            }
        }

        //if (SceneManager.GetActiveScene().name == "level1") gameCanvas.SetActive(false);
        if (SceneManager.GetActiveScene().name != "level1") gameCanvas.SetActive(true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetStage(int newStage)
    {
        currentStage = newStage;
    }

    public void IncreaseStage(int increment)
    {
        currentStage += increment;
    }

    public int GetStage()
    {
        return (currentStage);
    }

    public void LoadNewScene()
    {
        string nextScene = "level" + (GetStage() + 1);
        SceneManager.LoadScene(nextScene);
    }
}
