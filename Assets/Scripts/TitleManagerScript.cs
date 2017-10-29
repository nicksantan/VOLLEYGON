using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
	public GameObject mainMenuPanel;
	private bool mainMenuActive = false;

	public EventSystem es1;
	public EventSystem es2;
	public EventSystem es3;
	public EventSystem es4;






	// Use this for initialization
	void Start () {
		MusicManagerScript.Instance.FadeOutEverything ();
		versionText.text = DataManagerScript.version;
		DataManagerScript.ResetStats ();
		DataManagerScript.ResetPlayerTypes ();

		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;

	}
	
	// Update is called once per frame
	void Update () {
		MusicManagerScript.Instance.FadeOutEverything ();

		for (int i = 0; i < buttons.Length; i++) {
			if (Input.GetButtonDown (buttons [i])) {
				if (!mainMenuActive) {
					
					mainMenuActive = true;
					mainMenuPanel.SetActive (true);

					// set the correct event system if necessary here and activate them
					es1.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_P1";
					es1.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_P1";
					es1.GetComponent<StandaloneInputModule>().submitButton = "Jump_P1";
					es1.GetComponent<StandaloneInputModule>().cancelButton = "Grav_P1";
				}
//				Application.LoadLevel ("choosePlayerScene");
			}
		}
	}
}
