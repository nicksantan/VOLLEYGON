using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FakePlayerScript : MonoBehaviour {

	public Sprite squareSprite;
	public Sprite circleSprite;
	public Sprite triangleSprite;
	public Sprite trapezoidSprite;

	public Image readyBG;

    private JoystickButtons joystickButtons;

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

	private int numberOfPlayerTypes = 6;
    
	Axis axis;

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

        // Revert from "ready to play" state to tagged in
		if (readyToPlay) {

			readyToPlay = false;
			readyText.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			readyBG.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			playerDescription.enabled = true;
			playerDifficulty.enabled = true;

        // Revert from tagged in to nothingness
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

    // Function to tie joystick to player 
    void assignJoystickToPlayer (int joystick) {

        // Save player int
        playerIdentifier = joystick;

        // Assign button names from player int
        joystickButtons = new JoystickButtons(playerIdentifier);

    }

	void Start () {

        // TEMP: assign joystick to player manually
        joystickButtons = new JoystickButtons(playerIdentifier);

        // Get axis string from joystick class
        axis = new Axis( joystickButtons.vertical );

		sr = GetComponent<SpriteRenderer> ();
		readyText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		readyBG.GetComponent<CanvasRenderer> ().SetAlpha(0.0f);

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

		square.SetActive (false);
		circle.SetActive (false);
		triangle.SetActive (false);
		trapezoid.SetActive (false);
		rectangle.SetActive (false);
		star.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
		if (!ChoosePlayerScript.Instance.locked && joystickButtons != null ) {

            // Joystick movements
			CheckAxis (axis);

            // Button presses
			if ( Input.GetButtonDown ( joystickButtons.jump ) ) {
				activateReadyState ();
			}

			if ( Input.GetButtonDown (joystickButtons.grav ) ) {
				cancelReadyState ();
			}
		}
	}

	void CheckAxis(Axis whichAxis){

        // Up or down pressed
		if (Input.GetAxisRaw (whichAxis.axisName) > 0 || Input.GetAxisRaw(whichAxis.axisName) < 0) {

            // Only proceed if player is tagged in but not ready, and joystick not already pressed up/down
            if (whichAxis.axisInUse == false && !readyToPlay && taggedIn ) {

                // Boolean to prevent scrolling more than one tick per press
                whichAxis.axisInUse = true;

                // See if going up or down
                bool goingUp = Input.GetAxisRaw(whichAxis.axisName) > 0;

                // Move up or down through shape ints
                int difference = (goingUp) ? 1 : -1;

                // Set type for player 
				switch (playerIdentifier) {

                    case 1:
                        thisType = DataManagerScript.playerOneType = (numberOfPlayerTypes + DataManagerScript.playerOneType + difference ) % numberOfPlayerTypes;
                        break;
                    case 2:
                        thisType = DataManagerScript.playerTwoType = (numberOfPlayerTypes + DataManagerScript.playerTwoType + difference ) % numberOfPlayerTypes;
                        break;
                    case 3: 
                        thisType = DataManagerScript.playerThreeType = (numberOfPlayerTypes + DataManagerScript.playerThreeType + difference ) % numberOfPlayerTypes;
                        break;
                    case 4: 
                        thisType = DataManagerScript.playerFourType = (numberOfPlayerTypes + DataManagerScript.playerFourType + difference ) % numberOfPlayerTypes;
                        break;

				}

                // Play sound effect
                AudioClip tick = ( goingUp ) ? tickUp : tickDown;
                audio.PlayOneShot(tick);

                // Save type
                UpdatePlayerType(thisType);
            }

        }
        else if (Input.GetAxisRaw(whichAxis.axisName) == 0)
        {
            // Reset boolean to prevent scrolling more than one tick per press when joystick returns to 0
            whichAxis.axisInUse = false;
        }
        
	}
}
