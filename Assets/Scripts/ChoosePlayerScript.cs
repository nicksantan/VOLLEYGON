﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Rewired;

#if UNITY_XBOXONE
	using Users;
#endif

public class ChoosePlayerScript : MonoBehaviour {

	public Image gutterBG;
    public Text versusText;

    public float timeOfLastActivity;
    public bool fourthBotActive = false;
    public bool soloMode = false;
	public GameObject fakePlayer1;
	public GameObject fakePlayer2;
	public GameObject fakePlayer3;
	public GameObject fakePlayer4;

	public GameObject curtain;

	// public GameObject gamepadIcon1;
	// public GameObject gamepadIcon2;
	// public GameObject gamepadIcon3;
	// public GameObject gamepadIcon4;
	// public GameObject[] gamepadIcons;

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

	private FakePlayerScript[] fakePlayers;
	public static ChoosePlayerScript Instance { get; private set; }

    private string defaultText = "Y: LOG IN";

    public bool isBotsMode = false;

    public GameObject CPUFourLabel;

    // Labels for CPU/Bot mode
    public GameObject CPULabels;
    public GameObject RightPlayerLabels;
    public GameObject CPU3BG;
    public GameObject CPU4BG;
    public GameObject addBotButton;
    public GameObject removeBotButton;


    private Player p1;
    private Player p2;
    private Player p3;
    private Player p4;
    private Player[] players = new Player[4];


    void Awake() {

		Instance = this;
		MusicManagerScript.Instance.whichSource += 1;
		MusicManagerScript.Instance.whichSource = MusicManagerScript.Instance.whichSource % 2;
		MusicManagerScript.Instance.SwitchToSource();

        p1 = ReInput.players.GetPlayer(0);
        p2 = ReInput.players.GetPlayer(1);
        p3 = ReInput.players.GetPlayer(2);
        p4 = ReInput.players.GetPlayer(3);

        players[0] = p1;
        players[1] = p2;
        players[2] = p3;
        players[3] = p4;

    }

