using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FakePlayerScript : MonoBehaviour {

    public Sprite squareSprite;
	public Sprite circleSprite;
	public Sprite triangleSprite;
	public Sprite trapezoidSprite;

	public Image readyBG;

    private JoystickButtons buttons;
    private int joystickIdentifier = -1;

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
    
	Axis verticalAxis;

    SpriteRenderer sr;

	void UpdatePlayerType(int whichType){
		if (!readyToPlay) {

			square.SetActive (false);
			circle.SetActive (false);
			triangle.SetActive (false);
			trapezoid.SetActive (false);
			rectangle.SetActive (false);
			star.SetActive (false);

			if (whichType == 0) {
				square.SetActive (true);
				playerDescription.text = "CLASSIC\nDEFENSIVE";
				playerDifficulty.text = "EASY";
			} else if (whichType == 1) {
				circle.SetActive (true);
				playerDescription.text = "ALL-AROUND\nVERSATILE";
				playerDifficulty.text = "MEDIUM";
			} else if (whichType == 2){
				triangle.SetActive (true);
				playerDescription.text = "AIRBORNE\nAGGRESSIVE";
				playerDifficulty.text = "HARD";
			} else if (whichType == 3){
				trapezoid.SetActive (true);
				playerDescription.text = "CRAZY!\nWEIRD!";
				playerDifficulty.text = "EXPERTS ONLY";
			} else if (whichType == 4){
				rectangle.SetActive (true);
				playerDescription.text = "DEFENSIVE\nSLOW";
				playerDifficulty.text = "EASY";
			} else if (whichType == 5){
				star.SetActive (true);
				playerDescription.text = "STRONG\nUNPREDICTABLE";
				playerDifficulty.text = "HARD";
			}

		}
	}

	void activateReadyState(){

		if (taggedIn) {

            // Ready up

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

            // Tag in
			taggedIn = true;
			audio.PlayOneShot (tagInSound);
			toJoinText.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			playerDescription.enabled = true;
			playerDifficulty.enabled = true;
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

            joystickIdentifier = -1;
            buttons = null;
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

	void Start () {

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

        // Be on the lookout for any button presses and see if joystick was assigned
        if (!taggedIn && Input.anyKeyDown)
        {
            checkForJoystick();
        }

        if (!ChoosePlayerScript.Instance.locked && buttons != null) {

            // Joystick movements
            checkVerticalAxis(verticalAxis);

            if (Input.GetButtonDown(buttons.jump))
            {
                activateReadyState();
            }

            if ( Input.GetButtonDown (buttons.grav ) ) {
				cancelReadyState ();
			}
		}
	}

    void checkForJoystick()
    {
        // Get joystick for player slot
        switch (playerIdentifier)
        {

            case 1:
                joystickIdentifier = DataManagerScript.playerOneJoystick;
                break;
            case 2:
                joystickIdentifier = DataManagerScript.playerTwoJoystick;
                break;
            case 3:
                joystickIdentifier = DataManagerScript.playerThreeJoystick;
                break;
            case 4:
                joystickIdentifier = DataManagerScript.playerFourJoystick;
                break;

        }

        // Activate slot if a joystick was selected
        if (joystickIdentifier != -1)
        {

            Debug.Log("Player " + playerIdentifier + " Joystick " + joystickIdentifier);

            // Assign joystick to player
            buttons = new JoystickButtons(joystickIdentifier);

            // Get axis string from joystick class
            verticalAxis = new Axis(buttons.vertical);
        }
    }

    void checkVerticalAxis(Axis whichAxis){

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
                        thisType = DataManagerScript.playerOneType = ( numberOfPlayerTypes + DataManagerScript.playerOneType + difference ) % numberOfPlayerTypes;
                        break;
                    case 2:
                        thisType = DataManagerScript.playerTwoType = ( numberOfPlayerTypes + DataManagerScript.playerTwoType + difference ) % numberOfPlayerTypes;
                        break;
                    case 3: 
                        thisType = DataManagerScript.playerThreeType = ( numberOfPlayerTypes + DataManagerScript.playerThreeType + difference ) % numberOfPlayerTypes;
                        break;
                    case 4: 
                        thisType = DataManagerScript.playerFourType = ( numberOfPlayerTypes + DataManagerScript.playerFourType + difference ) % numberOfPlayerTypes;
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
