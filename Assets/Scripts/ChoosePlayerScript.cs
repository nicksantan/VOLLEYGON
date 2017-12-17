using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoosePlayerScript : MonoBehaviour {

	public GameObject fakePlayer1;
	public GameObject fakePlayer2;
	public GameObject fakePlayer3;
	public GameObject fakePlayer4;

	public GameObject gamepadIcon1;
	public GameObject gamepadIcon2;
	public GameObject gamepadIcon3;
	public GameObject gamepadIcon4;

	private JoystickButtons gamepad1;
	private JoystickButtons gamepad2;
	private JoystickButtons gamepad3;
	private JoystickButtons gamepad4;

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
	// Use this for initialization

	void Awake() {

		Instance = this;
		MusicManagerScript.Instance.whichSource += 1;
		MusicManagerScript.Instance.whichSource = MusicManagerScript.Instance.whichSource % 2;
		MusicManagerScript.Instance.SwitchToSource();

	}

	void Start(){

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

		// Get all possible gamepads
		gamepad1 = new JoystickButtons(1);
		gamepad2 = new JoystickButtons(2);
		gamepad3 = new JoystickButtons(3);
		gamepad4 = new JoystickButtons(4);

		// Activate gamepad for player who selected the game mode
		if (DataManagerScript.gamepadControllingMenus == 1) {
			gamepadIcon1.SetActive(true);
		}
		else if (DataManagerScript.gamepadControllingMenus == 2) {
			gamepadIcon2.SetActive(true);
		}
		else if (DataManagerScript.gamepadControllingMenus == 3) {
			gamepadIcon3.SetActive(true);
		}
		else if (DataManagerScript.gamepadControllingMenus == 4) {
			gamepadIcon4.SetActive(true);
		}

		// foreach (Users.User u in Users.UsersManager.Users) {
		// 	if (u.IsSignedIn) {
		// 		Debug.Log("Logged in: " + u.OnlineID);
		// 		Debug.Log("Logged in: " + u.Index);
		// 		Debug.Log("Logged in: " + u.Id);
		// 	}
		// }
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

            // Single player startable
   			// gameIsStartable = true;
			// msgBG.enabled = true;
			// msgBG2.enabled = true;
			// onePlayerMessage.enabled = true;
			// twoOnOneMessage.enabled = false;
			// oneOnOneMessage.enabled = false;

		} else if (playersOnLeft > 0 && playersOnRight > 0 && noUnreadyPlayers()) {

            // Multiplayer game is startable
			gameIsStartable = true;
            msgBG.enabled = true;
            msgBG2.enabled = true;

            if (playersOnLeft == 2 && playersOnRight == 1 || playersOnLeft == 1 && playersOnRight == 2) {

				// Display 2v1 message
				twoOnOneMessage.enabled = true;
                oneOnOneMessage.enabled = false;
                onePlayerMessage.enabled = false;

			} else if (playersOnLeft == 1 && playersOnRight == 1){

				// Display 1v1 message
				oneOnOneMessage.enabled = true;
				twoOnOneMessage.enabled = false;
				onePlayerMessage.enabled = false;

			}

		} else {

            // Game is not startable
			twoOnOneMessage.enabled = false;
			oneOnOneMessage.enabled = false;
			onePlayerMessage.enabled = false;
			msgBG.enabled = false;
			msgBG2.enabled = false;
			gameIsStartable = false;

		}
	}

	IEnumerator StartGame(){
		float fadeTime = GameObject.Find ("FadeCurtain").GetComponent<FadingScript> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel ("chooseArenaScene");
	}

	// Update is called once per frame
	void Update () {
		bool pressed1 = Input.GetButtonDown (gamepad1.start);
		bool pressed2 = Input.GetButtonDown (gamepad2.start);
		bool pressed3 = Input.GetButtonDown (gamepad3.start);
		bool pressed4 = Input.GetButtonDown (gamepad4.start);

		if (pressed1 || pressed2 || pressed3 || pressed4) {
			if (gameIsStartable) {
				Application.LoadLevel ("chooseArenaScene");
			} else {
				if (pressed1 && gamepadIcon1.activeSelf == false) {
					gamepadIcon1.SetActive(true);
				}
				else if (pressed2 && gamepadIcon2.activeSelf == false) {
					gamepadIcon2.SetActive(true);
				}
				else if (pressed3 && gamepadIcon3.activeSelf == false) {
					gamepadIcon3.SetActive(true);
				}
				else if (pressed4 && gamepadIcon4.activeSelf == false) {
					gamepadIcon4.SetActive(true);
				}
			}
		}

		if (fakePlayer1.GetComponent<FakePlayerScript>().readyToPlay && fakePlayer2.GetComponent<FakePlayerScript>().readyToPlay && fakePlayer3.GetComponent<FakePlayerScript>().readyToPlay && fakePlayer4.GetComponent<FakePlayerScript>().readyToPlay) {
			locked = true;
			StartCoroutine ("StartGame");
		}
	}
}
