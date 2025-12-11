using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static int currentStage = 0;   // start at 0 for Pattern A
    public PlayerInfo player = new PlayerInfo(PlayerInfo.playerType.Akai);
    public GameObject gameUI;
    GameObject gameCanvas;

    private void Awake()
    {
        GameObject[] t = GameObject.FindGameObjectsWithTag("gameManager");
        if (t.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "level1")
        {
            gameCanvas = Instantiate(gameUI);
            gameCanvas.SetActive(true);
            gameCanvas.name = "Canvas";
            gameCanvas.transform.SetParent(gameObject.transform, false);
        }
        else
        {
            gameCanvas = GameObject.Find("Canvas");
        }

        if (SceneManager.GetActiveScene().name != "level1") gameCanvas.SetActive(true);
    }

    public void SetStage(int newStage) => currentStage = newStage;
    public void IncreaseStage(int increment) => currentStage += increment;
    public int GetStage() => currentStage;

    public void LoadNewScene()
    {
        // Pattern A: use GetStage() + 1
        string nextScene = "level" + (GetStage() + 1);
        SceneManager.LoadScene(nextScene);
    }
}