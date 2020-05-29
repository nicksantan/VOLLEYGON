using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

#if UNITY_XBOXONE
	using Users;
#endif

public class ChoosePlayerScript : MonoBehaviour {

	public Image gutterBG;

    public bool soloMode = false;
	public GameObject fakePlayer1;
	public GameObject fakePlayer2;
	public GameObject fakePlayer3;
	public GameObject fakePlayer4;

	public GameObject curtain;

	public GameObject gamepadIcon1;
	public GameObject gamepadIcon2;
	public GameObject gamepadIcon3;
	public GameObject gamepadIcon4;
	public GameObject[] gamepadIcons;

	public GameObject userText1;
	public GameObject userText2;
	public GameObject userText3;
	public GameObject userText4;
    public GameObject[] usernames;

	private JoystickButtons[] joysticks = new JoystickButtons[4] { new JoystickButtons(1), new JoystickButtons(2), new JoystickButtons(3), new JoystickButtons(4) };

	public Image msgBG;
	public Image msgBG2;

	public Text onePlayerMessage;
	public Text oneOnOneMessage;
	public Text twoOnOneMessage;

	public bool locked;
	private bool gameIsStartable = false;

    public bool player1Ready = false;
    public bool player2Ready = false;
    public bool player3Ready = false;
    public bool player4Ready = false;

	private int playersOnLeft = 0;
	private int playersOnRight = 0;

	private GameObject[] fakePlayers;
	public static ChoosePlayerScript Instance { get; private set; }

    private string defaultText = "Y: LOG IN";

	void Awake() {

		Instance = this;
		MusicManagerScript.Instance.whichSource += 1;
		MusicManagerScript.Instance.whichSource = MusicManagerScript.Instance.whichSource % 2;
		MusicManagerScript.Instance.SwitchToSource();

	}

	void Start(){
		gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(0.0f);
		// Reset slots
    	DataManagerScript.playerOneJoystick = -1;
    	DataManagerScript.playerTwoJoystick = -1;
    	DataManagerScript.playerThreeJoystick = -1;
    	DataManagerScript.playerFourJoystick = -1;

		MusicManagerScript.Instance.StartRoot ();
		oneOnOneMessage.enabled = false;
		twoOnOneMessage.enabled = false;
		onePlayerMessage.enabled = false;
		msgBG.enabled = false;
		msgBG2.enabled = false;
		locked = false;

		DataManagerScript.playerOnePlaying = false;
		DataManagerScript.playerTwoPlaying = false;
		DataManagerScript.playerThreePlaying = false;
		DataManagerScript.playerFourPlaying = false;

		DataManagerScript.playerOneType = 0;
		DataManagerScript.playerTwoType = 0;
		DataManagerScript.playerThreeType = 0;
		DataManagerScript.playerFourType = 0;

		// Fade in
		curtain.SetActive(true);
		curtain.GetComponent<NewFadeScript>().Fade(0f);

		// Make array of icons and usernames
		gamepadIcons = new GameObject[4] { gamepadIcon1, gamepadIcon2, gamepadIcon3, gamepadIcon4 };
        usernames = new GameObject[4] { userText1, userText2, userText3, userText4 };

		// Loop over icons and activate any already active in menu
		for ( int i = 0; i < gamepadIcons.Length; i++) {
			int gamepadId = i + 1;

			// Activate gamepad for player who selected the game mode
			bool isActive = DataManagerScript.gamepadControllingMenus == gamepadId;
			gamepadIcons[i].GetComponent<GamepadController>().ToggleIcon(isActive);
		}
	}

    // See if all players that are tagged in have also readied up
	bool noUnreadyPlayers(){

		if (fakePlayer1.GetComponent<FakePlayerScript> ().taggedIn && !fakePlayer1.GetComponent<FakePlayerScript> ().readyToPlay) {
			return false;
		} else if (fakePlayer2.GetComponent<FakePlayerScript> ().taggedIn && !fakePlayer2.GetComponent<FakePlayerScript> ().readyToPlay) {
			return false;
		} else if (fakePlayer3.GetComponent<FakePlayerScript> ().taggedIn && !fakePlayer3.GetComponent<FakePlayerScript> ().readyToPlay) {
			return false;
		} else if (fakePlayer4.GetComponent<FakePlayerScript> ().taggedIn && !fakePlayer4.GetComponent<FakePlayerScript> ().readyToPlay) {
			return false;
		} else {
			return true;
		}

	}

