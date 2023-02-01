using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class PauseManagerScript : MonoBehaviour
{
    public bool paused = false;
    public bool recentlyPaused = false;
    public GameObject pausePanel;
    public EventSystem es;
    private Player player;
    
    public AudioClip unpauseSound;
    public AudioClip pauseSound;
    
	public GameObject highlight;

    public bool isInputLocked = false;
    void Start()
    {
		highlight = GameObject.Find("Highlight");
    }

    public void Pause(int joystick)
    {
        int gamepadIndex = joystick - 1;
        player = ReInput.players.GetPlayer(gamepadIndex);

        if (!paused)
        {
            // Show pause
            pausePanel.SetActive(true);
            pausePanel.BroadcastMessage("ChangeActivePlayer", gamepadIndex);

            // Assign butons
            if (JoystickLayerManager.Instance != null)
            {
                JoystickLayerManager.Instance.AssignPlayerToEventSystem(gamepadIndex);
            }
           
            // Reset menu
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(es.firstSelectedGameObject);

            // SFX
            MusicManagerScript.Instance.TurnOffEverything();
            SoundManagerScript.instance.PlaySingle(pauseSound);
            // SoundManagerScript.instance.muteSFX();
            //TODO: Move the ball's SFX to sound manager script. Also, will this work with multiple balls? Maybe broadcast pause to everything?
            GameObject ball = GameObject.FindWithTag("Ball");
            if (ball != null) { 
            ball.GetComponent<BallScript>().Pause();
            }

            // Pause time
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void UnpauseAndQuit()
    {
        isInputLocked = true;
        Unpause();
 
        LeanTween.alpha(GameObject.Find("FadeCurtainCanvas").GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => {
            if (GameObject.Find("ChallengeManager"))
            {
                SceneManager.LoadSceneAsync("chooseChallengeScene");
            }
            else
            {
                SceneManager.LoadSceneAsync("consoleTitle");
            }
        });
    }

    public void UnpauseAndRestartChallenge()
    {
        isInputLocked = true;
        Unpause();
        LeanTween.alpha(GameObject.Find("FadeCurtainCanvas").GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("challengeScene");});
    }
       
    public void UnPauseAndQuitSoloMode()
    {
        isInputLocked = true;
        Unpause();
        LeanTween.alpha(GameObject.Find("FadeCurtainCanvas").GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("consoleTitle"); });
    }

   
    public void Unpause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
            pausePanel.SetActive(false);
            recentlyPaused = true;

            Debug.Log(highlight);
            if (highlight) {
                highlight.transform.position = new Vector3(-1000f, -1000f, 0);
            }

            // SFX
            // SoundManagerScript.instance.unMuteSFX();
            MusicManagerScript.Instance.RestoreFromPause();
            //TODO: Move the ball's SFX to sound manager script
            GameObject ball = GameObject.FindWithTag("Ball"); 
            if (ball != null)
            {
                ball.GetComponent<BallScript>().UnPause();
            }
			SoundManagerScript.instance.PlaySingle(unpauseSound);

            Invoke("CancelRecentlyPaused", 0.1f);
        }
    }

    public void CancelRecentlyPaused()
    {
        recentlyPaused = false;
    }
}
