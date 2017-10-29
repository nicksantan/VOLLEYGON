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

	private JoystickButtons joyButts;
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
	public GameObject singlePlayerPanel;
	public GameObject soloModeButton;

	private bool mainMenuActive = false;

	public EventSystem es1;

	public Button firstButton;


	// Use this for initialization
	void Start () {
		MusicManagerScript.Instance.FadeOutEverything ();
		versionText.text = DataManagerScript.version;
		DataManagerScript.ResetStats ();
		DataManagerScript.ResetPlayerTypes ();

		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;

	}

	void Update () {
		MusicManagerScript.Instance.FadeOutEverything ();

		for (int i = 0; i < buttons.Length; i++) {
			if (Input.GetButtonDown (buttons [i])) {

				// If the main menu isn't activated, activate it.
				if (!mainMenuActive) {
					mainMenuActive = true;
					mainMenuPanel.SetActive (true);
					//activate first button (weird ui thing)
					//firstButton.Select();
					print("resetting selected game object");
					es1.SetSelectedGameObject(null);
					es1.SetSelectedGameObject(es1.firstSelectedGameObject);
		
					// get the player number 
					int player = 0;
					if (buttons[i].Contains ("P1")) {
						player = 1;
					};
					if (buttons[i].Contains ("P2")) {
						player = 2;
					};
					if (buttons[i].Contains ("P3")) {
						player = 3;
					};
					if (buttons[i].Contains ("P4")) {
						player = 4;
					};
					// depending on which controller was tagged in, set the input stringes here
					joyButts = new JoystickButtons (player);
					print (joyButts.vertical);
					es1.GetComponent<StandaloneInputModule> ().horizontalAxis = joyButts.horizontal;
					es1.GetComponent<StandaloneInputModule> ().verticalAxis = joyButts.vertical;
					es1.GetComponent<StandaloneInputModule> ().submitButton = joyButts.jump;
					es1.GetComponent<StandaloneInputModule> ().cancelButton = joyButts.grav;
				}

				if (buttons[i] == joyButts.grav) {
					// cancel was pressed
					if (mainMenuActive && !singlePlayerPanel.active) {
						mainMenuActive = false;
						mainMenuPanel.SetActive (false);
						Debug.Log ("Canceling out of main menu");
					} else if (singlePlayerPanel.active) {
						print ("Cancelling out of single player menu");
						es1.SetSelectedGameObject(null);
						es1.SetSelectedGameObject(es1.firstSelectedGameObject);
						singlePlayerPanel.SetActive (false);
						mainMenuPanel.SetActive (true);
					}
				}
			
			}
		
		
		}
		// TODO: Listen for 'cancel' button to close menus here.

	}

	public void SetUpSinglePlayerMenu (){
		es1.SetSelectedGameObject(soloModeButton);
	}
	public void StartMultiplayerGame(){
		Application.LoadLevel ("ChoosePlayerScene");
	}
}
