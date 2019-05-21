using UnityEngine;
using System.Collections;

public class DataManagerScript : MonoBehaviour {

	public static DataManagerScript dataManager;

	//public static string version; 
	public int teamOneWins;
	public int teamTwoWins;
	public static bool playerOnePlaying = true;
	public static bool playerTwoPlaying = true;
	public static bool playerThreePlaying = true;
	public static bool playerFourPlaying = true;
	public static bool CRTMode = true;

	public static int playerOneType;
	public static int playerTwoType;
	public static int playerThreeType;
	public static int playerFourType;
	public static int arenaType;

	// Arcade / credit-mode variables
	public static bool creditMode = false;
	public static int credits;
	private static string coinInsertButton = "Coin_Insert";
	public static AudioClip coinInsertSFX;
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
	void Awake(){
		if (dataManager == null) {
			DontDestroyOnLoad (gameObject);
			dataManager = this;
		} else if (dataManager != this){
			Destroy (gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		version = "V1.7a";
		//xboxMode = true;
		credits = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Check for inserted coin across all scenes
		if (Input.GetButtonDown (coinInsertButton) && creditMode) {
			credits += 1;
			Debug.Log ("COIN INSERT");
			// play a credit sfx here
			MusicManagerScript.Instance.CoinInsert();
		};
	}



	public static void ResetPlayerTypes(){
		playerOneType = 0;
		playerTwoType = 0;
		playerThreeType = 0;
		playerFourType = 0;

	}
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
