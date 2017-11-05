using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoosePlayerScript : MonoBehaviour {

	public GameObject fakePlayer1;
	public GameObject fakePlayer2;
	public GameObject fakePlayer3;
	public GameObject fakePlayer4;
	public Image msgBG;
	public Image msgBG2;

	public Text onePlayerMessage;
	public Text oneOnOneMessage;
	public Text twoOnOneMessage;

	public bool locked;

	private string start1 = "Start_P1";
	private string start2 = "Start_P2";
	private string start3 = "Start_P3";
	private string start4 = "Start_P4";
	private string start1_Xbox = "Start_P1_Xbox";
	private string start2_Xbox = "Start_P2_Xbox";
	private string start3_Xbox = "Start_P3_Xbox";
	private string start4_Xbox = "Start_P4_Xbox";

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

            // Single player startable
            gameIsStartable = true;
			msgBG.enabled = true;
			msgBG2.enabled = true;
			onePlayerMessage.enabled = true;
			twoOnOneMessage.enabled = false;
			oneOnOneMessage.enabled = false;

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
		
		if (Input.GetButtonDown (start1) || Input.GetButtonDown (start2) || Input.GetButtonDown (start3) || Input.GetButtonDown (start4) || Input.GetButtonDown (start1_Xbox) || Input.GetButtonDown (start2_Xbox) || Input.GetButtonDown (start3_Xbox) || Input.GetButtonDown (start4_Xbox)) {
			if (gameIsStartable) {	
				Application.LoadLevel ("chooseArenaScene");
			}
		}
		
		if (fakePlayer1.GetComponent<FakePlayerScript>().readyToPlay && fakePlayer2.GetComponent<FakePlayerScript>().readyToPlay && fakePlayer3.GetComponent<FakePlayerScript>().readyToPlay && fakePlayer4.GetComponent<FakePlayerScript>().readyToPlay) {
			locked = true; 	
			StartCoroutine ("StartGame");
		}
	}
}
