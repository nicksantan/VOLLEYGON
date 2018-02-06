using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManagerScript : MonoBehaviour {

	public GameObject scoreboard;
	public GameObject background;
	public float gravTimer;
	public float gameTimer;
	private bool timerRunning = false;
	private bool readyForReplay;
	private bool locked;
	public int teamOneScore;
	public int teamTwoScore;
	public Text winText;
	public int rallyCount;
	public Text rallyCountText;
	public bool isGameOver;
	public int scorePlayedTo = 11;
	public int arenaType;
	public bool paused = false;
	public bool recentlyPaused = false;
	private float timeSinceLastPowerup;
	private float powerupAppearTime;
	public GameObject speedPowerupPrefab;
	public GameObject powerupPrefab;
	public GameObject gravityIndicator;
	public GameObject playerClonePrefab;
	public GameObject pausePanel;
	public int lastScore;
	public int bouncesOnBottom;
	public int bouncesOnTopLeft;
	public int bouncesOnTopRight;
	public int bouncesOnBottomRight;
	public int bouncesOnBottomLeft;
	public int bounces = 0;
	public bool soloMode;
	public int soloModeBalls;

	public GameObject ball;

	// Hold references to each of the players. Activate or de-activate them based on options chosen on the previous page.
	public GameObject Player1;
	public GameObject Player2;
	public GameObject Player3;
	public GameObject Player4;

	public Material Player1Material;
	public Material Player2Material;
	public Material Player3Material;
	public Material Player4Material;

	private bool OnePlayerMode;

	// Hold references to each of the arenas
	public GameObject Arena1;
	public GameObject Arena2;
	public GameObject Arena3;
	public GameObject Arena4;
	public GameObject Arena5;
	public GameObject Arena6;
	public GameObject Arena7;
	public GameObject Arena8;
	public GameObject Arena9;

	public GameObject CurrentArena;

	public string startButton1 = "Start_P1";
	public string startButton2 = "Start_P2";
	public string startButton3 = "Start_P3";
	public string startButton4 = "Start_P4";

	public EventSystem es;

	public int lastTouch;
	public int secondToLastTouch;

	// Static singleton property
	public static GameManagerScript Instance { get; private set; }

    // Initialization
	void Awake()
	{
		// Save a reference to the AudioHandler component as our singleton instance
		Instance = this;

		locked = false;

		Player1.GetComponent<PlayerController>().playerType = DataManagerScript.playerOneType;
		Player2.GetComponent<PlayerController>().playerType = DataManagerScript.playerTwoType;
		Player3.GetComponent<PlayerController>().playerType = DataManagerScript.playerThreeType;
		Player4.GetComponent<PlayerController>().playerType = DataManagerScript.playerFourType;

		MusicManagerScript.Instance.StartRoot ();
		launchTimer ();
		timeSinceLastPowerup = 0f;
		soloModeBalls = 3;
		powerupAppearTime = 10f;
		readyForReplay = false;
		lastTouch = 0;
		secondToLastTouch = 0;
		ball.SetActive(true);
		winText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		// Invoke ("StartReplay", 2f);

		rallyCount = 0;
		// Set up players and their rigidbodies based on character selection choice
		//	Player1.SetActive (false);


		// Set up arena based on options
		arenaType = DataManagerScript.arenaType;

		// This is so dumb.
		switch (arenaType) {

		case 1:
			Arena1.SetActive (true);
			break;

		case 2:
			Arena2.SetActive (true);
			break;

		case 3:
			Arena3.SetActive (true);
			break;

		case 4:
			Arena4.SetActive (true);
			break;

		case 5:
			Arena5.SetActive (true);
			break;

		case 6:
			Arena6.SetActive (true);
			break;

		case 7:
			Arena7.SetActive (true);
			break;

		case 8:
			Arena8.SetActive (true);
			break;

		case 9:
			Arena9.SetActive (true);
			break;

		default:
			Arena1.SetActive (true);
			break;

		}

		CurrentArena = GameObject.FindWithTag("Arena");

		int playersActive = 0;
		int whichSoloPlayer = 0;

		// make this a common function in a class
		if (DataManagerScript.playerOnePlaying == true) {
			Player1.SetActive (true);
			playersActive++;
			whichSoloPlayer = 1;
		}
		if (DataManagerScript.playerTwoPlaying == true) {
			Player2.SetActive (true);
			playersActive++;
			whichSoloPlayer = 2;
		}
		if (DataManagerScript.playerThreePlaying == true) {
			Player3.SetActive (true);
			playersActive++;
			whichSoloPlayer = 3;
		}
		if (DataManagerScript.playerFourPlaying == true) {
			Player4.SetActive (true);
			playersActive++;
			whichSoloPlayer = 4;
		}

		if (playersActive == 1) {

			OnePlayerMode = true;
			InstantiateClone (whichSoloPlayer);
			// set this somewhere else
			soloMode = true;
			rallyCountText.gameObject.SetActive (true);

        }
        else {
			OnePlayerMode = false;
		}

	}
	void launchTimer(){
		timerRunning = true;

	}

	void IncreasePlayCount(string whichType){
		int tempTotal = PlayerPrefs.GetInt (whichType);
		tempTotal += 1;
		PlayerPrefs.SetInt (whichType, tempTotal);
	}

	void InstantiateClone(int whichSoloPlayer){
		// create a clone of the current player, place it on the opposite team, and bind the same controls to it

		GameObject playerClone = null;
		int playerType = 0;
		Material whichMat = null;

		switch (whichSoloPlayer) {

		    case 1:
			    playerClone = Instantiate (playerClonePrefab, new Vector3 (10.0f, -5f, -0.5f), Quaternion.identity);
			    playerType = Player1.GetComponent<PlayerController> ().playerType;
			    playerClone.GetComponent<PlayerController> ().team = 2;
			    playerClone.GetComponent<PlayerController> ().startingGrav = 1;
			    whichMat = Player1Material;
			    // determine position
			    // run a config function to bind the controls
			    break;

		    case 2:
			    playerClone = Instantiate (playerClonePrefab, new Vector3 (10.0f, 5f, -0.5f), Quaternion.identity);
			    playerType = Player2.GetComponent<PlayerController> ().playerType;
			    playerClone.GetComponent<PlayerController> ().team = 2;
			    playerClone.GetComponent<PlayerController> ().startingGrav = -1;
			    whichMat = Player2Material;
			    break;

		    case 3:
			    playerClone = Instantiate (playerClonePrefab, new Vector3 (-10.0f, -5f, -0.5f), Quaternion.identity);
			    playerType = Player3.GetComponent<PlayerController> ().playerType;
			    playerClone.GetComponent<PlayerController> ().team = 1;
			    playerClone.GetComponent<PlayerController> ().startingGrav = 1;
			    whichMat = Player3Material;
			    break;

		    case 4:
			    playerClone = Instantiate (playerClonePrefab, new Vector3 (-10.0f, 5f, -0.5f), Quaternion.identity);
			    playerType = Player4.GetComponent<PlayerController> ().playerType;
			    playerClone.GetComponent<PlayerController> ().team = 1;
			    playerClone.GetComponent<PlayerController> ().startingGrav = -1;
			    whichMat = Player4Material;
			    break;

		    default:
			    playerClone = Instantiate (playerClonePrefab, new Vector3 (10.0f, -5f, -0.5f), Quaternion.identity);
			    playerType = Player1.GetComponent<PlayerController> ().playerType;
			    playerClone.GetComponent<PlayerController> ().team = 2;
                playerClone.GetComponent<PlayerController>().startingGrav = 1;
                break;
		}

		playerClone.SetActive (true);
		playerClone.transform.SetParent (GameObject.Find ("Players").transform);
	//	playerClone.GetComponent<BoxCollider2D> ().enabled = true;
		playerClone.GetComponent<PlayerController>().playerID = whichSoloPlayer;
		playerClone.GetComponent<PlayerController>().playerType = playerType;
		playerClone.GetComponent<MeshRenderer> ().material = whichMat;
		if (playerType == 1) {
			playerClone.transform.Find ("Circle").GetComponent<CircleEfficient> ().Rebuild ();
			playerClone.transform.Find ("Circle").GetComponent<MeshRenderer> ().material = whichMat;
		}

	}

	// void LaunchTitleScreen(){
	// 	Application.LoadLevel("titleScene");
	// }

	void LaunchStatsScreen(){
		StartCoroutine ("FadeToStats");
	}

	IEnumerator FadeToStats(){
		if (!locked) {
			locked = true;
			float fadeTime = GameObject.Find ("FadeCurtain").GetComponent<FadingScript> ().BeginFade (1);
			yield return new WaitForSeconds (fadeTime);
			if (!OnePlayerMode) {
				Application.LoadLevel("statsScene");
			} else {
				Application.LoadLevel("singlePlayerStatsScene");
			}
		}
	}

	// End game for single player only
	public void endGame(){
		isGameOver = true;
		DataManagerScript.rallyCount = rallyCount;
		Invoke ("LaunchStatsScreen", 5f);
	}

	// End game for team maches
	void teamWins(int whichTeam){

		switch (whichTeam) {
		case 1:
			scoreboard.GetComponent<ScoreboardManagerScript> ().TeamOneWin ();
			background.GetComponent<BackgroundColorScript> ().whoWon = 1;
			background.GetComponent<BackgroundColorScript> ().matchOver = true;
			background.GetComponent<BackgroundColorScript> ().TurnOffMatchPoint ();

			break;
		case 2:
			scoreboard.GetComponent<ScoreboardManagerScript> ().TeamTwoWin ();
			background.GetComponent<BackgroundColorScript> ().whoWon = 2;
			background.GetComponent<BackgroundColorScript> ().matchOver = true;
			break;
		}
		isGameOver = true;
		if (!readyForReplay) {
		//	EZReplayManager.get.stop ();
			readyForReplay = true;
		//	Invoke ("PlayReplay", 2f);
		}

		//EZReplayManager.get.play(-1,false,true,true);
		//DataManagerScript.dataManager.teamOneWins += 1;
		//Invoke ("LaunchTitleScreen", 5f);
		Invoke ("LaunchStatsScreen", 5f);
	}

	void PlayReplay(){
//		EZReplayManager.get.play (0, true, false, true);
	}

	void Update () {

		if (OnePlayerMode) {
			rallyCountText.text = rallyCount.ToString();
		}
		// if all 4 start buttons are pressed, warp back to title screen
		if (Input.GetButton (startButton1) && Input.GetButton (startButton2) && Input.GetButton (startButton3) && Input.GetButton (startButton4)) {
			Debug.Log ("returning to title");
			Application.LoadLevel("titleScene");
		}

		// keep track of match time
		DataManagerScript.gameTime += Time.deltaTime;
		timeSinceLastPowerup += Time.deltaTime;

		if (timerRunning) {
			gameTimer -= Time.deltaTime;
		}

		// Match point
		if (teamOneScore >= scorePlayedTo && teamOneScore == teamTwoScore + 1) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (1);
		} else if (teamTwoScore >= scorePlayedTo && teamTwoScore == teamOneScore + 1) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (2);
		}

		// Team wins
		if (teamOneScore >= scorePlayedTo && teamOneScore > teamTwoScore + 1) {
			teamWins (1);
		} else if (teamTwoScore >= scorePlayedTo && teamTwoScore > teamOneScore + 1) {
			teamWins (2);
		}

		if (!isGameOver) {
			ConsiderAPowerup ();
		}
	}

	public void Pause(JoystickButtons buttons){
		if (!paused) {
			// Sho wpause
			pausePanel.SetActive (true);

			// Assign butons
            es.GetComponent<StandaloneInputModule>().horizontalAxis = buttons.horizontal;
            es.GetComponent<StandaloneInputModule>().verticalAxis = buttons.vertical;
            es.GetComponent<StandaloneInputModule>().submitButton = buttons.jump;
            es.GetComponent<StandaloneInputModule>().cancelButton = buttons.grav;

            // Reset menu
			es.SetSelectedGameObject(null);
			es.SetSelectedGameObject(es.firstSelectedGameObject);
			MusicManagerScript.Instance.TurnOffEverything ();
			SoundManagerScript.instance.muteSFX ();
			//TODO: Move the ball's SFX to sound manager script
			ball.GetComponent<BallScript>().Pause ();
			Time.timeScale = 0;
			paused = true;
		}
	}

	public void Unpause(){
		if (paused){
			Time.timeScale = 1;
			paused = false;
			pausePanel.SetActive (false);
			recentlyPaused = true;
			MusicManagerScript.Instance.RestoreFromPause ();
			//TODO: Move the ball's SFX to sound manager script
			SoundManagerScript.instance.unMuteSFX ();
			ball.GetComponent<BallScript>().UnPause ();
			Invoke ("CancelRecentlyPaused", 0.1f);
		}
	}

	public void CancelRecentlyPaused(){
		recentlyPaused = false;
	}
	public void QuitGame(){
		Application.LoadLevel("TitleScene");
	}

	void ConsiderAPowerup(){
		if (timeSinceLastPowerup >= powerupAppearTime) {

			// spawn a powerup
			float xVal = Random.Range(4f, 15.5f);
			float inverseXVal = -1 * xVal;
			float yVal = Random.Range (-5f, 5f);
			int whichType = Random.Range (1, 5);
			float powerupDuration = 5f + (Random.value * 5f);
			GameObject firstPowerup = (GameObject)Instantiate(powerupPrefab, new Vector3(xVal, yVal, 0), Quaternion.identity);
			firstPowerup.SendMessage("Config", whichType);
			firstPowerup.SendMessage("ResetTime", powerupDuration);
			GameObject secondPowerup = (GameObject)Instantiate(powerupPrefab, new Vector3(inverseXVal, yVal, 0), Quaternion.identity);
			secondPowerup.SendMessage("Config", whichType);
			secondPowerup.SendMessage("ResetTime", powerupDuration);
			timeSinceLastPowerup = 0f;
			powerupAppearTime = 20f + Random.value * 20f;
		}
	}

	void CheckForMatchPoint(){
		// check for match point
		if (teamTwoScore == teamOneScore) {
			background.GetComponent<BackgroundColorScript> ().TurnOffMatchPoint ();
			//MusicManagerScript.Instance.SwitchMusic ();
		} else if (teamOneScore == scorePlayedTo - 1 && teamTwoScore < scorePlayedTo) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (1);
			background.GetComponent<BackgroundColorScript> ().TurnOffDeuce();
			MusicManagerScript.Instance.StartFifth ();
		} else if (teamTwoScore == scorePlayedTo - 1 && teamOneScore < scorePlayedTo) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (2);
			background.GetComponent<BackgroundColorScript> ().TurnOffDeuce();
			MusicManagerScript.Instance.StartFifth ();
		}
	}

