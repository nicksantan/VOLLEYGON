using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SingleStatManagerScript : MonoBehaviour {

	private string jumpButton1 = "Jump_P1";
	private string gravButton1 = "Grav_P1";
	private string jumpButton2 = "Jump_P2";
	private string gravButton2 = "Grav_P2";
	private string jumpButton3 = "Jump_P3";
	private string gravButton3 = "Grav_P3";
	private string jumpButton4 = "Jump_P4";
	private string gravButton4 = "Grav_P4";

	public Text rallyCountText;
	public Text highScoreText;
	public Text newText;

	string formattedMatchTime;

	public int playersPlaying = 1;
	public int playersReady = 0;

	public static SingleStatManagerScript Instance { get; private set; }
	// Use this for initialization
	List<string> buttons = new List<string>();


		

		

	void Start () {
		GameObject.Find ("FadeCurtainCanvas").GetComponent<NewFadeScript> ().Fade (0f);
		MusicManagerScript.Instance.StartIntro ();


		buttons.Add (jumpButton1);
		buttons.Add (jumpButton2);
		buttons.Add (jumpButton3);
		buttons.Add (jumpButton4);

		buttons.Add (gravButton1);
		buttons.Add (gravButton2);
		buttons.Add (gravButton3);
		buttons.Add (gravButton4);

		// calculate match time
		int minutes = Mathf.FloorToInt(DataManagerScript.gameTime / 60F);
		int seconds = Mathf.FloorToInt(DataManagerScript.gameTime - minutes * 60);
		formattedMatchTime = string.Format("{0:00}:{1:00}", minutes, seconds);
		Debug.Log (formattedMatchTime);


		PopulateStats ();

		LogStats ();
		Invoke ("TimeOutTitle", 30f);

	}
	
	// Update is called once per frame
	void TimeOutTitle(){
		StartCoroutine ("BackToTitle");
	}
	void Update () {
		if (playersReady == playersPlaying) {
			StartCoroutine ("BackToTitle");
		}

		foreach (string buttonName in buttons) {
			if (Input.GetButtonDown (buttonName)) {
				playersReady++;
			}
		}
	}

	public void CheckStartable(){
		
	}

	public void increasePlayerReady(){
		playersReady++;

	}

	public void decreasePlayerReady(){
		playersReady--;
	}

	void LogStats(){
		string logText = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss") + " " + playersPlaying.ToString () + " played a game and the match time was " + formattedMatchTime + ". The longest rally was " + DataManagerScript.longestRallyCount.ToString() + "\n";
		System.IO.File.AppendAllText("playlog.txt", logText);

	}


	IEnumerator BackToTitle(){
		float fadeTime = GameObject.Find ("FadeCurtainCanvas").GetComponent<NewFadeScript> ().Fade (1f);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel ("titleScene");
	}

	void PopulateStats(){

		int[] aces = {
			DataManagerScript.playerOneAces,
			DataManagerScript.playerTwoAces,
			DataManagerScript.playerThreeAces,
			DataManagerScript.playerFourAces
		};

		int[] scores = {
			DataManagerScript.playerOneScores,
			DataManagerScript.playerTwoScores,
			DataManagerScript.playerThreeScores,
			DataManagerScript.playerFourScores
		};

		int[] returns = {
			DataManagerScript.playerOneReturns,
			DataManagerScript.playerTwoReturns,
			DataManagerScript.playerThreeReturns,
			DataManagerScript.playerFourReturns
		};

		int[] bumbles = {
			DataManagerScript.playerOneBumbles,
			DataManagerScript.playerTwoBumbles,
			DataManagerScript.playerThreeBumbles,
			DataManagerScript.playerFourBumbles
		};

		int rallyCount = DataManagerScript.rallyCount;
		//populate matchtime and longest rally counters

		rallyCountText.text = rallyCount.ToString (); 
		int highScore = PlayerPrefs.GetInt ("highscore");
		newText.enabled = false;
		if (rallyCount > highScore) {
			PlayerPrefs.SetInt ("highscore", rallyCount);
			highScore = rallyCount;
			newText.enabled = true;

		}
		highScoreText.text = highScore.ToString ();
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
}
