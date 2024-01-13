using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Rewired;

#if UNITY_XBOXONE
	using Users;
#endif

public class TitleManagerScript : MonoBehaviour {

    private Player controllingGamepad;
	//private JoystickButtons controllingGamepad;
	private JoystickButtons[] gamepads = new JoystickButtons[4];
    private bool inputAllowed = false;
    public GameObject pressStartAnimation;
    public bool allowQuit = false;

	public Text versionText;
	public GameObject highlightImages;
	public GameObject mainMenuPanel;
	public GameObject singlePlayerPanel;
    public GameObject AIBotPanel;
    public GameObject soloModeButton;
    public GameObject versusAIButton;
    public GameObject oneBotButton;
    public GameObject quitPanel;
    public GameObject quitButton;
    public GameObject resumeButton;
    public GameObject singlePlayerButton;

	public GameObject curtain;
    public GameObject cutsceneManager;

	private bool mainMenuActive = false;

	public EventSystem es1;

	public Button firstButton;
    
	public AudioClip startSound;
	public AudioClip confirmSound;
	public AudioClip cancelSound;

    private Player p1;
    private Player p2;
    private Player p3;
    private Player p4;
    private Player[] players = new Player[4];
    
	public GameObject highlight;

    private void Awake()
    {
        p1 = ReInput.players.GetPlayer(0);
        p2 = ReInput.players.GetPlayer(1);
        p3 = ReInput.players.GetPlayer(2);
        p4 = ReInput.players.GetPlayer(3);

        players[0] = p1;
        players[1] = p2;
        players[2] = p3;
        players[3] = p4;

    }

    void Start() {
        curtain.SetActive(true);
        //curtain.GetComponent<NewFadeScript>().Fade(0f);
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 0f, .5f);
        DataManagerScript.whichTeamWon = 0;
        MusicManagerScript.Instance.FadeOutEverything();
        versionText.text = DataManagerScript.version;
        DataManagerScript.ResetStats();
        DataManagerScript.ResetPlayerTypes();
        DataManagerScript.isChallengeMode = false;
        DataManagerScript.isChallengeMode = false;
        DataManagerScript.isSinglePlayerMode = false;
        DataManagerScript.arenaType = -99;
        
		highlight = GameObject.Find("Highlight");

        // Init controller maps
        for (int i = 0; i < 4; i++) {
            gamepads[i] = new JoystickButtons(i + 1);
        }

        if (DataManagerScript.isFirstPlay)
        {
            Invoke("AllowInput", 4f);
            DataManagerScript.isFirstPlay = false;
        }
        else
        {
            AllowInput();
        }

