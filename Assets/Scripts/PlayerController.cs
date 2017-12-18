using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using PigeonCoopToolkit.Effects.Trails;

public class PlayerController : MonoBehaviour {
	// Switch player behavior based on mode

	public bool isChallengeMode;
    // Core shape stats, public for tesitng
    public float jumpPower;
    public float speed;
	public float spinPower;

    // Save original stats for reversion from powerups
	private float startSpeed;
	private float startMass;
	private float startJumpPower;

    // Button names
    private JoystickButtons buttons;

    // Properties of player by ID
    public int playerID;
	public int playerType = 0;
    private string playerColor;

    // Misc initializations
    public int team = 1;
    public float startingGrav = 1;
    public bool isJumping = false;
    private bool inPenalty = false;
    private bool canMove = true;
	private bool recentlyPaused = false;

    // Particle system
	public ParticleSystem ps;

	// Eventsystem
	public EventSystem es;
    // Rigidbody, mesh, colliders
    Rigidbody2D rb;
    MeshRenderer mr;
    public PolygonCollider2D trianglePC, trapezoidPC, rectanglePC, starPC;
    private Collider2D shapeCollider;

    // Child objects
    public TextMesh pandemoniumCounter;
    public GameObject penaltyExplosion;
    public GameObject trail;

    // Powerup flags and timers
    public float penaltyTimer;
    private bool penaltyTimerActive = false;

    private float speedPowerupTimer;
    private bool speedPowerupActive = false;

    private float sizePowerupTimer;
    private bool sizePowerupActive = false;

    private float pandemoniumTimer;
    private bool pandemoniumPowerupActive = false;

    // TODO Get audio clips from global object?
    public AudioClip jumpSound1;
	public AudioClip jumpSound2;
	public AudioClip landSound;
	public AudioClip changeGravSound1;
	public AudioClip changeGravSound2;
	public AudioClip collideWithBallSound1;
	public AudioClip collideWithBallSound2;
	public AudioClip collideWithBallSoundBig;
	public AudioClip playerExplode;
	public AudioClip powerupSound;
	public AudioClip speedUpSFX1;
	public AudioClip speedUpSFX2;
    public AudioClip speedUpSFX3;
	public AudioClip sizeUpSFX1;
	public AudioClip sizeUpSFX2;
	public AudioClip pandemoniumSFX1;
    public AudioClip pandemoniumSFX2;

    // Map shape numbers to names for use in stat fetching, etc (index == playerType)
    private string[] shapeNames = new string[] {
        "square",
        "circle",
        "triangle",
        "trapezoid",
        "rectangle",
        "star"
    };

    // Use this for initialization
    void Start () {

		//check for challenge mode
		isChallengeMode = DataManagerScript.isChallengeMode;

        // Particle system?
        if ( GetComponent<ParticleSystem>() != null) {
            ps = transform.Find("ssps").GetComponent<ParticleSystem>();
            ps.Stop();
        }

        // Rigidbody, mesh
		rb = GetComponent<Rigidbody2D>();
		mr = GetComponent<MeshRenderer>();

        // Make single reference for appropriate collider
        switch (shapeNames[playerType]) {
            case "square":
                shapeCollider = GetComponent<BoxCollider2D>();
                break;
            case "circle":
                Transform circle = transform.Find("Circle");
                // Special case for circle mesh rendering
                circle.gameObject.SetActive(true);
                shapeCollider = GetComponent<CircleCollider2D>();
                break;
            case "triangle":
                shapeCollider = trianglePC;
                break;
            case "trapezoid":
                shapeCollider = trapezoidPC;
                break;
            case "rectangle":
                shapeCollider = rectanglePC;
                break;
            case "star":
                shapeCollider = starPC;
                break;
        }

        // Starting physics
        if (rb != null) {
            rb.gravityScale = startingGrav;
            startMass = rb.mass;
            pandemoniumCounter.GetComponent<TextMesh>().color = new Vector4(0f, 0f, 0f, 0f);
        }

        int joystick = -1;

		// Assign player color and joystick
		if (isChallengeMode) {

			joystick = DataManagerScript.gamepadControllingMenus;

		} else {
			switch (playerID) {
			case 1:
				playerColor = "1069A8";
				joystick = DataManagerScript.playerOneJoystick;
				break;
			case 2:
				playerColor = "7CBEE8";
				joystick = DataManagerScript.playerTwoJoystick;
				break;
			case 3:
				playerColor = "D63236";
				joystick = DataManagerScript.playerThreeJoystick;
				break;
			case 4:
				playerColor = "D97A7B";
				joystick = DataManagerScript.playerFourJoystick;
				break;
			}
		}

        // Get player input names\
        buttons = new JoystickButtons(joystick);

        // Get stats for chosen shape
        string playerShape = shapeNames[playerType];
        ShapeStats stats = new ShapeStats( playerShape );
        jumpPower = startJumpPower = stats.jump;
        speed = startSpeed = stats.speed;
        spinPower = stats.spin;

        // Get collider for chosen shape
        shapeCollider.enabled = true;
    }

