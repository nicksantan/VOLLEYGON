using UnityEngine;
using System.Collections;

public class ScoreboardManagerScript : MonoBehaviour {

	public GameObject Team1Scores;
	public GameObject Team2Scores;
	public GameObject Team1Wins;
	public GameObject Team2Wins;

	public GameObject deuce;
	public GameObject dash;
	public bool scoreBoardShowing; 
	// Use this for initialization
	void Start () {
		

		scoreBoardShowing = false;
		for (int i = 0; i < Team1Scores.transform.childCount; i++) {
			Transform child = Team1Scores.transform.GetChild (i);
			//child.gameObject.SetActive (false);
			iTween.FadeTo (child.gameObject, 0f, .1f);
		
		}

		for (int j = 0; j < Team2Scores.transform.childCount; j++) {
			//	Debug.Log ("trying to hide");
			Transform childTwo = Team2Scores.transform.GetChild (j);
		
			iTween.FadeTo (childTwo.gameObject, 0f, .1f);
		
		}
		Invoke ("moveIntoPlace", 1f);
		Invoke ("cleanUp", 1f); 
		//cleanUp ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void moveIntoPlace(){
		gameObject.transform.position = new Vector3 (0f, 0f, 0f);
	}

	public void TeamOneWin(){
		Team1Wins.gameObject.SetActive (true);
		MusicManagerScript.Instance.StartRoot ();
		scoreBoardShowing = true;
	}

	public void TeamTwoWin(){
		Team2Wins.gameObject.SetActive (true);
		MusicManagerScript.Instance.StartRoot ();
		scoreBoardShowing = true;
	}

	public void enableNumbers (int team1Score, int team2Score, bool overtime){
	
		if (!overtime) {
			Transform theNumOne = Team1Scores.transform.Find (team1Score.ToString ());
			theNumOne.gameObject.SetActive (true);
			iTween.FadeTo (theNumOne.gameObject, 0.8f, .25f);

			Transform theNumTwo = Team2Scores.transform.Find (team2Score.ToString ());
			theNumTwo.gameObject.SetActive (true);
			iTween.FadeTo (theNumTwo.gameObject, 0.8f, .25f);
			enableDash ();
		} else {
			if (team1Score > team2Score) {
				Transform theNumOne = Team1Scores.transform.Find ("A");
				theNumOne.gameObject.SetActive (true);
				iTween.FadeTo (theNumOne.gameObject, 0.8f, .25f);

				Transform theNumTwo = Team2Scores.transform.Find ("D");
				theNumTwo.gameObject.SetActive (true);
				iTween.FadeTo (theNumTwo.gameObject, 0.8f, .25f);
			} else if (team1Score < team2Score) {
				Transform theNumOne = Team1Scores.transform.Find ("D");
				theNumOne.gameObject.SetActive (true);
				iTween.FadeTo (theNumOne.gameObject, 0.8f, .25f);

				Transform theNumTwo = Team2Scores.transform.Find ("A");
				theNumTwo.gameObject.SetActive (true);
				iTween.FadeTo (theNumTwo.gameObject, 0.8f, .25f);
			} else if (team1Score == team2Score) {
				
				deuce.SetActive (true);
				iTween.FadeTo (deuce, 0.8f, .25f);
				MusicManagerScript.Instance.StartFourth ();
			}
		}		

	
		scoreBoardShowing = true;
	

		Invoke ("FadeOutScore", 2f);
	}

	public void FadeOutScore(){
		//for now just turn everything off
		disableEverything ();
	}
	public void enableDash(){
		dash.SetActive (true);
		iTween.FadeTo (dash, .8f, .25f);
	}


	public void cleanUp(){
		for (int i = 0; i < Team1Scores.transform.childCount; i++) {
			Transform child = Team1Scores.transform.GetChild (i);
			child.gameObject.SetActive (false);
		//	iTween.FadeTo (child.gameObject, 0f, .1f);
		}

		for (int j = 0; j < Team2Scores.transform.childCount; j++) {
		//	Debug.Log ("trying to hide");
			Transform childTwo = Team2Scores.transform.GetChild (j);
			childTwo.gameObject.SetActive (false);
		//	iTween.FadeTo (childTwo.gameObject, 0f, .1f);
		}
		dash.SetActive (false);
	}
	public void disableEverything(){
		iTween.FadeTo (dash, 0f, .25f);
		iTween.FadeTo (deuce, 0f, .25f);
		for (int i = 0; i < Team1Scores.transform.childCount; i++) {
			Transform child = Team1Scores.transform.GetChild (i);
			//if (child.gameObject.activeSelf) {
				iTween.FadeTo (child.gameObject, 0f, .25f);
			//}
			//child.gameObject.SetActive (false);
		}
		for (int j = 0; j < Team2Scores.transform.childCount; j++) {
		//	Debug.Log ("trying to hide");
			Transform childTwo = Team2Scores.transform.GetChild (j);

				iTween.FadeTo (childTwo.gameObject, 0f, .25f);

			//childTwo.gameObject.SetActive (false);
		}
		Invoke ("cleanUp", .5f);

		scoreBoardShowing = false;
	}

}
