using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Users;

public class TitleManagerScript : MonoBehaviour {

	private JoystickButtons controllingGamepad;
	private JoystickButtons[] gamepads = new JoystickButtons[4];

	public Text versionText;
	public GameObject mainMenuPanel;
	public GameObject singlePlayerPanel;
	public GameObject soloModeButton;

	private bool mainMenuActive = false;

	public EventSystem es1;

	public Button firstButton;


	// Use this for initialization
	void Start () {
		MusicManagerScript.Instance.FadeOutEverything ();
		versionText.text = DataManagerScript.version;
		DataManagerScript.ResetStats ();
		DataManagerScript.ResetPlayerTypes ();
		DataManagerScript.isChallengeMode = false;

		// Init controller maps
		for (int i = 0; i < 4; i++) {
			gamepads[i] = new JoystickButtons(i+1);
		}

		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
	}

	public static readonly XboxOneKeyCode[] startTitleButtons = {
		XboxOneKeyCode.Gamepad1ButtonA,
		XboxOneKeyCode.Gamepad2ButtonA,
		XboxOneKeyCode.Gamepad3ButtonA,
		XboxOneKeyCode.Gamepad4ButtonA,
		XboxOneKeyCode.Gamepad1ButtonX,
		XboxOneKeyCode.Gamepad2ButtonX,
		XboxOneKeyCode.Gamepad3ButtonX,
		XboxOneKeyCode.Gamepad4ButtonX,
		XboxOneKeyCode.Gamepad1ButtonView,
		XboxOneKeyCode.Gamepad2ButtonView,
		XboxOneKeyCode.Gamepad3ButtonView,
		XboxOneKeyCode.Gamepad4ButtonView,
		XboxOneKeyCode.Gamepad1ButtonMenu,
		XboxOneKeyCode.Gamepad2ButtonMenu,
		XboxOneKeyCode.Gamepad3ButtonMenu,
		XboxOneKeyCode.Gamepad4ButtonMenu
	};

	public static readonly XboxOneKeyCode[] yButtons = {
		XboxOneKeyCode.Gamepad1ButtonY,
		XboxOneKeyCode.Gamepad2ButtonY,
		XboxOneKeyCode.Gamepad3ButtonY,
		XboxOneKeyCode.Gamepad4ButtonY
	};

    public static bool GetValidButtonDown(XboxOneKeyCode[] validButtons, out XboxOneKeyCode buttonThatWasPressed) {
        for (int i = 0; i < validButtons.Length; i++) {
            XboxOneKeyCode keyCode = validButtons[i];
            if (XboxOneInput.GetKeyDown(keyCode)) {
                buttonThatWasPressed = keyCode;
                return true;
            }
        }
        buttonThatWasPressed = XboxOneKeyCode.Gamepad1ButtonA;
        return false;
    }

	void Update () {
		MusicManagerScript.Instance.FadeOutEverything ();

		// Listen for activation
		if (!mainMenuActive) {
			// Look for xbox input
			if (XboxOneInput.GetKeyDown( XboxOneKeyCode.GamepadButtonA )
				|| XboxOneInput.GetKeyDown( XboxOneKeyCode.GamepadButtonX )
				|| XboxOneInput.GetKeyDown( XboxOneKeyCode.GamepadButtonView )
				|| XboxOneInput.GetKeyDown( XboxOneKeyCode.GamepadButtonMenu )
				) {

				// Get gamepad
	            XboxOneKeyCode keyCode;
	            if (GetValidButtonDown(startTitleButtons, out keyCode)) {
	                int gamepadIndex = (int)XboxOneInput.GetGamepadIndexFromGamepadButton(keyCode);

	                // Sign in if active player is not associated with this controller
	                if (XboxOneInput.GetUserIdForGamepad((uint)gamepadIndex) == 0) {
	                	DataManagerScript.shouldActivateMenu = true;
						UsersManager.RequestSignIn(Users.AccountPickerOptions.None, (ulong)gamepadIndex);
					}
					else {
	 					// Open main menu with this controller
						activateMainMenu(gamepadIndex);
					}
				}
			}
			else {
				// Iterate over all inputs for actions
				for (int i = 0; i < gamepads.Length; i++) {
					if (Input.GetButtonDown (gamepads[i].jump) || Input.GetButtonDown (gamepads[i].start)) {

	 					// Open main menu
						activateMainMenu(i + 1);
					}
				}
			}
		} else {
			// Listen for user change (Y button)
			if (XboxOneInput.GetKeyDown( XboxOneKeyCode.GamepadButtonY )) {

				// Get gamepad
	            XboxOneKeyCode keyCode;
	            if (GetValidButtonDown(yButtons, out keyCode)) {
	                int gamepadIndex = (int)XboxOneInput.GetGamepadIndexFromGamepadButton(keyCode);

	                // Back out and log in again if active player presses Y
	                if (gamepadIndex == DataManagerScript.gamepadControllingMenus) {
						cancelCurrentMenu(true);
	                	DataManagerScript.shouldActivateMenu = true;
						UsersManager.RequestSignIn(Users.AccountPickerOptions.None, (ulong)gamepadIndex);
					}
				}
			}

			// Listen for cancel
			if (controllingGamepad != null &&  Input.GetButtonDown (controllingGamepad.grav)) {
				cancelCurrentMenu(false);
			}
		}
	}

	void cancelCurrentMenu(bool cancelAll) {
		if (!singlePlayerPanel.active || cancelAll) {
			// Canceling out of main menu
			mainMenuActive = false;
			mainMenuPanel.SetActive (false);
			singlePlayerPanel.SetActive (false);
		} else {
			// Cancelling out of single player menu
			es1.SetSelectedGameObject(null);
			es1.SetSelectedGameObject(es1.firstSelectedGameObject);
			singlePlayerPanel.SetActive (false);
			mainMenuPanel.SetActive (true);
		}
	}

	public void activateMainMenu(int gamepad) {

		// Addign gamepad to menus
		DataManagerScript.gamepadControllingMenus = gamepad;

		// Save "active" user if on xbox
		if (DataManagerScript.xboxMode) {
			int userId = XboxOneInput.GetUserIdForGamepad((uint)gamepad);
			DataManagerScript.userControllingMenus = UsersManager.FindUserById(userId);
		}

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
	public void StartMultiplayerGame(){
		Application.LoadLevel ("ChoosePlayerScene");
	}
	public void StartChallengesGame(){
		DataManagerScript.isChallengeMode = true;
		Application.LoadLevel ("ChooseChallengeScene");

	}
	public void StartOptionsMenu(){
		Application.LoadLevel ("OptionsScene");
	}
}