        DataManagerScript.lastViewedChallenge = 0;
    }

    void AllowInput()
    {
        inputAllowed = true;
    }

    void Update () {
		MusicManagerScript.Instance.FadeOutEverything ();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetButtonDown("Supplementary"))
            {
                Debug.Log("Fire!");
            }

			if (!mainMenuActive) {
                if (inputAllowed) {
                    // Listen for activation
                    if (players[i].GetButtonDown("Grav"))
                    {
                        if (allowQuit)
                        {
                            // Back out to quit menu
                        //   SoundManagerScript.instance.PlaySingle(cancelSound);
                        //  showQuitAppPanel();
                        //   DataManagerScript.gamepadControllingMenus = i;

                       //   if (JoystickLayerManager.Instance != null){
                       //        JoystickLayerManager.Instance.AssignPlayerToEventSystem(i);
                       //    }
                        }
                    }

                    if (players[i].GetButtonDown("Jump")) { //used to check for start here as well
                        SoundManagerScript.instance.PlaySingle(startSound);
                    
                        if (DataManagerScript.demoMode) {

                            // Jump right into lobby if in demo mode
                            DataManagerScript.gamepadControllingMenus = i;
                            //StartMultiplayerGame();
                            StartChallengesGame();

                        }
                        else {

#if UNITY_XBOXONE
                            // Sign in if active player is not associated with this controller
                            if (DataManagerScript.xboxMode && XboxOneInput.GetUserIdForGamepad((uint)gamepadIndex) == 0) {

                                DataManagerScript.shouldActivateMenu = true;
                                UsersManager.RequestSignIn(Users.AccountPickerOptions.None, (ulong)gamepadIndex);

                            }
                            else {
                                // Open main menu with this controller
                                activateMainMenu(gamepadIndex);
                            }
#else
                            // Open main menu with this controller
                            // remove press start animation
                            pressStartAnimation.SetActive(false);
                            int currentGP = i;
                            // NOTE: Prevent user from summoning quit menu while main menu is animating.
                            allowQuit = false;

                            // stop the cutscene manager from loading attract mode
                            cutsceneManager.SetActive(false);
                            LeanTween.move(Camera.main.gameObject, new Vector3(0f, -3.3f, -10f), 0.3f).setOnComplete(()=>activateMainMenu(currentGP)).setEase(LeanTweenType.easeOutQuad);
                        // activateMainMenu(gamepadIndex);
#endif
                        }
                    }
                }                
			}
			else {
#if UNITY_XBOXONE
                // Listen for user change (Y button)
                if (Input.GetButtonDown(gamepads[i].y)) {

                    // Back out and log in again if active player presses Y
                    if (gamepadIndex == DataManagerScript.gamepadControllingMenus) {
                        cancelCurrentMenu(true);
                        DataManagerScript.shouldActivateMenu = true;
                        UsersManager.RequestSignIn(Users.AccountPickerOptions.None, (ulong)gamepadIndex);
                    }
                }
#endif	
			}
		}
        // Listen for cancel
        if (controllingGamepad != null && controllingGamepad.GetButtonDown("Grav"))
        {
            // Debug.Log("cancelling current menu");
            cancelCurrentMenu(false);
        }
    }

    void showQuitAppPanel()
    {
        es1.SetSelectedGameObject(null);
        quitPanel.SetActive(true);
        inputAllowed = false;
        es1.SetSelectedGameObject(quitButton);
        resumeButton.GetComponent<ChangeButtonTextColorScript>().Unhighlight();
    }

    public void allowInputSoon()
    {
        Invoke("AllowInput", .25f);
    }
    public void hideQuitAppPanel()
    {
        quitPanel.SetActive(false); 
        es1.SetSelectedGameObject(null);
        controllingGamepad = null;
        allowInputSoon();
            
        if (highlight) {
            highlight.transform.position = new Vector3(-1000f, -1000f, 0);
        }
        
		SoundManagerScript.instance.PlaySingle(confirmSound);
    }

    public void quitApp()
    {
        Application.Quit();
    }

    void cancelCurrentMenu(bool cancelAll) {
		SoundManagerScript.instance.PlaySingle(cancelSound);

        if (!singlePlayerPanel.activeSelf && !AIBotPanel.activeSelf || cancelAll) {
			// Canceling out of main menu
			mainMenuActive = false;
			mainMenuPanel.SetActive (false);
			singlePlayerPanel.SetActive (false);
            // disallow input for a moment for the title screen to reset
            inputAllowed = false;
            allowInputSoon();
            LeanTween.move(Camera.main.gameObject, new Vector3(0f, 0f, -10f), 0.3f).setEase(LeanTweenType.easeOutQuad);
            pressStartAnimation.SetActive(true);
            pressStartAnimation.GetComponent<PlayAnimationScript>().PlayAnimation();
            controllingGamepad = null;
            
            cutsceneManager.SetActive(true);
            if (highlight) {
                highlight.transform.position = new Vector3(-1000f, -1000f, 0);
            }
            
        } else {
            if (AIBotPanel.activeSelf)
            {
             
                es1.SetSelectedGameObject(null);

                AIBotPanel.SetActive(false);
                singlePlayerPanel.SetActive(true);
                SetUpSinglePlayerMenu();

                // SO stupid but I can't figure out why this button won't turn off otherwise.
                singlePlayerPanel.transform.Find("AIButton").GetComponent<ChangeButtonTextColorScript>().Unhighlight();

            }
            else
            {
                // Cancelling out of single player menu
                es1.SetSelectedGameObject(null);
                es1.SetSelectedGameObject(es1.firstSelectedGameObject);
                singlePlayerPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
            
                 es1.SetSelectedGameObject(null);
                es1.SetSelectedGameObject(es1.firstSelectedGameObject);
                singlePlayerButton.GetComponent<ChangeButtonTextColorScript>().Unhighlight();
            }
		}
	}

	public void activateMainMenu(int gamepad) {
      //  allowQuit = true;
        // Assign gamepad to menus
        // Note: This is a player index (0 index). So 3 means player 4.
        DataManagerScript.gamepadControllingMenus = gamepad;
        // Save "active" user if on xbox
#if UNITY_XBOXONE
        if (DataManagerScript.xboxMode) {
            int userId = XboxOneInput.GetUserIdForGamepad((uint)gamepad);
            DataManagerScript.userControllingMenus = UsersManager.FindUserById(userId);
        }
#endif

		// activate menu and its first button (weird ui thing)
		mainMenuActive = true;
		mainMenuPanel.SetActive (true);

        // Test rumble
        //JoystickLayerManager.Instance.ActivateLargeRumble(gamepad, 15f);

        es1.SetSelectedGameObject(null);
		es1.SetSelectedGameObject(es1.firstSelectedGameObject);

        JoystickLayerManager.Instance.AssignPlayerToEventSystem(gamepad);
     
        // depending on which controller was tagged in, set the input stringes here
        controllingGamepad = players[gamepad];
        //	controllingGamepad = new JoystickButtons (gamepad);
        //	es1.GetComponent<StandaloneInputModule> ().horizontalAxis = controllingGamepad.horizontal;
        //	es1.GetComponent<StandaloneInputModule> ().verticalAxis = controllingGamepad.vertical;
        //	es1.GetComponent<StandaloneInputModule> ().submitButton = controllingGamepad.jump;
        //	es1.GetComponent<StandaloneInputModule> ().cancelButton = controllingGamepad.grav;
    }

	public void SetUpSinglePlayerMenu() {
		SoundManagerScript.instance.PlaySingle(confirmSound);
		es1.SetSelectedGameObject(soloModeButton);
	}

    public void SetUpAIMenu() {
		SoundManagerScript.instance.PlaySingle(confirmSound);
        es1.SetSelectedGameObject(oneBotButton);
    }

    public void StartSoloModeGame()
    {
        SoundManagerScript.instance.PlaySingle(startSound);
        //TODO: This should be a different scene, specifically for choosing ONE shape. For now, just start the game with Square
        DataManagerScript.isSinglePlayerMode = true;
        DataManagerScript.isBotsMode = false;
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("soloGameScene"); });
    
    }

    public void StartMultiplayerGame()
    {
        SoundManagerScript.instance.PlaySingle(startSound);
        DataManagerScript.isSinglePlayerMode = false;
        DataManagerScript.isBotsMode = false;

        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("ChoosePlayerScene"); });
        
      //  curtain..GetComponentInChildren<Image>();
       // SceneManager.LoadSceneAsync ("ChoosePlayerScene");
	}
    public void StartOneBotGame()
    {
        SoundManagerScript.instance.PlaySingle(startSound);
        DataManagerScript.isSinglePlayerMode = false;
        DataManagerScript.isBotsMode = true;
        DataManagerScript.numBots = 2; //TODO: Arcade hack
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("ChoosePlayerScene"); });
    }
    public void StartTwoBotGame()
    {
        SoundManagerScript.instance.PlaySingle(startSound);
        DataManagerScript.isSinglePlayerMode = false;
        DataManagerScript.isBotsMode = true;
        DataManagerScript.numBots = 2;
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("ChoosePlayerScene"); });
    }
    public void StartChallengesGame()
    {
        SoundManagerScript.instance.PlaySingle(startSound);
        DataManagerScript.isSinglePlayerMode = false;
        DataManagerScript.isChallengeMode = true;
        DataManagerScript.isBotsMode = false;
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("ChooseChallengeScene"); });

    }
	public void StartOptionsMenu()
    {
        SoundManagerScript.instance.PlaySingle(startSound);
		LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("OptionsScene"); });
	}
}