    // See if chosen slots and player ready statuses are ok to start the game
	public void CheckStartable(){

		playersOnLeft = 0;
		playersOnRight = 0;

		if (fakePlayer1.GetComponent<FakePlayerScript> ().readyToPlay) {
			playersOnLeft++;
		}
		if (fakePlayer2.GetComponent<FakePlayerScript> ().readyToPlay) {
			playersOnLeft++;
		}

		if (fakePlayer3.GetComponent<FakePlayerScript> ().readyToPlay) {
			playersOnRight++;
		}
		if (fakePlayer4.GetComponent<FakePlayerScript> ().readyToPlay) {
			playersOnRight++;
		}

		if ((playersOnLeft == 1 && playersOnRight == 0) && noUnreadyPlayers () || (playersOnLeft == 0 && playersOnRight == 1) && noUnreadyPlayers ()) {

            // disable single player, to be replaced with "solo mode"
            if (soloMode)
            {
                // Single player startable
                gameIsStartable = true;
                msgBG.enabled = true;
                msgBG2.enabled = true;
                onePlayerMessage.enabled = true;
                twoOnOneMessage.enabled = false;
                oneOnOneMessage.enabled = false;
            }

        } else if (playersOnLeft > 0 && playersOnRight > 0 && noUnreadyPlayers()) {

            // Multiplayer game is startable
			gameIsStartable = true;
			gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(1.0f);
            if (playersOnLeft == 2 && playersOnRight == 1 || playersOnLeft == 1 && playersOnRight == 2) {

				// Display 2v1 message
				// TODO: Msg bg is shown before 4 player start, which it shouldn't. It's dumb that there are two images, they should be a group.
				msgBG.enabled = true;
				msgBG2.enabled = true;
				twoOnOneMessage.enabled = true;
                oneOnOneMessage.enabled = false;
                onePlayerMessage.enabled = false;
				gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(1.0f);

			} else if (playersOnLeft == 1 && playersOnRight == 1){

				// Display 1v1 message
				// TODO: Msg bg is shown before 4 player start, which it shouldn't. It's dumb that there are two images, they should be a group.
				msgBG.enabled = true;
				msgBG2.enabled = true;
				oneOnOneMessage.enabled = true;
				twoOnOneMessage.enabled = false;
				onePlayerMessage.enabled = false;
				gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(1.0f);

			}

		} else {

            // Game is not startable
			twoOnOneMessage.enabled = false;
			oneOnOneMessage.enabled = false;
			onePlayerMessage.enabled = false;
			msgBG.enabled = false;
			msgBG2.enabled = false;
			gameIsStartable = false;
			gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(0f);

		}
	}

