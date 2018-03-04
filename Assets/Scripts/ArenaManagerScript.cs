using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManagerScript : MonoBehaviour {


	private int markerPos = 0;
	private float[] markerYPositions  = {7.4f, 3.7f, 0f, -3.7f, -7.4f};
	private float[] markerXPositions = { -17.58f, -1.81f };
	private string jumpButton1 = "Jump_P1";
	private string gravButton1 = "Grav_P1";
	private string vertAxis1 = "Vertical_P1";
	private string jumpButton2 = "Jump_P2";
	private string gravButton2 = "Grav_P2";
	private string vertAxis2 = "Vertical_P2";
	private string jumpButton3 = "Jump_P3";
	private string gravButton3 = "Grav_P3";
	private string vertAxis3 = "Vertical_P3";
	private string jumpButton4 = "Jump_P4";
	private string gravButton4 = "Grav_P4";
	private string vertAxis4 = "Vertical_P4";
	private string jumpButton1_Xbox = "Jump_P1_Xbox";
	private string jumpButton2_Xbox = "Jump_P2_Xbox";
	private string jumpButton3_Xbox = "Jump_P3_Xbox";
	private string jumpButton4_Xbox = "Jump_P4_Xbox";

	private string gravButton1_Xbox = "Grav_P1_Xbox";
	private string gravButton2_Xbox = "Grav_P2_Xbox";
	private string gravButton3_Xbox = "Grav_P3_Xbox";
	private string gravButton4_Xbox = "Grav_P4_Xbox";

	public GameObject marker;
	private bool axis1InUse = false;
	private bool axis2InUse = false;
	private bool axis3InUse = false;
	private bool axis4InUse = false;
	private int numberOfArenas = 9;
	private bool locked = false;
	public AudioClip tickUp;
	public AudioClip tickDown;

	private AudioSource audio;

	Axis va1;
	Axis va2;
	Axis va3;
	Axis va4;

	Axis va1_Xbox;
	Axis va2_Xbox;
	Axis va3_Xbox;
	Axis va4_Xbox;

	Axis ha1;
	Axis ha2;
	Axis ha3;
	Axis ha4;

	Axis ha1_Xbox;
	Axis ha2_Xbox;
	Axis ha3_Xbox;
	Axis ha4_Xbox;

	//VertAxis[] verticalAxes;
	List<Axis> verticalAxes = new List<Axis>();
	List<Axis> horizontalAxes = new List<Axis>();

	List<string> buttons = new List<string>();

	// Use this for initialization
	void Start () {
		GameObject.Find ("FadeCurtainCanvas").GetComponent<NewFadeScript> ().Fade (0f);
		audio = GetComponent<AudioSource> ();
		locked = false;

		va1 = new Axis("Vertical_P1");
		va2 = new Axis("Vertical_P2");
		va3 = new Axis("Vertical_P3");
		va4 = new Axis("Vertical_P4");

		va1_Xbox = new Axis("Vertical_P1_Xbox");
		va2_Xbox = new Axis("Vertical_P2_Xbox");
		va3_Xbox = new Axis("Vertical_P3_Xbox");
		va4_Xbox = new Axis("Vertical_P4_Xbox");

		verticalAxes.Add (va1);
		verticalAxes.Add (va2);
		verticalAxes.Add (va3);
		verticalAxes.Add (va4);

		verticalAxes.Add (va1_Xbox);
		verticalAxes.Add (va2_Xbox);
		verticalAxes.Add (va3_Xbox);
		verticalAxes.Add (va4_Xbox);

		ha1 = new Axis("Horizontal_P1");
		ha2 = new Axis("Horizontal_P2");
		ha3 = new Axis("Horizontal_P3");
		ha4 = new Axis("Horizontal_P4");

		ha1_Xbox = new Axis("Horizontal_P1_Xbox");
		ha2_Xbox = new Axis("Horizontal_P2_Xbox");
		ha3_Xbox = new Axis("Horizontal_P3_Xbox");
		ha4_Xbox = new Axis("Horizontal_P4_Xbox");

		horizontalAxes.Add (ha1);
		horizontalAxes.Add (ha2);
		horizontalAxes.Add (ha3);
		horizontalAxes.Add (ha4);

		horizontalAxes.Add (ha1_Xbox);
		horizontalAxes.Add (ha2_Xbox);
		horizontalAxes.Add (ha3_Xbox);
		horizontalAxes.Add (ha4_Xbox);

		buttons.Add (jumpButton1);
		buttons.Add (gravButton1);
		buttons.Add (jumpButton2);
		buttons.Add (gravButton2);
		buttons.Add (jumpButton3);
		buttons.Add (gravButton3);
		buttons.Add (jumpButton4);
		buttons.Add (gravButton4);

		buttons.Add (jumpButton1_Xbox);
		buttons.Add (gravButton1_Xbox);
		buttons.Add (jumpButton2_Xbox);
		buttons.Add (gravButton2_Xbox);
		buttons.Add (jumpButton3_Xbox);
		buttons.Add (gravButton3_Xbox);
		buttons.Add (jumpButton4_Xbox);
		buttons.Add (gravButton4_Xbox);




		Vector3 tempPos = new Vector3( markerXPositions [0],  markerYPositions [0], 1f);
		marker.transform.position = tempPos;
	}

	void IncreasePlayCount(string whichType){
		int tempTotal = PlayerPrefs.GetInt (whichType);
		tempTotal += 1;
		PlayerPrefs.SetInt (whichType, tempTotal);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!locked) {
			foreach (Axis va in verticalAxes) {
				if (Input.GetAxisRaw (va.axisName) < 0) {
					if (va.axisInUse == false) {
						// Call your event function here.
						va.axisInUse = true;
						markerPos++;
						updateMarkerPos ();
						audio.PlayOneShot (tickUp);
					}
				}

				if (Input.GetAxisRaw (va.axisName) > 0) {
					if (va.axisInUse == false) {
						// Call your event function here.
						va.axisInUse = true;
						markerPos--;
						audio.PlayOneShot (tickDown);
						updateMarkerPos ();
					}
				}

				if (Input.GetAxisRaw (va.axisName) == 0) {
					va.axisInUse = false;
				}
			}

			foreach (Axis va in horizontalAxes) {
				if (Input.GetAxisRaw (va.axisName) < 0) {
					if (va.axisInUse == false) {
						// Call your event function here.
						va.axisInUse = true;
						markerPos += 5;
						updateMarkerPos ();
						audio.PlayOneShot (tickUp);

					}
				}

				if (Input.GetAxisRaw (va.axisName) > 0) {
					if (va.axisInUse == false) {
						// Call your event function here.
						va.axisInUse = true;
						markerPos += 5;
						audio.PlayOneShot (tickDown);
						updateMarkerPos ();
					}
				}

				if (Input.GetAxisRaw (va.axisName) == 0) {
					va.axisInUse = false;
				}
			}

			foreach (string butt in buttons) {
				if (Input.GetButtonDown (butt)) {
					// log which arena
					if (markerPos == 0) {
						Debug.Log ("choosing random level");
						DataManagerScript.arenaType = Random.Range (0, numberOfArenas);
						IncreasePlayCount ("randomArenaPlays");

					} else {
						DataManagerScript.arenaType = markerPos;
						IncreasePlayCount ("arena" + markerPos + "Plays");
					}
					StartCoroutine ("NextScene");
				}
			}
		}
	}

	IEnumerator NextScene(){
		locked = true;
		float fadeTime = GameObject.Find ("FadeCurtainCanvas").GetComponent<NewFadeScript> ().Fade (1f);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel ("proTipScene");
	}

	void updateMarkerPos(){
		
		if (markerPos < 0) {
			markerPos = numberOfArenas;
		}
		markerPos = markerPos % (numberOfArenas + 1);
		float posX;
		float posY;

		if (markerPos > 4) {
			posX = markerXPositions [1];
			marker.transform.localScale = new Vector3(-3f, marker.transform.localScale.y, marker.transform.localScale.z) ;
			posY = markerYPositions [markerPos - 5];

		} else {
			posX = markerXPositions [0];
			marker.transform.localScale = new Vector3(3f, marker.transform.localScale.y, marker.transform.localScale.z) ;
			posY = markerYPositions [markerPos];
		}
		Vector3 tempPos = new Vector3(posX, posY, 1f);
//		
		marker.transform.position = tempPos;
		Debug.Log (markerPos);
	}
}
