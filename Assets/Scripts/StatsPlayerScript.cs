using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Rewired;

public class StatsPlayerScript : MonoBehaviour {

	public Sprite squareSprite;
	public Sprite circleSprite;
	public Sprite triangleSprite;
	public Sprite trapezoidSprite;

	public string chooseAxis;

	public string confirmKey;
	public string cancelKey;

	private string confirmKey_Xbox;
	private string cancelKey_Xbox;

	public int playerIdentifier;
	public Text readyText;
	public Text toJoinText;
	public Image readyBG;

	public bool readyToPlay;
	public bool taggedIn = false;

	private int whichType;

    // Button names
    //private JoystickButtons buttons;
    private Player player;

	SpriteRenderer sr;

	GameObject square;
	GameObject circle;
	GameObject triangle;
	GameObject trapezoid;
	GameObject rectangle;
	GameObject star;

	void activateReadyState(){
		if (taggedIn) {
			readyToPlay = true;

			readyText.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			readyBG.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);

			StatManagerScript.Instance.increasePlayerReady ();
		}
	}

	void cancelReadyState(){
		if (readyToPlay) {
			readyToPlay = false;
			readyText.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			readyBG.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
			StatManagerScript.Instance.decreasePlayerReady ();
		}

	}


	void Start () {

		square = transform.Find ("Square").gameObject;
		circle = transform.Find ("Circle").gameObject;
		triangle = transform.Find ("Triangle").gameObject;
		trapezoid = transform.Find ("Trapezoid").gameObject;
		rectangle = transform.Find ("Rectangle").gameObject;
		star = transform.Find ("Star").gameObject;

		// Assign keys
		int joystick = -1;
		switch (playerIdentifier) {
		case 1:
			joystick = 1;
			break;
		case 2:
			joystick = 2;
			break;
		case 3:
			joystick = 3;
			break;
		case 4:
			joystick =4;
			break;
		}

        // Get player input names
        //buttons = new JoystickButtons(joystick);
        int gamepadIndex = joystick - 1;
        player = ReInput.players.GetPlayer(gamepadIndex);

		sr = GetComponent<SpriteRenderer> ();
		readyText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		readyBG.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		sr.sprite = squareSprite;

		sr.enabled = false;

		switch(playerIdentifier){

		case 1:
			whichType = DataManagerScript.playerOneType;
						break;
		case 2:
			whichType = DataManagerScript.playerTwoType;
						break;
		case 3:
			whichType = DataManagerScript.playerThreeType;
						break;
		case 4:
			whichType = DataManagerScript.playerFourType;
						break;
					}



		switch (playerIdentifier) {
		case 1:
			if (DataManagerScript.playerOnePlaying) {
			//	sr.enabled = true;
				taggedIn = true;
			}
			break;
		case 2:
			if (DataManagerScript.playerTwoPlaying) {
			//	sr.enabled = true;
				taggedIn = true;
			}
			break;
		case 3:
			if (DataManagerScript.playerThreePlaying) {
			//	sr.enabled = true;
				taggedIn = true;
			}
			break;
		case 4:
			if (DataManagerScript.playerFourPlaying) {
			//	sr.enabled = true;
				taggedIn = true;
			}
			break;

		}

		if (taggedIn) {
			if (whichType == 0) {
				//				fakePlayer1.GetComponent<MeshFilter> ().mesh = meshType1;
				//change sprite here
				//				sr.sprite = squareSprite;
				square.SetActive (true);
			} else if (whichType == 1) {
				circle.SetActive (true);
				//change sprite here
			} else if (whichType == 2) {
				triangle.SetActive (true);
				//change sprite here
			} else if (whichType == 3) {
				trapezoid.SetActive (true);
				//change sprite here
			} else if (whichType == 4) {
				rectangle.SetActive (true);
			} else if (whichType == 5) {
				star.SetActive (true);
			}
		}

	//	Invoke ("BackToTitle", 8f);

	}

	void BackToTitle(){
		SceneManager.LoadSceneAsync("titleScene");
	}
    // Update is called once per frame
    void Update() {
        if (player != null)
        {
            if (player.GetButtonDown("Jump"))
            {
                activateReadyState();
            }


            if (player.GetButtonDown("Grav"))
            {
                cancelReadyState();
            }
        }
	}
}
