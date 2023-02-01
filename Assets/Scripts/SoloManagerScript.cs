using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SoloManagerScript : MonoBehaviour
{
    private Player player;
    public GameObject winPanel;
    private EventSystem es;
    public GameObject PlayAgainButton;
    public bool gameRunning = true;
    private GameObject curtain;
    public GameObject rallyText;
    public GameObject highScoreText;
    public GameObject newText;
    private bool isHighScore = false;
    public GameObject primaryText;

    // Start is called before the first frame update
    void Start()
    {
        curtain = GameObject.FindGameObjectWithTag("FadeCurtain");
        player = ReInput.players.GetPlayer(DataManagerScript.gamepadControllingMenus);
        es = EventSystem.current;
    }

    public void TurnOnMenu()
    {
        winPanel.SetActive(true);
        if (JoystickLayerManager.Instance != null)
        {
            JoystickLayerManager.Instance.AssignPlayerToEventSystem(DataManagerScript.gamepadControllingMenus);
            es.SetSelectedGameObject(PlayAgainButton);
        }
        gameRunning = false;
        rallyText.GetComponent<Text>().text = "YOUR SCORE: " + DataManagerScript.rallyCount.ToString();

        // Determine if high score
        DetermineIfHighScore();
    }

    public void DetermineIfHighScore()
    {
        int highScore = FileSystemLayer.Instance.soloRallyModeHighScore;
       
        if (DataManagerScript.rallyCount > highScore)
        {
            FileSystemLayer.Instance.soloRallyModeHighScore = DataManagerScript.rallyCount;
            FileSystemLayer.Instance.SavePref("soloRallyModeHighScore", DataManagerScript.rallyCount);
            highScore = DataManagerScript.rallyCount;
            isHighScore = true;
            newText.SetActive(true);
            primaryText.GetComponent<Text>().text = "WELL DONE!";

        }
        highScoreText.GetComponent<Text>().text = "HIGH SCORE: " + highScore;
    }
    public void PlayAgain()
    {
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("soloGameScene"); });   
    }

    public void QuitToTitle()
    {
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("consoleTitle"); });
    }
}
