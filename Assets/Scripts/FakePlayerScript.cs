using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FakePlayerScript : MonoBehaviour {

	public Sprite squareSprite;
	public Sprite circleSprite;
	public Sprite triangleSprite;
	public Sprite trapezoidSprite;

	public Image readyBG;
	public string chooseAxis;
	public string chooseAxis_Xbox;

	public string confirmKey;
	public string cancelKey;
	private string confirmKey_Xbox;
	private string cancelKey_Xbox;
	public int playerIdentifier; 
	public Text readyText;
	public Text toJoinText;
	public Text playerDescription;
	public Text playerDifficulty;

	private bool axisInUse = false;
	public bool readyToPlay;
	public bool taggedIn = false;
	private AudioSource audio;
	public AudioClip tickUp;
	public AudioClip tickDown;
	public AudioClip tagInSound;
	public AudioClip readySound;
	public int thisType = 0;

	public GameObject square;
	public GameObject circle;
	public GameObject triangle;
	public GameObject trapezoid;
	public GameObject rectangle;
	public GameObject star;

	public GameObject ui_triangles;


	private int numberOfPlayerTypes = 6;

	Axis normalAxis;
	Axis xboxAxis;

	SpriteRenderer sr;

	void UpdatePlayerType(int whichType){
		if (!readyToPlay) {
			switch(playerIdentifier){

			case 1:
				DataManagerScript.playerOneType = whichType;
				break;
			case 2:
				DataManagerScript.playerTwoType = whichType;
				break;
			case 3:
				DataManagerScript.playerThreeType = whichType;
				break;
			case 4:
				DataManagerScript.playerFourType = whichType;
				break;
			}
			square.SetActive (false);
			circle.SetActive (false);
			triangle.SetActive (false);
			trapezoid.SetActive (false);
			rectangle.SetActive (false);
			star.SetActive (false);


			if (whichType == 0) {
//				fakePlayer1.GetComponent<MeshFilter> ().mesh = meshType1;
				//change sprite here
				//sr.sprite = squareSprite;
				square.SetActive (true);
				playerDescription.text = "CLASSIC\nDEFENSIVE";
				playerDifficulty.text = "EASY";
			} else if (whichType == 1) {
				//sr.sprite = circleSprite;
				circle.SetActive (true);
				playerDescription.text = "ALL-AROUND\nVERSATILE";
				playerDifficulty.text = "MEDIUM";
				//change sprite here
			} else if (whichType == 2){
				//sr.sprite = triangleSprite;
				triangle.SetActive (true);
				playerDescription.text = "AIRBORNE\nAGGRESSIVE";
				playerDifficulty.text = "HARD";
				//change sprite here
			} else if (whichType == 3){
			//	sr.sprite = trapezoidSprite;
				trapezoid.SetActive (true);
				playerDescription.text = "CRAZY!\nWEIRD!";
				playerDifficulty.text = "EXPERTS ONLY";
				//change sprite here
			} else if (whichType == 4){
				//	sr.sprite = trapezoidSprite;
				rectangle.SetActive (true);
				playerDescription.text = "DEFENSIVE\nSLOW";
				playerDifficulty.text = "EASY";
				//change sprite here
			} else if (whichType == 5){
				//	sr.sprite = trapezoidSprite;
				star.SetActive (true);
				playerDescription.text = "STRONG\nUNPREDICTABLE";
				playerDifficulty.text = "HARD";
				//change sprite here
			}

		}
	}


	void activateReadyState(){
		if (taggedIn) {
			if (!readyToPlay) {
				audio.PlayOneShot (readySound);
			}
			readyToPlay = true;
			ui_triangles.SetActive(false);
			playerDescription.enabled = false;
			playerDifficulty.enabled = false;

	
			if (readyToPlay) {
				readyText.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
				readyBG.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			} else {
				
				readyText.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
				readyBG.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			}
		} else {
			taggedIn = true;

			audio.PlayOneShot (tagInSound);
			toJoinText.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			playerDescription.enabled = true;
			playerDifficulty.enabled = true;
			//sr.enabled = true;
			square.SetActive (false);
			circle.SetActive (false);
			triangle.SetActive (false);
			trapezoid.SetActive (false);
			rectangle.SetActive (false);
			star.SetActive (false);

			ui_triangles.SetActive(true);

			switch (thisType) {
			case 0:
				square.SetActive (true);
				break;
			case 1:
				circle.SetActive (true);
				break;
			case 2:
				triangle.SetActive (true);
				break;
			case 3:
				trapezoid.SetActive (true);
				break;
			case 4:
				rectangle.SetActive (true);
				break;
			case 5:
				star.SetActive (true);
				break;

			}

			switch(playerIdentifier){

			case 1:
				DataManagerScript.playerOnePlaying = true;
				break;
			case 2:
				DataManagerScript.playerTwoPlaying = true;
				break;
			case 3:
				DataManagerScript.playerThreePlaying = true;
				break;
			case 4:
				DataManagerScript.playerFourPlaying = true;
				break;
			}
		}
		ChoosePlayerScript.Instance.CheckStartable ();

	}

	void cancelReadyState(){
		if (readyToPlay) {
			readyToPlay = false;
			readyText.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			readyBG.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			playerDescription.enabled = true;
			playerDifficulty.enabled = true;
			ui_triangles.SetActive(true);
		} else if (taggedIn) {
			taggedIn = false;
			toJoinText.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			sr.enabled = false;
			square.SetActive (false);
			circle.SetActive (false);
			triangle.SetActive (false);
			trapezoid.SetActive (false);
			rectangle.SetActive (false);
			star.SetActive (false);
			playerDescription.enabled = false;
			playerDifficulty.enabled = false;
			ui_triangles.SetActive(false);

			switch(playerIdentifier){

			case 1:
				DataManagerScript.playerOnePlaying = false;
				break;
			case 2:
				DataManagerScript.playerTwoPlaying = false;
				break;
			case 3:
				DataManagerScript.playerThreePlaying = false;
				break;
			case 4:
				DataManagerScript.playerFourPlaying = false;
				break;
			}
		}
		ChoosePlayerScript.Instance.CheckStartable ();
	}


	void Start () {

		// assign xbox keys dynamically
		confirmKey_Xbox = confirmKey + "_Xbox";
		cancelKey_Xbox = cancelKey + "_Xbox";

		normalAxis = new Axis(chooseAxis);
		xboxAxis = new Axis(chooseAxis_Xbox);

		sr = GetComponent<SpriteRenderer> ();
		readyText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		//sr.sprite = squareSprite;

		readyBG.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		sr.enabled = false;
		playerDescription.enabled = false;
		playerDifficulty.enabled = false;

		audio = GetComponent<AudioSource> ();

		square = transform.Find ("Square").gameObject;
		circle = transform.Find ("Circle").gameObject;
		triangle = transform.Find ("Triangle").gameObject;
		trapezoid = transform.Find ("Trapezoid").gameObject;
		rectangle = transform.Find ("Rectangle").gameObject;
		star = transform.Find ("Star").gameObject;

		ui_triangles = transform.Find("Triangles").gameObject;

		square.SetActive (false);
		circle.SetActive (false);
		triangle.SetActive (false);
		trapezoid.SetActive (false);
		rectangle.SetActive (false);
		star.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
		//Debug.Log(Input.GetAxis("Vertical_P1_Xbox")) ;
		// only allow input if game isn't already starting
		//Debug.Log(ChoosePlayerScript.Instance.locked);
	//	Debug.Log(DataManagerScript.xboxMode);
		if (!ChoosePlayerScript.Instance.locked) {

//			if (!DataManagerScript.xboxMode) {
//				CheckAxis (chooseAxis);
//			} else if (DataManagerScript.xboxMode) {
//				CheckAxis (chooseAxis_Xbox);
//			}

			CheckAxis (normalAxis);
			CheckAxis (xboxAxis);

			if (Input.GetButtonDown (confirmKey) || Input.GetButtonDown(confirmKey_Xbox)) {
				activateReadyState ();
			}

			if (Input.GetButtonDown (cancelKey) || Input.GetButtonDown(cancelKey_Xbox)) {
				cancelReadyState ();
			}
		}
	}

	void CheckAxis(Axis whichAxis){
		if (Input.GetAxisRaw (whichAxis.axisName) > 0) {
			if (whichAxis.axisInUse == false) {
				// Call your event function here.
				whichAxis.axisInUse = true;

				if (!readyToPlay && taggedIn) {
					if (playerIdentifier == 1) {
						DataManagerScript.playerOneType += 1; 
						DataManagerScript.playerOneType = DataManagerScript.playerOneType % numberOfPlayerTypes;
						thisType = DataManagerScript.playerOneType;
						audio.PlayOneShot (tickUp);
						UpdatePlayerType (DataManagerScript.playerOneType);
					} else if (playerIdentifier == 2) {
						DataManagerScript.playerTwoType += 1; 
						DataManagerScript.playerTwoType = DataManagerScript.playerTwoType % numberOfPlayerTypes;
						thisType = DataManagerScript.playerTwoType;
						audio.PlayOneShot (tickUp);
						UpdatePlayerType (DataManagerScript.playerTwoType);

					} else if (playerIdentifier == 3) {
						DataManagerScript.playerThreeType += 1; 
						DataManagerScript.playerThreeType = DataManagerScript.playerThreeType % numberOfPlayerTypes;
						thisType = DataManagerScript.playerThreeType;
						audio.PlayOneShot (tickUp);
						UpdatePlayerType (DataManagerScript.playerThreeType);

					} else if (playerIdentifier == 4) {
						DataManagerScript.playerFourType += 1; 
						DataManagerScript.playerFourType = DataManagerScript.playerFourType % numberOfPlayerTypes;
						thisType = DataManagerScript.playerFourType;
						audio.PlayOneShot (tickUp);
						UpdatePlayerType (DataManagerScript.playerFourType);
					}
				}
			}
		}

		if (Input.GetAxisRaw (whichAxis.axisName) < 0) {


			if (whichAxis.axisInUse == false) {
				// Call your event function here.
				whichAxis.axisInUse = true;
				if (!readyToPlay && taggedIn) {
					if (playerIdentifier == 1) {
						DataManagerScript.playerOneType -= 1; 
						if (DataManagerScript.playerOneType < 0) {
							DataManagerScript.playerOneType = numberOfPlayerTypes - 1;
						}
						audio.PlayOneShot (tickDown);
						thisType = DataManagerScript.playerOneType;
						UpdatePlayerType (DataManagerScript.playerOneType);
					} else if (playerIdentifier == 2) {
						DataManagerScript.playerTwoType -= 1; 
						if (DataManagerScript.playerTwoType < 0) {
							DataManagerScript.playerTwoType = numberOfPlayerTypes - 1;
						}
						audio.PlayOneShot (tickDown);
						thisType = DataManagerScript.playerTwoType;
						UpdatePlayerType (DataManagerScript.playerTwoType);

					} else if (playerIdentifier == 3) {
						DataManagerScript.playerThreeType -= 1; 
						if (DataManagerScript.playerThreeType < 0) {
							DataManagerScript.playerThreeType = numberOfPlayerTypes - 1;

						}
						Debug.Log (DataManagerScript.playerThreeType);
						audio.PlayOneShot (tickDown);
						thisType = DataManagerScript.playerThreeType;
						UpdatePlayerType (DataManagerScript.playerThreeType);

					} else if (playerIdentifier == 4) {
						DataManagerScript.playerFourType -= 1; 
						if (DataManagerScript.playerFourType < 0) {
							DataManagerScript.playerFourType = numberOfPlayerTypes - 1;

						}
						Debug.Log (DataManagerScript.playerFourType);
						audio.PlayOneShot (tickDown);
						thisType = DataManagerScript.playerFourType;
						UpdatePlayerType (DataManagerScript.playerFourType);
					}
				}
				//				if (!readyToPlay && taggedIn) {
				//					DataManagerScript.playerOneType -= 1; 
				//					if (DataManagerScript.playerOneType < 0) {
				//						DataManagerScript.playerOneType = numberOfPlayerTypes - 1;
				//					}
				//					DataManagerScript.playerOneType = DataManagerScript.playerOneType % numberOfPlayerTypes;
				//					Debug.Log ("YES!" + DataManagerScript.playerOneType);
				//					UpdatePlayerType (DataManagerScript.playerOneType);
				//				}
			}
		}

		if (Input.GetAxisRaw (whichAxis.axisName) == 0) {
			whichAxis.axisInUse = false;
		}
	}
}
