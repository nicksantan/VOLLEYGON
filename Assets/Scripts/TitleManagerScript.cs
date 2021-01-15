using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


#if UNITY_XBOXONE
	using Users;
#endif

public class TitleManagerScript : MonoBehaviour {

	private JoystickButtons controllingGamepad;
	private JoystickButtons[] gamepads = new JoystickButtons[4];
    private bool inputAllowed = false;
    public GameObject pressStartAnimation;

	public Text versionText;
	public GameObject mainMenuPanel;
	public GameObject singlePlayerPanel;
	public GameObject soloModeButton;

	public GameObject curtain;

	private bool mainMenuActive = false;

	public EventSystem es1;

	public Button firstButton;

    void Start() {
        curtain.SetActive(true);
        curtain.GetComponent<NewFadeScript>().Fade(0f);
        DataManagerScript.whichTeamWon = 0;
        MusicManagerScript.Instance.FadeOutEverything();
        versionText.text = DataManagerScript.version;
        DataManagerScript.ResetStats();
        DataManagerScript.ResetPlayerTypes();
        DataManagerScript.isChallengeMode = false;
        DataManagerScript.isSinglePlayerMode = false;
        // Init controller maps
        for (int i = 0; i < 4; i++) {
            gamepads[i] = new JoystickButtons(i + 1);
        }

        if (DataManagerScript.isFirstPlay)
        {
            Invoke("AllowInput", 9f);
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

		// Iterate over all inputs for actions
		for (int i = 0; i < gamepads.Length; i++) {

            // Xbox numbering
            int gamepadIndex = i + 1;

			// Listen for activation
			if (!mainMenuActive) {

				if (inputAllowed && (Input.GetButtonDown(gamepads[i].jump) || Input.GetButtonDown(gamepads[i].start))) {

					if (DataManagerScript.demoMode) {

                        // Jump right into lobby if in demo mode
                        DataManagerScript.gamepadControllingMenus = gamepadIndex;
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
                        LeanTween.move(Camera.main.gameObject, new Vector3(0f, -3.3f, -10f), 0.5f).setOnComplete(()=>activateMainMenu(gamepadIndex)).setEase(LeanTweenType.easeOutQuad);
                      // activateMainMenu(gamepadIndex);
						#endif
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

				// Listen for cancel
				if (controllingGamepad != null && Input.GetButtonDown(controllingGamepad.grav)) {
					cancelCurrentMenu(false);
				}
			}
		}
	}

	void cancelCurrentMenu(bool cancelAll) {
		if (!singlePlayerPanel.activeSelf || cancelAll) {
			// Canceling out of main menu
			mainMenuActive = false;
			mainMenuPanel.SetActive (false);
			singlePlayerPanel.SetActive (false);
            LeanTween.move(Camera.main.gameObject, new Vector3(0f, 0f, -10f), 0.5f).setEase(LeanTweenType.easeOutQuad);
            pressStartAnimation.SetActive(true);
            pressStartAnimation.GetComponent<PlayAnimationScript>().PlayAnimation();
        } else {
			// Cancelling out of single player menu
			es1.SetSelectedGameObject(null);
			es1.SetSelectedGameObject(es1.firstSelectedGameObject);
			singlePlayerPanel.SetActive (false);
			mainMenuPanel.SetActive (true);
		}
	}

	public void activateMainMenu(int gamepad) {

		// Assign gamepad to menus
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

    

        es1.SetSelectedGameObject(null);
		es1.SetSelectedGameObject(es1.firstSelectedGameObject);

		// depending on which controller was tagged in, set the input stringes here
		controllingGamepad = new JoystickButtons (gamepad);
		es1.GetComponent<StandaloneInputModule> ().horizontalAxis = controllingGamepad.horizontal;
		es1.GetComponent<StandaloneInputModule> ().verticalAxis = controllingGamepad.vertical;
		es1.GetComponent<StandaloneInputModule> ().submitButton = controllingGamepad.jump;
		es1.GetComponent<StandaloneInputModule> ().cancelButton = controllingGamepad.grav;
	}

	public void SetUpSinglePlayerMenu (){
		es1.SetSelectedGameObject(soloModeButton);
	}

    public void StartSoloModeGame()
    {
        //TODO: This should be a different scene, specifically for choosing ONE shape. For now, just start the game with Square
        DataManagerScript.isSinglePlayerMode = true;
        SceneManager.LoadSceneAsync("soloGameScene");
    }

    public void StartMultiplayerGame(){
        DataManagerScript.isSinglePlayerMode = false;
        SceneManager.LoadSceneAsync ("ChoosePlayerScene");
	}
	public void StartChallengesGame(){
		DataManagerScript.isChallengeMode = true;
		SceneManager.LoadSceneAsync ("ChooseChallengeScene");

	}
	public void StartOptionsMenu(){
		SceneManager.LoadSceneAsync ("OptionsScene");
	}
}