    void ToggleFourthBot()
    {
        if (!fourthBotActive)
        {
            // Add CPU4 BG
            CPU4BG.SetActive(true);
            Text dt = CPU4BG.transform.Find("difficultyText").Find("dt").GetComponent<Text>();
            int whichShapeTwo = Mathf.FloorToInt(Random.Range(0, 6));
            DataManagerScript.botTwoType = whichShapeTwo;
            switch (whichShapeTwo)
            {
                case 0:
                    fakePlayer4.transform.Find("Square").gameObject.SetActive(true);
                    dt.text = "HARD";

                    break;
                case 1:
                    fakePlayer4.transform.Find("Circle").gameObject.SetActive(true);
                    dt.text = "NORMAL";
                    break;
                case 2:
                    fakePlayer4.transform.Find("Triangle").gameObject.SetActive(true);
                    dt.text = "NORMAL";
                    break;
                case 3:
                    fakePlayer4.transform.Find("Trapezoid").gameObject.SetActive(true);
                    dt.text = "EASY";
                    break;
                case 4:
                    fakePlayer4.transform.Find("Rectangle").gameObject.SetActive(true);
                    dt.text = "HARD";
                    break;
                case 5:
                    fakePlayer4.transform.Find("Star").gameObject.SetActive(true);
                    dt.text = "NORMAL";
                    break;
            }

            DataManagerScript.playerFourPlaying = true;
            DataManagerScript.playerFourType = DataManagerScript.botTwoType;
            CPUFourLabel.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
            fourthBotActive = true;
            addBotButton.SetActive(false);
            removeBotButton.SetActive(true);
        } else if (fourthBotActive)
        {
            // Add CPU4 BG
            CPU4BG.SetActive(false);
            switch (DataManagerScript.botTwoType)
            {
                case 0:
                    fakePlayer4.transform.Find("Square").gameObject.SetActive(false);

                    break;
                case 1:
                    fakePlayer4.transform.Find("Circle").gameObject.SetActive(false);
                    break;
                case 2:
                    fakePlayer4.transform.Find("Triangle").gameObject.SetActive(false);
                    break;
                case 3:
                    fakePlayer4.transform.Find("Trapezoid").gameObject.SetActive(false);
                    break;
                case 4:
                    fakePlayer4.transform.Find("Rectangle").gameObject.SetActive(false);
                    break;
                case 5:
                    fakePlayer4.transform.Find("Star").gameObject.SetActive(false);
                    break;
            }


            DataManagerScript.playerFourPlaying = false;
           
            CPUFourLabel.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);
            fourthBotActive = false;
            addBotButton.SetActive(true);
            removeBotButton.SetActive(false);
        }
        CheckStartable();

}

	void Start(){
        isBotsMode = DataManagerScript.isBotsMode;
        LogActivity();
        // Activate certain labels if this is bots mode
        if (isBotsMode)
        {
           // addBotButton.SetActive(true); //Arcade hack
            // TODO: Choose how many CPUs
            CPULabels.SetActive(true);
            RightPlayerLabels.SetActive(false);
            CPU3BG.SetActive(true);
            Text dt = CPU3BG.transform.Find("difficultyText").Find("dt").GetComponent<Text>();

            //Randomly choose bots.
            int whichShapeOne = Mathf.FloorToInt(Random.Range(0, 6));
            DataManagerScript.botOneType = whichShapeOne;
            switch (whichShapeOne)
            {
                case 0:
                    fakePlayer3.transform.Find("Square").gameObject.SetActive(true);
                    dt.text = "HARD";
                    break;
                case 1:
                    fakePlayer3.transform.Find("Circle").gameObject.SetActive(true);
                    dt.text = "NORMAL";
                    break;
                case 2:
                    fakePlayer3.transform.Find("Triangle").gameObject.SetActive(true);
                    dt.text = "NORMAL";
                    break;
                case 3:
                    fakePlayer3.transform.Find("Trapezoid").gameObject.SetActive(true);
                    dt.text = "EASY";
                    break;
                case 4:
                    fakePlayer3.transform.Find("Rectangle").gameObject.SetActive(true);
                    dt.text = "HARD";
                    break;
                case 5:
                    fakePlayer3.transform.Find("Star").gameObject.SetActive(true);
                    dt.text = "NORMAL";
                    break;
            }

            //Randomly choose bots.

            if (DataManagerScript.numBots == 2)
            {
                // Add CPU4 BG
                CPU4BG.SetActive(true);
                int whichShapeTwo = Mathf.FloorToInt(Random.Range(0, 6));
                DataManagerScript.botTwoType = whichShapeTwo;
                switch (whichShapeTwo)
                {
                    case 0:
                        fakePlayer4.transform.Find("Square").gameObject.SetActive(true);

                        break;
                    case 1:
                        fakePlayer4.transform.Find("Circle").gameObject.SetActive(true);
                        break;
                    case 2:
                        fakePlayer4.transform.Find("Triangle").gameObject.SetActive(true);
                        break;
                    case 3:
                        fakePlayer4.transform.Find("Trapezoid").gameObject.SetActive(true);
                        break;
                    case 4:
                        fakePlayer4.transform.Find("Rectangle").gameObject.SetActive(true);
                        break;
                    case 5:
                        fakePlayer4.transform.Find("Star").gameObject.SetActive(true);
                        break;
                }
            }

            //fakePlayer3.transform.Find("Star").gameObject.SetActive(true);
            //fakePlayer4.transform.Find("Square").gameObject.SetActive(true);
        }

		gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(0.0f);
        versusText.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
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

        if (isBotsMode)
        {
            DataManagerScript.playerThreePlaying = true;
            DataManagerScript.playerThreeType = DataManagerScript.botOneType;

            if (DataManagerScript.numBots == 2)
            {
                DataManagerScript.playerFourPlaying = true;
                DataManagerScript.playerFourType = DataManagerScript.botTwoType;
                Debug.Log("playerfourtype is");
                Debug.Log(DataManagerScript.playerFourType);
            } else
            {
                CPUFourLabel.GetComponent<Text>().color = new Color(0f, 0f,0f,0f);
            }
        }

		// Fade in
		curtain.SetActive(true);
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 0f, .5f);

		// Make array of icons, usernames, and fake players
        usernames = new GameObject[4] { userText1, userText2, userText3, userText4 };
        fakePlayers = new FakePlayerScript[4] { 
            fakePlayer1.GetComponent<FakePlayerScript>(), 
            fakePlayer2.GetComponent<FakePlayerScript>(), 
            fakePlayer3.GetComponent<FakePlayerScript>(), 
            fakePlayer4.GetComponent<FakePlayerScript>()
        };

        // Loop over icons and activate any already active in menu
        for (int gamepadId = 0; gamepadId < fakePlayers.Length; gamepadId++)
        {
            // Activate slot for player who selected the game mode
            if (DataManagerScript.gamepadControllingMenus == gamepadId)
            {
                fakePlayers[gamepadId].checkForJoystick();
                fakePlayers[gamepadId].activateReadyState();
            }
        }
    }

    // See if all players that are tagged in have also readied up
	bool noUnreadyPlayers(){
        // Only need two humans for bots mode.
        int totalHumans = isBotsMode ? 2 : 4;

        for (int i = 0; i < totalHumans; i++)
        {
            if (fakePlayers[i].taggedIn && !fakePlayers[i].readyToPlay)
            {
                return false;
            }
        }

        return true;
	}

    int numberOfTaggedInPlayers()
    {
        int num = 0;

        for (int i = 0; i < fakePlayers.Length; i++)
        {
            if (fakePlayers[i].taggedIn)
            {
                num++;
            }
        }

        return num;
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


        if (isBotsMode)
        { 
        playersOnRight++;
            if (DataManagerScript.playerFourPlaying)
            {
                playersOnRight++;
            }


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
            versusText.GetComponent<Text>().color = new Color(0f,0f,0f, 1f);
            if (playersOnLeft == 2 && playersOnRight == 1 || playersOnLeft == 1 && playersOnRight == 2) {

                // Display 2v1 message
                // TODO: Msg bg is shown before 4 player start, which it shouldn't. It's dumb that there are two images, they should be a group.
                if (playersOnLeft == 2 && isBotsMode)
                {
                }
                else
                {
                    msgBG.enabled = true;
                    msgBG2.enabled = true;
                    twoOnOneMessage.enabled = true;
                }
                oneOnOneMessage.enabled = false;
                onePlayerMessage.enabled = false;
				gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(1.0f);
                versusText.GetComponent<Text>().color = new Color(0f, 0f, 0f, 1f);

            } else if (playersOnLeft == 1 && playersOnRight == 1){

				// Display 1v1 message
				// TODO: Msg bg is shown before 4 player start, which it shouldn't. It's dumb that there are two images, they should be a group.
				msgBG.enabled = true;
				msgBG2.enabled = true;
				oneOnOneMessage.enabled = true;
				twoOnOneMessage.enabled = false;
				onePlayerMessage.enabled = false;
				gutterBG.GetComponent<CanvasRenderer> ().SetAlpha(1.0f);
                versusText.GetComponent<Text>().color = new Color(0f, 0f, 0f, 1f);

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
            versusText.GetComponent<Text>().color = new Color(1f,1f,1f, 1f);

        }
	}

	void StartGame(){
		if (!locked) {
			locked = true;
            GameObject curtain = GameObject.Find("FadeCurtainCanvas");
            LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { 
                Debug.Log("Solo mode?");
                Debug.Log(DataManagerScript.isSinglePlayerMode);
                if (!DataManagerScript.isSinglePlayerMode)
                {
                    SceneManager.LoadSceneAsync("chooseArenaScene");
                }
                else
                {
                    DataManagerScript.playerTwoPlaying = false;
                    DataManagerScript.playerThreePlaying = false;
                    DataManagerScript.playerFourPlaying = false;
                    SceneManager.LoadSceneAsync("soloGameScene");
                }
            });
        }
	}

	void exitIfNoOtherGamepads() {

		// See if any slots besides this one are active
		for (int i = 0; i < fakePlayers.Length; i++) {
			if (fakePlayers[i].taggedIn) {
				return;
			}
		}

		// If no slots were active, return to title
        Debug.Log("back to title");
		BackToTitle();
	}

	void BackToTitle(){
		if (!locked) {
			locked = true;
            GameObject curtain = GameObject.Find("FadeCurtainCanvas");
			LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("titleScene"); });
		}
	}
    
    // void HideNonTaggedInPlayers()
    // {
    //     for (int i = 0; i < players.Length; i++)
    //     {
    //         if (!gamepadIcons[i].GetComponent<GamepadController>().enabled)
    //         {
    //             gamepadIcons[i].GetComponent<GamepadController>().HideIcon();
    //         }
    //     }
    // }

    // public void ShowNonTaggedInPlayers()
    // {
    //     for (int i = 0; i < players.Length; i++)
    //     {
    //         if (!gamepadIcons[i].GetComponent<GamepadController>().enabled)
    //         {
    //             gamepadIcons[i].GetComponent<GamepadController>().ShowIcon();
    //         }
    //     }
    // }

    // Update is called once per frame
    void Update () {

        // check for idle time out
        if (Time.time - timeOfLastActivity > 20f){
            // go back to title
            BackToTitle();
        }
		// Look for start button presses
		for (int i = 0; i < players.Length; i++) {
			int slotId = i + 1;
			//JoystickButtons joystick = joysticks[i];

           
            // If this is bots mode, let any player toggle the fourth bot
          
            if (players[i].GetButtonDown("Supplementary") && isBotsMode)
            {
                Debug.Log("y hit");
                ToggleFourthBot();
            }
			if (players[i].GetButtonDown("Jump") && !fakePlayers[i].taggedIn) {

                LogActivity();

				// Start game if startable and gamepad not tagged in
				if (gameIsStartable && fakePlayers[i].taggedIn) {
                    StartGame();
                }
				else if (!fakePlayers[i].taggedIn) {

                    // Tag in gamepad if not
                    if (isBotsMode) // Check if max slots have been exceeded first
                    {
                        if (numberOfTaggedInPlayers() < 2)
                        {
                            // if (numberOfTaggedInPlayers() == 1)
                            // {
                            //     HideNonTaggedInPlayers(); 
                            // }
                            
                            fakePlayers[i].checkForJoystick();
                            fakePlayers[i].activateReadyState();
                        }
                    }
                    else
                    {
                        fakePlayers[i].checkForJoystick();
                        fakePlayers[i].activateReadyState();
                    }

				}
			}
            if (players[i].GetButtonDown("Start") && gameIsStartable)
            {
                StartGame();
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

        // go ahead and start if this is a vs. ai game and both players on left are ready
        if (DataManagerScript.isBotsMode && (fakePlayer1.GetComponent<FakePlayerScript>().readyToPlay
            && fakePlayer2.GetComponent<FakePlayerScript>().readyToPlay))
        {
            StartCoroutine("StartGame");
        }

    }

	void FixedUpdate() {
		// Back out if no gamepads
		exitIfNoOtherGamepads();
	}

    public void LogActivity(){
        // Debug.Log("time of last activity is now");
        // Debug.Log(Time.time);
        timeOfLastActivity = Time.time;
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
