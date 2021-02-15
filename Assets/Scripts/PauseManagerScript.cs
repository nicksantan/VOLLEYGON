using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
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

    void Start()
    {
		highlight = GameObject.Find("Highlight");
    }

   
    void Update()
    {
        
    }

    public void Pause(int joystick)
    {
        int gamepadIndex = joystick - 1;
        player = ReInput.players.GetPlayer(gamepadIndex);

        if (!paused)
        {
            // Show pause
            pausePanel.SetActive(true);

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
        StartCoroutine("UnpauseAndQuitRoutine");
    }

    public void UnpauseAndRestartChallenge()
    {
        StartCoroutine("UnpauseAndRestartChallengeRoutine");
    }
       
    public IEnumerator UnpauseAndRestartChallengeRoutine()
    {
        Unpause();
        GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("challengeScene");
    }

    public IEnumerator UnpauseAndQuitRoutine()
    {
        Unpause();
        GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
        yield return new WaitForSeconds(0.5f);
        //If this is a challenge, go back to the challenge menu, not the main menu.
        if (GameObject.Find("ChallengeManager"))
        {
            SceneManager.LoadSceneAsync("chooseChallengeScene");
        }
        else
        {
            SceneManager.LoadSceneAsync("titleScene");
        }
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
