using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManagerScript : MonoBehaviour {

    // Store a style of ball to spawn
    public GameObject ballPrefab;

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

	// Logic for what game manager should do. If these are turned off it is probably to allow challenge manager to handle things.
	public bool handleBalls = true;
	public bool handleArenas = true;
    public bool handleScore = true;

    // Testing this easy mode thing
    public bool easyMode = false;

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

    public GameObject[] Arenas;

    public bool TrainingMode = false;
	public GameObject CurrentArena;

	public string startButton1 = "Start_P1";
	public string startButton2 = "Start_P2";
	public string startButton3 = "Start_P3";
	public string startButton4 = "Start_P4";

    public GameObject AIControllerPrefab;

	public EventSystem es;

	// Static singleton property
	public static GameManagerScript Instance { get; private set; }

    // Initialization
	void Awake()
	{
		// Save a reference to the AudioHandler component as our singleton instance
		Instance = this;

		locked = false;

        easyMode = DataManagerScript.easyMode;

        if (!DataManagerScript.isSinglePlayerMode && !TrainingMode)
        {
            Player1.GetComponent<PlayerController>().playerType = DataManagerScript.playerOneType;
            Player2.GetComponent<PlayerController>().playerType = DataManagerScript.playerTwoType;
            Player3.GetComponent<PlayerController>().playerType = DataManagerScript.playerThreeType;
            Player4.GetComponent<PlayerController>().playerType = DataManagerScript.playerFourType;
        }

        if (!GameObject.Find("ChallengeManager") && !TrainingMode)
        {
            MusicManagerScript.Instance.StartRoot();
        }
		launchTimer ();
		timeSinceLastPowerup = 0f;
		soloModeBalls = 3;
		readyForReplay = false;

		if (handleBalls) {
            SpawnNewBall();
		}
		if (winText != null){
			winText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		}

		rallyCount = 0;

        // Fade in
        if (GameObject.Find("FadeCurtainCanvas"))
        {
            GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(0f);
        }

        // Set up arena based on options
        arenaType = DataManagerScript.arenaType;

        // If no arena is selected (Solo mode, testing, etc., just choose the balanced arena (2)

        if (arenaType <= 0)
        {
            arenaType = 2;
        }

        if (handleArenas) {
            Debug.Log("what is the arena type?");
            Debug.Log(DataManagerScript.arenaType);
            Arenas[arenaType - 1].SetActive(true);
		}

		int playersActive = 0;
		int whichSoloPlayer = 0;

        if (!TrainingMode)
        {
            // make this a common function in a class
            if (DataManagerScript.playerOnePlaying == true)
            {
                Player1.SetActive(true);
                playersActive++;
                whichSoloPlayer = 1;
            }
            if (!DataManagerScript.isSinglePlayerMode)
            {
                if (DataManagerScript.playerTwoPlaying == true)
                {
                    Player2.SetActive(true);
                    playersActive++;
                    whichSoloPlayer = 2;
                }
                if (DataManagerScript.playerThreePlaying == true)
                {
                    Player3.SetActive(true);
                    playersActive++;
                    whichSoloPlayer = 3;
                }
                if (DataManagerScript.playerFourPlaying == true)
                {
                    Player4.SetActive(true);
                    playersActive++;
                    whichSoloPlayer = 4;
                }
            }
            Debug.Log("how many players active?");
            Debug.Log(playersActive);
            Debug.Log("Does data manager tyhink this is single player mode?");
            Debug.Log(DataManagerScript.isSinglePlayerMode);

            if (playersActive == 1)
            {

                OnePlayerMode = true;
                InstantiateClone(whichSoloPlayer);
                // set this somewhere else
                soloMode = true;
                rallyCountText.gameObject.SetActive(true);

            }
            else
            {
                OnePlayerMode = false;
            }
        }

	}
    void Start()
    {
        Debug.Log("challenge mode?");
        Debug.Log(DataManagerScript.isChallengeMode);
        if (!DataManagerScript.isChallengeMode)
        {
            CurrentArena = GameObject.FindWithTag("Arena");
        }
        else
        {

            CurrentArena = GameObject.FindWithTag("Arena");
            Debug.Log(CurrentArena);
            //GameObject.FindWithTag("Arena");
        }

        // Assign AI to bot players.

        if (DataManagerScript.isBotsMode)
        {
            //Player2.GetComponent<PlayerController>().isAI = true;
            //GameObject ai = Instantiate(AIControllerPrefab);
            //ai.transform.Find("AIManager").GetComponent<ManualAIScript>().playerBeingControlled = Player2;
            //ai.transform.Find("AIManager").GetComponent<ManualAIScript>().allowGravityChanges = true;

            Player3.GetComponent<PlayerController>().isAI = true;
            GameObject aic = Instantiate(AIControllerPrefab);

            string whichAIManager = "AIManager-default";

            if (Player3.GetComponent<PlayerController>().playerType == 1)
            {
                aic.transform.GetChild(1).gameObject.SetActive(true);
                aic.transform.GetChild(0).gameObject.SetActive(false);
                whichAIManager = "AIManager-circle";
            }

            if (Player3.GetComponent<PlayerController>().playerType == 2)
            {
                aic.transform.GetChild(2).gameObject.SetActive(true);
                aic.transform.GetChild(0).gameObject.SetActive(false);
                whichAIManager = "AIManager-triangle";
            }

            aic.transform.Find(whichAIManager).GetComponent<ManualAIScript>().playerBeingControlled = Player3;

            aic.transform.Find(whichAIManager).GetComponent<ManualAIScript>().allowGravityChanges = true;


            if (DataManagerScript.playerFourPlaying)
            {
                string whichAIManagerTwo = "AIManager-default";
               
                aic.transform.Find(whichAIManager).GetComponent<ManualAIScript>().allowGravityChanges = false;
                Player4.GetComponent<PlayerController>().isAI = true;
                GameObject aic_two = Instantiate(AIControllerPrefab);
                if (Player4.GetComponent<PlayerController>().playerType == 1)
                {
                    aic_two.transform.GetChild(1).gameObject.SetActive(true);
                    aic_two.transform.GetChild(0).gameObject.SetActive(false);
                    whichAIManagerTwo = "AIManager-circle";
                }
                if (Player4.GetComponent<PlayerController>().playerType == 2)
                {
                    aic_two.transform.GetChild(2).gameObject.SetActive(true);
                    aic_two.transform.GetChild(0).gameObject.SetActive(false);
                    whichAIManagerTwo = "AIManager-triangle";
                }
                aic_two.transform.Find(whichAIManagerTwo).GetComponent<ManualAIScript>().playerBeingControlled = Player4;
                aic_two.transform.Find(whichAIManagerTwo).GetComponent<ManualAIScript>().allowGravityChanges = false;
            }
        }
        // set other options here

    }

    void OnBallDied(int whichSide)
    {
        // These flags suck. Eventually pare down GameManager into components such as 'Ball Respawner' or whatever
        if (handleBalls)
        {
            Debug.Log("Game manager knows the ball has died");
            Debug.Log("Is the game over?");
            Debug.Log(isGameOver);
            // Received a message from the ball. It died. Spawn a new one if the game is still going.
            if (!isGameOver)
            {
                Invoke("SpawnNewBall", 1f);
            }
        }

        }

    void SpawnNewBall()
    {
        // Only spawn if game is still going.
        if (!isGameOver)
        {

            GameObject newBall = Instantiate(ballPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            newBall.transform.parent = gameObject.transform.parent;
            IEnumerator coroutine_1 = newBall.GetComponent<BallScript>().LaunchBallWithDelay(2f);
            StartCoroutine(coroutine_1);
            // set ball's gravChangeMode to true;
            if (easyMode)
            {
                Debug.Log("Easy mode: setting gravchange mode to false");
                newBall.GetComponent<BallScript>().gravChangeMode = false;
                newBall.GetComponent<BallScript>().startWithRandomGrav = false;
            }
            else
            {
                Debug.Log("setting gravchange mode to true");
                newBall.GetComponent<BallScript>().gravChangeMode = true;
            }

            //TODO: Is there a better way to store these settins, which will be different for each mode?

            if (handleScore)
            {
                newBall.GetComponent<BallScript>().scoringMode = true;
            }

            // Add a random grav sign based on flag
            newBall.GetComponent<BallScript>().startWithRandomGrav = true;
            ball = newBall;
        }
    }

    void LaunchBall(){
		ball.GetComponent<BallScript>().LaunchBall ();
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
		playerClone.GetComponent<PlayerController>().playerID = whichSoloPlayer;
		playerClone.GetComponent<PlayerController>().playerType = playerType;
		playerClone.GetComponent<MeshRenderer> ().material = whichMat;
        // Special case for circle shape
		if (playerType == 1) {
			playerClone.transform.Find ("Circle").GetComponent<CircleEfficient> ().Rebuild ();
			playerClone.transform.Find ("Circle").GetComponent<MeshRenderer> ().material = whichMat;
		}

	}

	 void LaunchTitleScreen(){
        StartCoroutine("FadeToTitle");
    }

	void LaunchStatsScreen(){
		StartCoroutine ("FadeToStats");
	}

    IEnumerator FadeToTitle()
    {
        if (!locked)
        {
            locked = true;
            GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadSceneAsync("titleScene");

        }
    }

    IEnumerator FadeToStats(){
		if (!locked) {
			locked = true;
            GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
            yield return new WaitForSeconds(0.5f);
			if (!OnePlayerMode) {
				SceneManager.LoadSceneAsync("statsScene");
			} else {
				SceneManager.LoadSceneAsync("singlePlayerStatsScene");
			}
		}
	}

	// End game for single player only
	public void endGame(){
		isGameOver = true;
		DataManagerScript.rallyCount = rallyCount;
		Invoke ("LaunchStatsScreen", 5f);
	}

    // End game for team maches. TODO: Move to scoreboard manager
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over message received by game manager");
        Invoke("LaunchStatsScreen", 5f);
    }

	void Update () {

        if (OnePlayerMode) {
            rallyCountText.text = rallyCount.ToString();
            if (rallyCount >= 50) { 
                AchievementManagerScript.Instance.Achievements[6].Unlock();
            }
		}
		// if all 4 start buttons are pressed, warp back to title screen
		if (Input.GetButton (startButton1) && Input.GetButton (startButton2) && Input.GetButton (startButton3) && Input.GetButton (startButton4)) {
			Debug.Log ("returning to title");
			SceneManager.LoadSceneAsync("titleScene");
		}

		// keep track of match time
		DataManagerScript.gameTime += Time.deltaTime;
		timeSinceLastPowerup += Time.deltaTime;

		if (timerRunning) {
			gameTimer -= Time.deltaTime;
		}
	}

	public void Pause(JoystickButtons buttons){
		if (!paused) {
			// Show pause
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
        Debug.Log("trying to unpause");
        Debug.Log(paused);
		if (paused){
            Debug.Log("unpausing");
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

    public void QuitGame()
    {
        // Temp switchin this for playtest night
        LaunchTitleScreen();
    }

    void CheckForMatchPoint()
    {
        // check for match point
        if (teamTwoScore == teamOneScore)
        {
            background.GetComponent<BackgroundColorScript>().TurnOffMatchPoint();
            //MusicManagerScript.Instance.SwitchMusic ();
        }
        else if (teamOneScore == scorePlayedTo - 1 && teamTwoScore < scorePlayedTo)
        {
            background.GetComponent<BackgroundColorScript>().TurnOnMatchPoint(1);
            background.GetComponent<BackgroundColorScript>().TurnOffDeuce();
            MusicManagerScript.Instance.StartFifth();
        }
        else if (teamTwoScore == scorePlayedTo - 1 && teamOneScore < scorePlayedTo)
        {
            background.GetComponent<BackgroundColorScript>().TurnOnMatchPoint(2);
            background.GetComponent<BackgroundColorScript>().TurnOffDeuce();
            MusicManagerScript.Instance.StartFifth();
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

        //TODO: TEST THIS STUFF IN STATS MODULE BEFORE REMOVING COMPLETELY
		//DataManagerScript.currentRallyCount += 1;
		//if (DataManagerScript.currentRallyCount > DataManagerScript.longestRallyCount) {
		//	DataManagerScript.longestRallyCount = DataManagerScript.currentRallyCount;
		//	// Debug.Log ("longest rally count is now " + DataManagerScript.longestRallyCount);
		//}

		//// Credit a return to the last touch player
		//switch (lastTouch) {
		//case 1:
		//	DataManagerScript.playerOneReturns += 1;
		//	break;
		//case 2:
		//	DataManagerScript.playerTwoReturns += 1;
		//	break;
		//case 3:
		//	DataManagerScript.playerThreeReturns += 1;
		//	break;
		//case 4:
		//	DataManagerScript.playerFourReturns += 1;
		//	break;
		//}

		if (soloMode && ball.GetComponent<BallScript> ().lastXPos != 0) {
			GameManagerScript.Instance.GetComponent<GameManagerScript>().rallyCount++;
		}

	}
	public void ReturnArenaToOriginalColor(){
		CurrentArena.BroadcastMessage ("ReturnColor");
	}

	public void ManageScore(float ballPosition){
		//if (!soloMode) {
		//	if (Mathf.Sign (ballPosition) < 0) {
		//		teamTwoScore += 1;
		//		ComputeStat (2);
		//		if (lastScore != 2) {
		//			MusicManagerScript.Instance.SwitchMusic (2);
		//		}

		//		lastScore = 2;
		//	} else {
		//		teamOneScore += 1;
		//		ComputeStat (1);

		//		if (lastScore != 1) {
		//			MusicManagerScript.Instance.SwitchMusic (1);
		//		}

		//		lastScore = 1;
		//	}

		//	CurrentArena.BroadcastMessage ("ReturnColor");

		//	if (teamTwoScore < scorePlayedTo && teamOneScore < scorePlayedTo) {
		//		if (teamTwoScore == scorePlayedTo - 1 && teamOneScore == scorePlayedTo - 1) {
		//			scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
		//			background.GetComponent<BackgroundColorScript> ().TurnOnDeuce ();
		//		} else {

		//			scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, false);
		//		}

		//		CheckForMatchPoint ();

		//		ball.GetComponent<BallScript> ().ResetBall ();
		//		//Instantiate(prefab, new Vector3(0f, 0, 0), Quaternion.identity);
		//		//Destroy (gameObject);
		//	} else if (Mathf.Abs (GameManagerScript.Instance.teamOneScore - GameManagerScript.Instance.teamTwoScore) < 2) {
		//		if (GameManagerScript.Instance.teamTwoScore >= GameManagerScript.Instance.scorePlayedTo || GameManagerScript.Instance.teamOneScore >= GameManagerScript.Instance.scorePlayedTo) {
		//			//winByTwoText.CrossFadeAlpha (0.6f, .25f, false);
		//			MusicManagerScript.Instance.StartFifth ();
		//			CheckForMatchPoint ();
		//			scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
		//		}
		//		ball.GetComponent<BallScript> ().ResetBall ();

		//	} else {
		//		// GAME IS OVER
		//		transform.position = new Vector3 (0f, 0f, 0f);
  //              ball.SetActive(false);
  //              isGameOver = true;
		//	}
		//}
		//// If you're in one player mode....
	 //	else {
		//	// single mode
		//	soloModeBalls--;
		//	// Debug.Log ("scored");
		//	// generate a random number between one and two
		//	int randomTrack = Random.Range (1, 3);
		//	MusicManagerScript.Instance.SwitchMusic (randomTrack);
		//	if (soloModeBalls <= 0) {
		//		// GAME IS OVER
		//		transform.position = new Vector3 (0f, 0f, 0f);
		//		gameObject.SetActive (false);
		//		GameManagerScript.Instance.endGame ();
		//	} else {
		//		// Hide ball on game over
		//		ball.SetActive(false);
		//	}
		//}
	}
}


