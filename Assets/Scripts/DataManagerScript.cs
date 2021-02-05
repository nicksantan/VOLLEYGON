using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
	using Users;
#endif

public class DataManagerScript : MonoBehaviour {

	public static DataManagerScript dataManager;

    //public static string version;

    // Universal game variables
    public static bool easyMode = false;
	public int teamOneWins;
	public int teamTwoWins;
	public static bool playerOnePlaying = true;
	public static bool playerTwoPlaying = true;
	public static bool playerThreePlaying = true;
	public static bool playerFourPlaying = true;
	public static bool CRTMode = true;
	public static bool isChallengeMode = false;
    public static bool isSinglePlayerMode = false;
    public static bool isFirstPlay = true;

	// Flags for user login events

	public static bool shouldActivateMenu = false;
	public static int slotToUpdate = -1;

	// UI Flags

	public static int gamepadControllingMenus = 0;
	#if UNITY_XBOXONE
		public static Users.User userControllingMenus;
	#endif

    // Player shapes

	public static int playerOneType;
	public static int playerTwoType;
	public static int playerThreeType;
	public static int playerFourType;

    // Player joysticks (default to nonsense int)

    public static int playerOneJoystick = -1;
    public static int playerTwoJoystick = -1;
    public static int playerThreeJoystick = -1;
    public static int playerFourJoystick = -1;

    // Player stats

    public static int playerOneAces;
	public static int playerOneReturns;
	public static int playerOneBumbles;
	public static int playerOneScores;

	public static int playerTwoAces;
	public static int playerTwoReturns;
	public static int playerTwoBumbles;
	public static int playerTwoScores;

	public static int playerThreeAces;
	public static int playerThreeReturns;
	public static int playerThreeBumbles;
	public static int playerThreeScores;

	public static int playerFourAces;
	public static int playerFourReturns;
	public static int playerFourBumbles;
	public static int playerFourScores;

    // Arena type
    public static int arenaType;

    // Challenge type (for single player challenge mode)
    public static int challengeType;

    // Match stats

	public static int longestRallyCount;
	public static int matchTime;
	public static int currentRallyCount;
	public static int rallyCount;
    public static int soloRallyCount;

	public static float gameTime;

    public static int whichTeamWon = 0;
	// Tournament mode variables

	public bool tournamentMode;
	public static int TM_TeamOneWins;
	public static int TM_TeamTwoWins;

	public static string version;
	public static bool gamepadMode = true;
	public static bool xboxMode = false;
	public static bool demoMode = false;

    public static int lastViewedChallenge = 0;

    public static bool isBotsMode = false;
    public static int botOneType = 0;
    public static int botTwoType = 0;
    public static int numBots = 0;

    // Save instance of self over scene loads
	void Awake() {

		if (dataManager == null) {
			DontDestroyOnLoad (gameObject);
			dataManager = this;
		} else if (dataManager != this){
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {

        // Version number
        // TODO: Uncomment public property instead?
		version = "V1.8.8";

        // Determine if on Xbox
        xboxMode = Application.platform == RuntimePlatform.XboxOne;
		if (xboxMode) {
			gamepadMode = true;
		}

	}
 
    // Reset all player shape choices
    public static void ResetPlayerTypes(){
		playerOneType = 0;
		playerTwoType = 0;
		playerThreeType = 0;
		playerFourType = 0;
	}

    // Reset all plater stats AND global match stats
	public static void ResetStats(){
		 playerOneAces = 0;
		 playerOneReturns = 0;
		 playerOneBumbles = 0;
		 playerOneScores = 0;

		 playerTwoAces = 0;
		 playerTwoReturns = 0;
		 playerTwoBumbles = 0;
		 playerTwoScores = 0;

		 playerThreeAces = 0;
		 playerThreeReturns = 0;
		 playerThreeBumbles = 0;
		 playerThreeScores = 0;

		 playerFourAces = 0;
		 playerFourReturns = 0;
		 playerFourBumbles = 0;
		 playerFourScores = 0;

		longestRallyCount = 0;
		currentRallyCount = 0;

		gameTime = 0;

		rallyCount = 0;
        soloRallyCount = 0;
	}

}
