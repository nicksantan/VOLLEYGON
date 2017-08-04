using UnityEngine;
using System.Collections;

public class DataManagerScript : MonoBehaviour {

	public static DataManagerScript dataManager;

	//public static string version; 

    // Universal game variables

	public int teamOneWins;
	public int teamTwoWins;
	public static bool playerOnePlaying = true;
	public static bool playerTwoPlaying = true;
	public static bool playerThreePlaying = true;
	public static bool playerFourPlaying = true;
	public static bool CRTMode = true;

    // Player shapes

	public static int playerOneType;
	public static int playerTwoType;
	public static int playerThreeType;
	public static int playerFourType;

    // Player joysticks

    public static int playerOneJoystick;
    public static int playerTwoJoystick;
    public static int playerThreeJoystick;
    public static int playerFourJoystick;

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

    // Arena type
    public static int arenaType;

    // Match stats

    public static int playerFourScores;

	public static int longestRallyCount;
	public static int matchTime;
	public static int currentRallyCount;
	public static int rallyCount;

	public static float gameTime;

	// Tournament mode variables

	public bool tournamentMode;
	public static int TM_TeamOneWins;
	public static int TM_TeamTwoWins;

	public static string version;
	public static bool xboxMode = true;

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
		version = "V1.5.6";

        // Determine if on Xbox
        // TODO: make dynamic 
        xboxMode = false;

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
	}
		
}