    void IncreasePlayCount(string whichType)
    {
        int tempTotal = PlayerPrefs.GetInt(whichType);
        tempTotal += 1;
        PlayerPrefs.SetInt(whichType, tempTotal);
    }

    void FixedUpdate()
    {
        if (transform.parent.tag != "FakePlayer")
        {
            // Get horizontal input
            if (buttons != null && buttons.horizontal != null)
            {
                float moveHorizontal = Input.GetAxis(buttons.horizontal);

                // Clamp input
                moveHorizontal = Mathf.Clamp(moveHorizontal, -1f, 1f);

                if (isJumping)
                {
                    GetComponent<Rigidbody2D>().angularVelocity = (moveHorizontal * spinPower * rb.gravityScale);
                }

                if (GetComponent<Rigidbody2D>() != null)
                {
                    Vector3 v3 = GetComponent<Rigidbody2D>().velocity;
                    v3.x = moveHorizontal * speed;
                    GetComponent<Rigidbody2D>().velocity = v3;
                }
            }
        }
    }

	void Update() {
        if (transform.parent.tag != "FakePlayer") {
            if (!inPenalty
                && buttons != null
                && buttons.jump != null
                && GameManagerScript.Instance != null
                && !GameManagerScript.Instance.paused
                && !GameManagerScript.Instance.recentlyPaused)
            {
                // Handle jumping
                if (Input.GetButtonDown(buttons.jump))
                {

                    if (isJumping == false && rb != null)
                    {
                        Vector3 jumpForce = new Vector3(0f, jumpPower * rb.gravityScale, 0f);
                        rb.AddForce(jumpForce);
                        SoundManagerScript.instance.RandomizeSfx(jumpSound1, jumpSound2);
                        isJumping = true;
                    }
                }

                // Handle gravity switch
                if (Input.GetButtonDown(buttons.grav) && rb != null && !GameManagerScript.Instance.paused)
                {
                    rb.gravityScale *= -1f;
                    SoundManagerScript.instance.RandomizeSfx(changeGravSound1, changeGravSound2);
                }

            	// Handle start button
            	if (Input.GetButtonDown(buttons.start)
            		&& GameManagerScript.Instance != null
	                && !GameManagerScript.Instance.paused)
	                {
                    GameManagerScript.Instance.Pause(buttons);
                }

	            ClampPosition();
	            ManagePowerups();
	        }
        }
	}

	void checkPenalty(){

		if (team == 1 && transform.position.x > -1.0f // if blue is on the right side
            || team == 2 && transform.position.x < 1.0f ) { // or if red is on the left side

			penaltyTimerActive = true;
			penaltyTimer = 10f;
			DisableShapeAndCollider ();
			FirePenaltyExplosion();
			inPenalty = true;
		}
	}

	void DisableShapeAndCollider(){

        // Disable trail
        trail.GetComponent<Trail>().ClearSystem(true);
        trail.SetActive(false);

        // Disable collider
        shapeCollider.enabled = false;

        if (playerType == 1) {
            // this is a special case for circle mesh rendering
            Transform circle = transform.Find("Circle");
            circle.gameObject.SetActive(false);
        } else {
            mr.enabled = false;
        }
	}

	void EnableShapeAndCollider()  {

        // Enable trail
        trail.SetActive(true);
        trail.GetComponent<Trail>().ClearSystem(true);

        // Enable collider
        shapeCollider.enabled = true;

        if (playerType == 1) {
            // this is a special case for circle mesh rendering
            Transform circle = transform.Find("Circle");
            circle.gameObject.SetActive(true);
        } else {
            mr.enabled = true;
        }
	}