	IEnumerator StartGame(){
		if (!locked) {
			locked = true;
            yield return new WaitForSeconds (GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f));
            Debug.Log("Solo mode?");
            Debug.Log(soloMode);
            if (!soloMode)
            {
                SceneManager.LoadSceneAsync("chooseArenaScene");
            }
            else
            {
                SceneManager.LoadSceneAsync("soloGameScene");
            }
        }
	}

	void exitIfNoOtherGamepads() {

		// See if any gamepads besides this one are active
		for ( int i = 0; i < gamepadIcons.Length; i++) {
			if (gamepadIcons[i].GetComponent<GamepadController>().enabled) {
				return;
			}
		}

		// If no gamepad icons were active, return to title
		StartCoroutine ("BackToTitle");
	}

	IEnumerator BackToTitle(){
		if (!locked) {
			locked = true;
            float fadeTime = GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
			yield return new WaitForSeconds (fadeTime);
			SceneManager.LoadSceneAsync ("titleScene");
		}
	}

	// Update is called once per frame
	void Update () {
		// Look for start button presses
		for (int i = 0; i < joysticks.Length; i++) {
			int slotId = i + 1;
			JoystickButtons joystick = joysticks[i];

			if (Input.GetButtonDown(joystick.start) || (Input.GetButtonDown(joystick.jump) && !gamepadIcons[i].GetComponent<GamepadController>().enabled)) {

				// Start game if startable and gamepad not tagged in
				if (gameIsStartable && gamepadIcons[i].GetComponent<GamepadController>().enabled) {
                    StartCoroutine("StartGame");
                }
				else if (!gamepadIcons[i].GetComponent<GamepadController>().enabled) {

					// Tag in gamepad if not
					gamepadIcons[i].GetComponent<GamepadController>().ToggleIcon(true);

				}
			}

			#if UNITY_XBOXONE
				// Show user select if on xbox
				if (DataManagerScript.xboxMode) {

					// See if this slot has a gamepad
					int joystickForSlot = -1;
					bool slotTaken = false;
					switch (slotId) {
						case 1:
							joystickForSlot = DataManagerScript.playerOneJoystick;
							slotTaken = joystickForSlot != -1;
							break;
						case 2:
							joystickForSlot = DataManagerScript.playerTwoJoystick;
							slotTaken = joystickForSlot != -1;
							break;
						case 3:
							joystickForSlot = DataManagerScript.playerThreeJoystick;
							slotTaken = joystickForSlot != -1;
							break;
						case 4:
							joystickForSlot = DataManagerScript.playerFourJoystick;
							slotTaken = joystickForSlot != -1;
							break;
					}

					// Show username or login prompt for selected slots
					if (slotTaken && !usernames[i].activeSelf) {

						int id = XboxOneInput.GetUserIdForGamepad((uint)joystickForSlot);
						showLoginPrompt(slotId, id);

					} else if (!slotTaken && usernames[i].activeSelf) {

						// Reset text and hide prompt
						usernames[i].SetActive(false);
						usernames[i].GetComponent<Text>().text = defaultText;
						Debug.Log("reset to default");

					}

					// Change player on Y press
					JoystickButtons slotsJoystick = new JoystickButtons(joystickForSlot);
					if (slotTaken && Input.GetButtonDown(slotsJoystick.y)) {
						Debug.Log(">>> Y Pressed");
						DataManagerScript.slotToUpdate = slotId;
						UsersManager.RequestSignIn(Users.AccountPickerOptions.None, (ulong)joystickForSlot);
					}
				}
			#endif
		}

		// Go ahead and start if all players ready
		if (fakePlayer1.GetComponent<FakePlayerScript>().readyToPlay
			&& fakePlayer2.GetComponent<FakePlayerScript>().readyToPlay
			&& fakePlayer3.GetComponent<FakePlayerScript>().readyToPlay
			&& fakePlayer4.GetComponent<FakePlayerScript>().readyToPlay) {

			StartCoroutine ("StartGame");

		}

        // go ahead and start if this is a single player game and any player is ready
        if (DataManagerScript.isSinglePlayerMode && (fakePlayer1.GetComponent<FakePlayerScript>().readyToPlay
            || fakePlayer2.GetComponent<FakePlayerScript>().readyToPlay
            || fakePlayer3.GetComponent<FakePlayerScript>().readyToPlay
            || fakePlayer4.GetComponent<FakePlayerScript>().readyToPlay)){
            StartCoroutine("StartGame");
        }

    }

	void FixedUpdate() {
		// Back out if no gamepads
		exitIfNoOtherGamepads();
	}

	#if UNITY_XBOXONE
		public void showLoginPrompt(int slotId, int userId) {
			int slotIndex = slotId - 1;

			// Show default text or username
			string promptText = defaultText;

			// Get user and print name
			if (userId != 0)
			{
				User u = UsersManager.FindUserById(userId);
				promptText = "Y: " + u.OnlineID;
			}

			Debug.Log("ID: " + userId + " | Name: " + promptText);
			Debug.Log(usernames[slotIndex].GetComponent<Text>().text);

			// Show text
			Text prompt = usernames[slotIndex].GetComponent<Text>();
			prompt.text = promptText;
			if (!usernames[slotIndex].activeSelf)
			{
				usernames[slotIndex].SetActive(true);
			}

			// Attempted fix for canvas not re-rendering with new value
			Canvas.ForceUpdateCanvases();

			Debug.Log(usernames[slotIndex].GetComponent<Text>().text);
		}
	#endif
}
