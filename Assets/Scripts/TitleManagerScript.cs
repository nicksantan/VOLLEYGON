using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleManagerScript : MonoBehaviour {

	static public string jumpButton1 = "Jump_P1";
	static public string gravButton1 = "Grav_P1";
	static public string jumpButton2 = "Jump_P2";
	static public string gravButton2 = "Grav_P2";
	static public string jumpButton3 = "Jump_P3";
	static public string gravButton3 = "Grav_P3";
	static public string jumpButton4 = "Jump_P4";
	static public string gravButton4 = "Grav_P4";
	static public string startButton1 = "Start_P1";
	static public string startButton2 = "Start_P2";
	static public string startButton3 = "Start_P3";
	static public string startButton4 = "Start_P4";

	// Xbox buttons
	static private string jumpButton1_Xbox = "Jump_P1_Xbox";
	static private string gravButton1_Xbox = "Grav_P1_Xbox";
	static private string startButton1_Xbox = "Start_P1_Xbox";
	static private string jumpButton2_Xbox = "Jump_P2_Xbox";
	static private string gravButton2_Xbox = "Grav_P2_Xbox";
	static private string startButton2_Xbox = "Start_P2_Xbox";
	static private string jumpButton3_Xbox = "Jump_P3_Xbox";
	static private string gravButton3_Xbox = "Grav_P3_Xbox";
	static private string startButton3_Xbox = "Start_P3_Xbox";
	static private string jumpButton4_Xbox = "Jump_P4_Xbox";
	static private string gravButton4_Xbox = "Grav_P4_Xbox";
	static private string startButton4_Xbox = "Start_P4_Xbox";

	private string[] buttons = {jumpButton1, jumpButton2, jumpButton3, jumpButton4, gravButton1, gravButton2, gravButton3, gravButton4, startButton1, startButton2, startButton3, startButton4, jumpButton1_Xbox, jumpButton2_Xbox, jumpButton3_Xbox, jumpButton4_Xbox, gravButton1_Xbox, gravButton2_Xbox, gravButton3_Xbox, gravButton4_Xbox, startButton1_Xbox, startButton2_Xbox, startButton3_Xbox, startButton4_Xbox};
	public Text versionText;
	public Text creditsText;

	public GameObject fcc;

	// Use this for initialization
	void Start () {
		Invoke ("FadeInScreen", 0.5f);
		MusicManagerScript.Instance.FadeOutEverything ();
		versionText.text = DataManagerScript.version;


		UpdateCredits ();

		DataManagerScript.ResetStats ();
		DataManagerScript.ResetPlayerTypes ();

		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
		//WHY DO WE NEED THIS??
	

	}
	void FadeInScreen(){
		fcc.GetComponent<NewFadeScript> ().Fade (0f);	
	}
	
	// Update is called once per frame

	void Update () {
		MusicManagerScript.Instance.FadeOutEverything ();

		for (int i = 0; i < buttons.Length; i++) {
			if (Input.GetButtonDown (buttons [i])) {
				if (DataManagerScript.creditMode && DataManagerScript.credits > 0) {
					DataManagerScript.credits -= 1; 
					Application.LoadLevel ("choosePlayerScene");
				} else if (!DataManagerScript.creditMode) {
					Application.LoadLevel ("choosePlayerScene");
				}
					
			}
		}
        if (DataManagerScript.creditMode)
        {
            UpdateCredits();
        }
	}

	void UpdateCredits(){
		if (DataManagerScript.creditMode) {
			creditsText.text = "CREDITS: " + DataManagerScript.credits;
		} else {
			creditsText.text = "FREE PLAY";
		}
	}
}