	void FirePenaltyExplosion(){
		Vector3 newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		GameObject pe = (GameObject)Instantiate(penaltyExplosion, newPos, Quaternion.identity);
		SoundManagerScript.instance.PlaySingle (playerExplode);
		pe.SendMessage ("Config", playerColor);
	}

	void OnCollisionStay2D(Collision2D collisionInfo) {

		if (collisionInfo.gameObject.tag == "Playfield") {
		//	Debug.Log ("stay with playfield");
		//	GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			canMove = false;
		}
	}
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "ScoringBoundary" || coll.gameObject.tag == "Player" || coll.gameObject.tag == "Obstacle") {
			isJumping = false;
			SoundManagerScript.instance.PlaySingle (landSound);
		}

		if (coll.gameObject.tag == "Playfield") {
		//	GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			canMove = false;
			//SoundManagerScript.instance.PlaySingle (landSound);
		}

		if (coll.gameObject.tag == "Ball") {
			// update the ball's touch information
			GameManagerScript.Instance.secondToLastTouch = GameManagerScript.Instance.lastTouch;
			GameManagerScript.Instance.lastTouch = playerID;

			// check relative velocity of collision
//			if (coll.relativeVelocity.magnitude > 40) {
//				SoundManagerScript.instance.PlaySingle (collideWithBallSoundBig);
//			} else {
//				SoundManagerScript.instance.RandomizeSfx (collideWithBallSound1, collideWithBallSound2);
//			}
		}

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Powerup") {
			// Debug.Log ("Happening");
			//Script other = coll.gameObject.GetComponent<NewPowerUpScript> ();
			int whichPowerup = coll.gameObject.GetComponent<NewPowerUpScript> ().powerupType;
			if (coll.gameObject.GetComponent<NewPowerUpScript> ().isAvailable) {
				coll.gameObject.GetComponent<NewPowerUpScript> ().FadeOut ();
				ApplyPowerup (whichPowerup);
				SoundManagerScript.instance.PlaySingle (powerupSound);
				// fade out all other powerups
				foreach (GameObject otherPowerup in GameObject.FindGameObjectsWithTag("Powerup")) {
				    otherPowerup.gameObject.GetComponent<NewPowerUpScript> ().FadeOut (false);
					//otherPowerup.SetActive(false);
				}
			}
		}
	}
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "ScoringBoundary" || coll.gameObject.tag == "Player") {
			//Debug.Log ("a collision ended!");
			if (!isJumping) {
					//isJumping = true;
			}
			//Debug.Log (isJumping);
		}

		if (coll.gameObject.tag == "Playfield") {
		//	canMove = true;
		}

		if (coll.gameObject.tag == "Ball") {
		//	Debug.Log (coll.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude);
			var mag = coll.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;
			if (mag > 30) {
				SoundManagerScript.instance.PlaySingle (collideWithBallSoundBig);
			} else {
				SoundManagerScript.instance.RandomizeSfx (collideWithBallSound1, collideWithBallSound2);
			}
		}
	}

	void ClampPosition(){

		// Only clamp position if pandemonium is not active;
		if (!pandemoniumPowerupActive){
			var pos = transform.position;
			if (team == 1) {
				// TODO: Make this dynamic based on raycasting
				pos.x = Mathf.Clamp (transform.position.x, -27.2f, -1.0f);
				transform.position = pos;
			} else if (team == 2) {
				pos.x = Mathf.Clamp (transform.position.x, 1f, 27.2f);
				transform.position = pos;
			}
		}
	}

	void ManagePowerups(){
		if (speedPowerupActive) {
			speedPowerupTimer -= Time.deltaTime;

			if (speedPowerupTimer <= 0){
				speedPowerupActive = false;
				ps.Stop ();
				speed = startSpeed;
			}
		}

		if (sizePowerupActive) {
			sizePowerupTimer -= Time.deltaTime;

			if (sizePowerupTimer <= 0){
				sizePowerupActive = false;
				// Restore scale to starting size
				gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
				rb.mass = startMass;
				jumpPower = startJumpPower;

			}
		}

		if (pandemoniumPowerupActive){
			pandemoniumTimer -= Time.deltaTime;
			pandemoniumCounter.GetComponent<TextMesh> ().color = new Vector4(1f, 1f, 1f, .25f);
			pandemoniumCounter.GetComponent<TextMesh> ().text = Mathf.Floor(pandemoniumTimer).ToString();
			if (pandemoniumTimer <= 0) {
			    pandemoniumCounter.GetComponent<TextMesh> ().color = new Vector4(0f, 0f, 0f, 0f);
				pandemoniumPowerupActive = false;
				// run 'punishment' check if player is offsides.
				checkPenalty();
				// if midpoint marker is faded out, fade it back
				RestoreMidpointMarker();
			}
		}

		if (penaltyTimerActive) {
			penaltyTimer -= Time.deltaTime;

			if (penaltyTimer <= 0) {
				penaltyTimerActive = false;
				ReturnFromPenalty ();
			}
		}
	}

	void ReturnFromPenalty() {

        // Remove flag
        inPenalty = false;

        // Place player in middle of their side, reset velocity and gravity
        float middleX = (team == 1) ? -6f : 6f;
		transform.position = new Vector3 (middleX, 0f, -0.5f);
		rb.velocity = Vector3.zero;
		rb.gravityScale = startingGrav;

        // Turn on collider and shape
        EnableShapeAndCollider();
    }

	void RestoreMidpointMarker(){
		foreach (GameObject mpm in GameObject.FindGameObjectsWithTag("MidpointMarker")) {
			//mpm.SetActive (false);
			iTween.FadeTo (mpm, 1.0f, .5f);
		}
	}

	IEnumerator PlaySFXWithDelay(int whichSFX){
		yield return new WaitForSeconds(.5f);
		switch (whichSFX) {
		case 1:
			SoundManagerScript.instance.RandomizeSfx (speedUpSFX1, speedUpSFX2, speedUpSFX3);
			break;
		case 2:
			SoundManagerScript.instance.RandomizeSfx (sizeUpSFX1, sizeUpSFX2);
			break;
		case 3:
			SoundManagerScript.instance.RandomizeSfx (pandemoniumSFX1, pandemoniumSFX2);
			break;
		case 4:
			break;
		}
	}

	void ApplyPowerup(int whichPowerup, bool playSFX = true){
		MusicManagerScript.Instance.StartFourth ();
		switch (whichPowerup) {

		case 1:
			speedPowerupActive = true;
			ps.Play ();
			speed = 20f; //was 22
			speedPowerupTimer = 20f;
			if (playSFX){
				StartCoroutine ("PlaySFXWithDelay", 1);
			}
			break;

		case 2:

			sizePowerupActive = true;
			gameObject.transform.localScale = new Vector3 (2f, 2f, 1f);
			rb.mass = startMass * 2f;
			jumpPower = startJumpPower * 1.75f;
			sizePowerupTimer = 20f;
			if (playSFX){
				StartCoroutine ("PlaySFXWithDelay", 2);
			}
			break;

		case 3:
			pandemoniumPowerupActive = true;
			pandemoniumTimer = 20f;
			if (playSFX) {
				StartCoroutine ("PlaySFXWithDelay", 3);
			}
			break;

		case 4:
			int randomNum = Random.Range (1, 7);
			//int randomNum = 6;
			switch (randomNum) {
			case 1:
				ApplyPowerup (1);
				break;
			case 2:
				ApplyPowerup (2);
				break;
			case 3:
				ApplyPowerup (3);
				break;
			case 4:
				Camera.main.GetComponent<ManageWiggleScript> ().ActivateWiggle ();
				break;
			case 5:
				// broadcast to all players to activate pandemonium
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					player.GetComponent<PlayerController> ().ApplyPowerup (3, false);
					PlaySFXWithDelay (3);
				}
				foreach (GameObject mpm in GameObject.FindGameObjectsWithTag("MidpointMarker")) {
					//mpm.SetActive (false);
					iTween.FadeTo (mpm, 0f, .5f);
				}
				//Invoke ("RestoreMidpointMarker", 20f);


				//iTween.FadeTo (midpointMarker, 0.8f, .25f);
				break;
			case 6:
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					player.GetComponent<PlayerController> ().ApplyPowerup (2, false);
					PlaySFXWithDelay (2);
				}
				break;
			}
			break;

		}

	}
}