//	void ComputeStat(int whichTeamScored){
//		if (whichTeamScored == 1) {
//			if (lastTouch == 1) {
//				DataManagerScript.playerOneAces += 1;
//				DataManagerScript.playerOneScores += 1;
//			}
//			if (lastTouch == 2) {
//				DataManagerScript.playerTwoAces += 1;
//				DataManagerScript.playerTwoScores += 1;
//			}
//
//			if (lastTouch == 3) {
//				if (secondToLastTouch == 1) {
//					DataManagerScript.playerOneScores += 1;
//				}
//				if (secondToLastTouch == 2) {
//					DataManagerScript.playerTwoScores += 1;
//				}
//				if (secondToLastTouch == 3) {
//					DataManagerScript.playerThreeBumbles += 1;
//				}
//				if (secondToLastTouch == 4) {
//					DataManagerScript.playerThreeBumbles += 1;
//				}
//			}
//			if (lastTouch == 4) {
//				if (secondToLastTouch == 1) {
//					DataManagerScript.playerOneScores += 1;
//				}
//				if (secondToLastTouch == 2) {
//					DataManagerScript.playerTwoScores += 1;
//				}
//				if (secondToLastTouch == 3) {
//					DataManagerScript.playerFourBumbles += 1;
//				}
//				if (secondToLastTouch == 4) {
//					DataManagerScript.playerFourBumbles += 1;
//				}
//			}
//		}
//		if (whichTeamScored == 2) {
//			if (lastTouch == 3) {
//				DataManagerScript.playerThreeAces += 1;
//				DataManagerScript.playerThreeScores += 1;
//			}
//			if (lastTouch == 4) {
//				DataManagerScript.playerFourAces += 1;
//				DataManagerScript.playerFourScores += 1;
//			}
//
//			if (lastTouch == 1) {
//				if (secondToLastTouch == 1) {
//					DataManagerScript.playerOneBumbles += 1;
//				}
//				if (secondToLastTouch == 2) {
//					DataManagerScript.playerOneBumbles += 1;
//				}
//				if (secondToLastTouch == 3) {
//					DataManagerScript.playerThreeScores += 1;
//				}
//				if (secondToLastTouch == 4) {
//					DataManagerScript.playerFourScores += 1;
//				}
//			}
//			if (lastTouch == 2) {
//				if (secondToLastTouch == 1) {
//					DataManagerScript.playerTwoBumbles += 1;
//				}
//				if (secondToLastTouch == 2) {
//					DataManagerScript.playerTwoBumbles += 1;
//				}
//				if (secondToLastTouch == 3) {
//					DataManagerScript.playerThreeScores += 1;
//				}
//				if (secondToLastTouch == 4) {
//					DataManagerScript.playerFourScores += 1;
//				}
//			}
//		}
//	}

	public void ComputeStat(int whichTeamScored){
		if (whichTeamScored == 1) {
			if (lastTouch == 1) {
				DataManagerScript.playerOneAces += 1;
				DataManagerScript.playerOneScores += 1;
			}
			if (lastTouch == 2) {
				DataManagerScript.playerTwoAces += 1;
				DataManagerScript.playerTwoScores += 1;
			}

			if (lastTouch == 3) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerOneScores += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerTwoScores += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerThreeBumbles += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerThreeBumbles += 1;
				}
			}
			if (lastTouch == 4) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerOneScores += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerTwoScores += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerFourBumbles += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerFourBumbles += 1;
				}
			}
		}
		if (whichTeamScored == 2) {
			if (lastTouch == 3) {
				DataManagerScript.playerThreeAces += 1;
				DataManagerScript.playerThreeScores += 1;
			}
			if (lastTouch == 4) {
				DataManagerScript.playerFourAces += 1;
				DataManagerScript.playerFourScores += 1;
			}

			if (lastTouch == 1) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerOneBumbles += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerOneBumbles += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerThreeScores += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerFourScores += 1;
				}
			}
			if (lastTouch == 2) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerTwoBumbles += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerTwoBumbles += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerThreeScores += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerFourScores += 1;
				}
			}
		}
	}
	public void SideChange(){
		bounces = 0;
		bouncesOnBottom = 0;
		bouncesOnTopLeft = 0;
		bouncesOnTopRight = 0;
		bouncesOnBottomLeft = 0;
		bouncesOnBottomRight = 0;
		CurrentArena.BroadcastMessage ("ReturnColor");

		DataManagerScript.currentRallyCount += 1;
		if (DataManagerScript.currentRallyCount > DataManagerScript.longestRallyCount) {
			DataManagerScript.longestRallyCount = DataManagerScript.currentRallyCount;
			Debug.Log ("longest rally count is now " + DataManagerScript.longestRallyCount);
		}

		// Credit a return to the last touch player
		switch (lastTouch) {
		case 1:
			DataManagerScript.playerOneReturns += 1;
			break;
		case 2:
			DataManagerScript.playerTwoReturns += 1;
			break;
		case 3:
			DataManagerScript.playerThreeReturns += 1;
			break;
		case 4:
			DataManagerScript.playerFourReturns += 1;
			break;
		}

		if (soloMode && ball.GetComponent<BallScript> ().lastXPos != 0) {
			GameManagerScript.Instance.GetComponent<GameManagerScript>().rallyCount++;
		}

	}

	public void ManageScore(float ballPosition){
		if (!soloMode) {
			if (Mathf.Sign (ballPosition) < 0) {
				teamTwoScore += 1;
				ComputeStat (2);
				if (lastScore != 2) {
					MusicManagerScript.Instance.SwitchMusic (2);
				}

				lastScore = 2;
			} else {
				teamOneScore += 1;
				ComputeStat (1);

				if (lastScore != 1) {
					MusicManagerScript.Instance.SwitchMusic (1);
				}

				lastScore = 1;
			}

			CurrentArena.BroadcastMessage ("ReturnColor");

			if (teamTwoScore < scorePlayedTo && teamOneScore < scorePlayedTo) {
				if (teamTwoScore == scorePlayedTo - 1 && teamOneScore == scorePlayedTo - 1) {
					scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
					background.GetComponent<BackgroundColorScript> ().TurnOnDeuce ();
				} else {

					scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, false);
				}

				CheckForMatchPoint ();

				ball.GetComponent<BallScript> ().ResetBall ();
				//Instantiate(prefab, new Vector3(0f, 0, 0), Quaternion.identity);
				//Destroy (gameObject);
			} else if (Mathf.Abs (GameManagerScript.Instance.teamOneScore - GameManagerScript.Instance.teamTwoScore) < 2) {
				if (GameManagerScript.Instance.teamTwoScore >= GameManagerScript.Instance.scorePlayedTo || GameManagerScript.Instance.teamOneScore >= GameManagerScript.Instance.scorePlayedTo) {
					//winByTwoText.CrossFadeAlpha (0.6f, .25f, false);
					MusicManagerScript.Instance.StartFifth ();
					CheckForMatchPoint ();
					scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
				}
				ball.GetComponent<BallScript> ().ResetBall ();

			} else {
				// GAME IS OVER
				transform.position = new Vector3 (0f, 0f, 0f);
                ball.SetActive(false);
			}
		}
		// If you're in one player mode....
	 	else {
			// single mode
			soloModeBalls--;
			// Debug.Log ("scored");
			// generate a random number between one and two
			int randomTrack = Random.Range (1, 3);
			MusicManagerScript.Instance.SwitchMusic (randomTrack);
			if (soloModeBalls <= 0) {
				// GAME IS OVER
				transform.position = new Vector3 (0f, 0f, 0f);
				gameObject.SetActive (false);
				GameManagerScript.Instance.endGame ();
			} else {
				// Hide ball on game over
				ball.SetActive(false);
			}
		}
	}
}


