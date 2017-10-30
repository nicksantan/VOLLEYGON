using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManagerScript : MonoBehaviour {

	public GameObject scoreboard;
	public GameObject background;
	public float gravTimer;
	public float gameTimer;
	private bool timerRunning = false;
	private bool readyForReplay;
	public int teamOneScore;
	public int teamTwoScore;
	public Text winText;
	public int rallyCount;
	public Text rallyCountText;
	public bool isGameOver;
	public int scorePlayedTo = 5;
	public int arenaType;
	public bool paused = false;
	private float timeSinceLastPowerup;
	private float powerupAppearTime;
	public GameObject speedPowerupPrefab;
	public GameObject powerupPrefab;
	public GameObject gravityIndicator;
	public GameObject playerClonePrefab;
	public GameObject pausePanel;

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

	// Static singleton property
	public static GameManagerScript Instance { get; private set; }

    // Initialization
	void Awake()
	{
		// Save a reference to the AudioHandler component as our singleton instance
		Instance = this;

		Player1.GetComponent<PlayerController>().playerType = DataManagerScript.playerOneType;
		Player2.GetComponent<PlayerController>().playerType = DataManagerScript.playerTwoType;
		Player3.GetComponent<PlayerController>().playerType = DataManagerScript.playerThreeType;
		Player4.GetComponent<PlayerController>().playerType = DataManagerScript.playerFourType;

		MusicManagerScript.Instance.StartRoot ();
		launchTimer ();
		timeSinceLastPowerup = 0f;
		powerupAppearTime = 10f;
		readyForReplay = false;
		winText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		Invoke ("StartReplay", 2f);

		rallyCount = 0;
		// Set up players and their rigidbodies based on character selection choice
		//	Player1.SetActive (false);


		// Set up arena based on options
		arenaType = DataManagerScript.arenaType;

		switch (arenaType) {

		case 1:
			Arena1.SetActive (true);
			CurrentArena = Arena1;
			break;

		case 2:
			Arena2.SetActive (true);
			CurrentArena = Arena2;
			break;

		case 3:
			Arena3.SetActive (true);
			CurrentArena = Arena3;
			break;

		case 4:
			Arena4.SetActive (true);
			CurrentArena = Arena4;
			break;

		case 5:
			Arena5.SetActive (true);
			CurrentArena = Arena5;
			break;

		case 6:
			Arena6.SetActive (true);
			CurrentArena = Arena6;
			break;

		case 7:
			Arena7.SetActive (true);
			CurrentArena = Arena7;
			break;

		case 8:
			Arena8.SetActive (true);
			CurrentArena = Arena8;
			break;

		case 9:
			Arena9.SetActive (true);
			CurrentArena = Arena9;
			break;

		default:
			Arena1.SetActive (true);
			CurrentArena = Arena1;
			break;

		}
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
			ball.GetComponent<BallScript> ().onePlayerMode = true;
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

	void LaunchTitleScreen(){
		Application.LoadLevel ("titleScene");
	}

	void LaunchStatsScreen(){
		StartCoroutine ("FadeToStats");
	}

	IEnumerator FadeToStats(){
		float fadeTime = GameObject.Find ("FadeCurtain").GetComponent<FadingScript> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		if (!OnePlayerMode) {
			Application.LoadLevel ("statsScene");
		} else {
			Application.LoadLevel ("singlePlayerStatsScene");
		}
	}

	public void endGame(){
		isGameOver = true;
		DataManagerScript.rallyCount = rallyCount;
		Invoke ("LaunchStatsScreen", 5f);
	}

	void teamWins(int whichTeam){
		Debug.Log ("Team wins running");
//		winText.text = "Team " + whichTeam.ToString () + " Wins!";
//		winText.CrossFadeAlpha(1f,.25f,false);
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
	// Update is called once per frame

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
			Application.LoadLevel ("titleScene");
		}

		// keep track of match time
		DataManagerScript.gameTime += Time.deltaTime;


		timeSinceLastPowerup += Time.deltaTime;

		if (timerRunning) {
			gameTimer -= Time.deltaTime;
		}	
		if (teamOneScore >= scorePlayedTo && teamOneScore == teamTwoScore + 1) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (1);
		} else if (teamTwoScore >= scorePlayedTo && teamTwoScore == teamOneScore + 1) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (2);
		}
		if (teamOneScore >= scorePlayedTo && teamOneScore > teamTwoScore + 1) {
			//Debug.Log ("Run team one wins routine here");
			teamWins (1);
		} else if (teamTwoScore >= scorePlayedTo && teamTwoScore > teamOneScore + 1){
		//	Debug.Log ("Run team two wins routine here");
			teamWins (2);
		}
			
		if (!isGameOver) {
			ConsiderAPowerup ();
		}

//		if (Input.GetKeyDown (KeyCode.P)) {
//			Pause ();
//		}
	}

	public void Pause(){
		if (!paused) {
			pausePanel.SetActive (true);
			es.SetSelectedGameObject(null);
			es.SetSelectedGameObject(es.firstSelectedGameObject);
			Time.timeScale = 0;
			paused = true;
		} 
	}

	public void Unpause(){
		if (paused){
			Time.timeScale = 1;
			paused = false;
			pausePanel.SetActive (false);
		}
	}

	public void QuitGame(){
		Application.LoadLevel ("TitleScene");
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
}
