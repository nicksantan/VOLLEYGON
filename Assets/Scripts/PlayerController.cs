using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using PigeonCoopToolkit.Effects.Trails;
using Rewired;

public class PlayerController : MonoBehaviour {
    // Switch player behavior based on mode
    public bool isAI = false;
    public int AILeft;
    public int AIRight;
    public int AIJump;
    public int AIGrav;

	public bool isChallengeMode;
    private ChallengeManagerScript challengeManager;
    private SoloManagerScript soloManager;

    // Core shape stats, public for tesitng
    public float jumpPower;
    public float speed;
	public float spinPower;

    // Save original stats for reversion from powerups
	private float startSpeed;
	private float startMass;
	private float startJumpPower;
    private Vector3 startingScale = new Vector3(1f,1f,1f);

    // Button names
    public JoystickButtons buttons;
    public VirtualJoystickButtons virtualButtons;
    private Player player;
    private Player pauseButtonPlayer;
    public int joystick = -1;

    // Properties of player by ID
    public int playerID;
	public int playerType = 0;
    private string playerColor;

    // Misc initializations
    public int team = 1;
    public float startingGrav = 1;
    public bool isJumping = false;
    public bool recentlyDidAGravChange = false;
    private bool inPenalty = false;
    private bool canMove = true;

    // Particle system
	public ParticleSystem ps;

	// Eventsystem
	public EventSystem es;
    // Rigidbody, mesh, colliders
    public Rigidbody2D rb;
    MeshRenderer mr;
    public PolygonCollider2D trianglePC, trapezoidPC, rectanglePC, starPC;
    private Collider2D shapeCollider;

    // Child objects
    public TextMesh pandemoniumCounter;
    public GameObject penaltyExplosion;
    public GameObject trail;

    // Powerup flags and timers
    public float penaltyTimer;
    public bool penaltyTimerActive = false;

    private float speedPowerupTimer;
    private bool speedPowerupActive = false;

    private float sizePowerupTimer;
    public bool sizePowerupActive = false;

    private float pandemoniumTimer;
    private bool pandemoniumPowerupActive = false;

    private bool easyMode = false;

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

    public GameObject innerShape;

    public float jumpVel = 21f;
    public bool holdingButtonDown = false;
    // Note: for AI players.
    public bool holdingGravButtonDown = false;
    public int framesSinceLastGravChange = 0;

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

        //check for easy mode
        easyMode = DataManagerScript.easyMode;

        //check for challenge mode
        isChallengeMode = DataManagerScript.isChallengeMode;
        // Is the above still used?

        if (GameObject.FindWithTag("ChallengeManager"))
        {
            challengeManager = GameObject.FindWithTag("ChallengeManager").GetComponent<ChallengeManagerScript>();
        }

        if (GameObject.FindWithTag("SoloManager"))
        {
            soloManager = GameObject.FindWithTag("SoloManager").GetComponent<SoloManagerScript>();
        }

        // Particle system?
        if ( GetComponent<ParticleSystem>() != null) {
            ps = transform.Find("ssps").GetComponent<ParticleSystem>();
            ps.Stop();
        }

        // Rigidbody, mesh
		rb = GetComponent<Rigidbody2D>();
		mr = GetComponent<MeshRenderer>();

        //Special case, rectangle needs its own trail.
        GameObject rect_trail = null;
        if (transform.Find("Trail-Rectangle") != null)
        {
            rect_trail = transform.Find("Trail-Rectangle").gameObject;
        }
      
      
        // Make single reference for appropriate collider and set up pandemonium counter 
        switch (shapeNames[playerType]) {
            case "square":
                shapeCollider = GetComponent<BoxCollider2D>();
                if (rect_trail != null) { rect_trail.SetActive(false); }
                break;
            case "circle":
                Transform circle = transform.Find("Circle");
                // Special case for circle mesh rendering
                circle.gameObject.SetActive(true);
                shapeCollider = GetComponent<CircleCollider2D>();
                if (pandemoniumCounter != null) {
                    pandemoniumCounter.transform.localPosition = new Vector3(0f, 0f, 0f);
                    pandemoniumCounter.GetComponent<TextMesh>().fontSize = 100;
                }
                if (rect_trail != null) { rect_trail.SetActive(false); }
                break;
            case "triangle":
                shapeCollider = trianglePC;
                if (pandemoniumCounter != null) {
                    pandemoniumCounter.transform.localPosition = new Vector3(0f, -0.12f, 0f);
                    pandemoniumCounter.GetComponent<TextMesh>().fontSize = 87;
                }
                if (rect_trail != null) { rect_trail.SetActive(false); }
                break;
            case "trapezoid":
                shapeCollider = trapezoidPC;
                if (rect_trail != null) { rect_trail.SetActive(false); }
                break;
            case "rectangle":
                shapeCollider = rectanglePC;
                if (pandemoniumCounter != null) {
                    pandemoniumCounter.transform.localPosition = new Vector3(0f, 0f, 0f);
                    pandemoniumCounter.GetComponent<TextMesh>().fontSize = 30;
                }
                // Special case, rectangle needs a smaller trail
                trail.SetActive(false);
                trail = rect_trail;
                break;
            case "star":
                shapeCollider = starPC;
                if (pandemoniumCounter != null) {
                    pandemoniumCounter.transform.localPosition = new Vector3(0f, 0.15f, 0f);
                    pandemoniumCounter.GetComponent<TextMesh>().fontSize = 52;
                }
                if (rect_trail != null) { rect_trail.SetActive(false); }
                break;
        }

        // Starting physics
        // Make sure grav is normal in easy mode
        if (easyMode)
        {
            startingGrav = Mathf.Abs(startingGrav);
        }
        if (rb != null) {
            if (!isChallengeMode && !DataManagerScript.isSinglePlayerMode){
            IncreasePlayCount(playerType);
            }
            rb.gravityScale = startingGrav;
            startMass = rb.mass;
            pandemoniumCounter.GetComponent<TextMesh>().color = new Vector4(0f, 0f, 0f, 0f);

            // Set default innershape
            innerShape = transform.Find("InnerShape").gameObject;

            //Special case for Star innershape:
            //0.06 y
            //.6 scale
            if (innerShape)
            {
                if (shapeNames[playerType] == "star")
                {
                    innerShape.transform.localScale = new Vector3(.6f, .6f, .6f);
                    innerShape.transform.localPosition = new Vector3(0f, 0.06f, -1f);
                }

                //Special case for flange innershape:
                if (shapeNames[playerType] == "rectangle")
                {
                    innerShape.transform.localScale = new Vector3(.85f, .5f, .8f);
                    //scale 0.85 .5 .8
                }
            }

            if (innerShape)
            {
                if (rb.gravityScale < 0)
                {
                    innerShape.SetActive(true);
                }
                else
                {
                    innerShape.SetActive(false);
                }
            }
        }

        joystick = -1;

		// Assign player color and joystick
		if (isChallengeMode || DataManagerScript.isSinglePlayerMode) {

         
            joystick = DataManagerScript.gamepadControllingMenus + 1;
            playerColor = "1069A8";

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

                case 5:
                    playerColor = "D63236";
                    break;

                case 6:
                    playerColor = "D97A7B";
                    break;

			}
		}

        // Get player input names
        if (!isAI)
        {
            //buttons = new JoystickButtons(joystick);

            // Note, convert joystick number to player index requires -1.
          
            player = ReInput.players.GetPlayer(joystick - 1);
            pauseButtonPlayer = ReInput.players.GetPlayer(2);
        } else
        {
            virtualButtons = new VirtualJoystickButtons();
        }

        // Get stats for chosen shape
        string playerShape = shapeNames[playerType];
        ShapeStats stats = new ShapeStats( playerShape );
        jumpPower = startJumpPower = stats.jump;
        speed = startSpeed = stats.speed;
        spinPower = stats.spin;

        // Get collider for chosen shape
        shapeCollider.enabled = true;

        // adjust scale based on Easy Mode flag if we are NOT in the character selection screen
        if (easyMode && gameObject.tag != "FakePlayer")
        {
            startingScale = new Vector3(1.5f, 1.5f, 1f);
        }
        else
        {
            startingScale = new Vector3(1f, 1f, 1f);
        }

        transform.localScale = startingScale;

    }

    void IncreasePlayCount(int whichType)
    {   
        if (!isAI){
        int tempTotal = FileSystemLayer.Instance.playerPlays[whichType];
        tempTotal += 1;
        FileSystemLayer.Instance.SavePlayerPlay(whichType, tempTotal);
        }
    }

    void FixedUpdate()
    {
        if (transform.parent && transform.parent.tag != "FakePlayer")
        {
            if ((!challengeManager || challengeManager.challengeRunning) && (!soloManager || soloManager.gameRunning) && GameManagerScript.Instance.GetComponent<PauseManagerScript>().isInputLocked == false)
            {
                // Get horizontal input
                if (player != null)
                {
                    float moveHorizontal = player.GetAxis("MoveX");

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
                       // GetComponent<Rigidbody2D>().AddForce(new Vector2(moveHorizontal * speed * 10f, 0f));
                    }
                }
            }
        }

        if (isAI)
        {
            // Get horizontal input

            float moveHorizontal = virtualButtons.horizontal;
//            Debug.Log(moveHorizontal);
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

	void Update() {
        //TODO: Oof, can this be changed?
        
        if (transform.parent && transform.parent.tag != "FakePlayer")
        {
            if (inPenalty && GameManagerScript.Instance != null
                && !GameManagerScript.Instance.GetComponent<PauseManagerScript>().paused
                && !GameManagerScript.Instance.GetComponent<PauseManagerScript>().recentlyPaused)
            {
                ManagePenalty();
                }
//            Debug.Log(isAI);
            if (isAI)
            {
                framesSinceLastGravChange += 1; // TODO: Move to fixed update 
                // Handle jumping
//                Debug.Log("is ai");
                if (virtualButtons.jump)
                {
//                    Debug.Log("VIRTUAL JUMP!");
                    if (isJumping == false && rb != null && !holdingButtonDown)
                    {
                        Vector3 jumpForce = new Vector3(0f, jumpPower * rb.gravityScale, 0f);
                        // rb.AddForce(jumpForce);
                        holdingButtonDown = true;
                        Vector3 v3 = GetComponent<Rigidbody2D>().velocity;
                        v3.y = jumpVel * rb.gravityScale; //TODO: Replace with shape-specific var
                        GetComponent<Rigidbody2D>().velocity = v3;
                        SoundManagerScript.instance.RandomizeSfx(jumpSound1, jumpSound2);
                        isJumping = true;
                    }
                }
                else
                {
                    // Fast fall!
                    holdingButtonDown = false;
                    if (isJumping && rb != null)
                    {
                        //  Debug.Log("fast fall!");

                        Vector3 fastFallForce = new Vector3(0f, rb.gravityScale * -4900f * Time.deltaTime, 0f);
                        rb.AddForce(fastFallForce);
                    }

                }

                // Handle gravity switch
                if (virtualButtons.grav && rb != null && !easyMode && framesSinceLastGravChange > 100 ) //TODO: will need a puse switch //!GameManagerScript.Instance.GetComponent<PauseManagerScript>().paused
                {
                    rb.gravityScale *= -1f;
                    framesSinceLastGravChange = 0;
                    isJumping = true;
                
                    holdingGravButtonDown = true;
                    SoundManagerScript.instance.RandomizeSfx(changeGravSound1, changeGravSound2);
                    if (rb.gravityScale < 0)
                    {
                        innerShape.SetActive(true);
                    }
                    else
                    {
                        innerShape.SetActive(false);
                    }
                } else if (!virtualButtons.grav)
                {
                    holdingGravButtonDown = false;
                }
                ClampPosition();
                ManagePowerups();
            }

            if (!inPenalty
                && GameManagerScript.Instance != null
                && !GameManagerScript.Instance.GetComponent<PauseManagerScript>().paused
                && !GameManagerScript.Instance.GetComponent<PauseManagerScript>().recentlyPaused
                && (!challengeManager || challengeManager.challengeRunning) && (!soloManager || soloManager.gameRunning) && !isAI
                && GameManagerScript.Instance.GetComponent<PauseManagerScript>().isInputLocked == false
                )
            {

                {

                    // Handle jumping
                //    Debug.Log(Input.GetButton(buttons.jump));
                    if (player.GetButton("Jump"))
                    {

                        if (isJumping == false && rb != null && !holdingButtonDown)
                        {
                            Vector3 jumpForce = new Vector3(0f, jumpPower * rb.gravityScale, 0f);
                            // rb.AddForce(jumpForce);
                            holdingButtonDown = true;
                            Vector3 v3 = GetComponent<Rigidbody2D>().velocity;
                            v3.y = jumpVel* rb.gravityScale; //TODO: Replace with shape-specific var
                            GetComponent<Rigidbody2D>().velocity = v3;
                            SoundManagerScript.instance.RandomizeSfx(jumpSound1, jumpSound2);
                            isJumping = true;
                        }
                    }
                    else
                    {
                        // Fast fall!
                        holdingButtonDown = false;
                        if (isJumping && rb != null)
                        {
                            //  Debug.Log("fast fall!");

                            Vector3 fastFallForce = new Vector3(0f, rb.gravityScale * -4900f *Time.deltaTime, 0f); //was 1900
                            rb.AddForce(fastFallForce);
                        }

                    }

                    // Handle gravity switch
                    if (player.GetButtonDown("Grav") && rb != null && !easyMode && !GameManagerScript.Instance.GetComponent<PauseManagerScript>().paused)
                    {
                        rb.gravityScale *= -1f;
                        isJumping = true;
                        SoundManagerScript.instance.RandomizeSfx(changeGravSound1, changeGravSound2);
                        if (rb.gravityScale < 0)
                        {
                            innerShape.SetActive(true);
                        }
                        else
                        {
                            innerShape.SetActive(false);
                        }
                    }

                    // Handle start button
                    if (pauseButtonPlayer.GetButtonDown("Start")
                        && GameManagerScript.Instance != null
                        && !GameManagerScript.Instance.GetComponent<PauseManagerScript>().paused)
                    {

                        bool allowedToPause = true;
                        // make sure user is allowed to pause right now in pause mode
                        Debug.Log("challenge manager exist?");
                        Debug.Log(GameObject.FindWithTag("ChallengeManager"));
                        if (GameObject.FindWithTag("ChallengeManager"))
                        {
                            GameObject cm = GameObject.FindWithTag("ChallengeManager");
                            Debug.Log(cm.GetComponent<ChallengeManagerScript>().challengeRunning);
                            if (!cm.GetComponent<ChallengeManagerScript>().challengeRunning)
                            {
                                allowedToPause = false;
                                Debug.Log("setting allowed to pause to false");
                            }
                        }
                        Debug.Log("allowed to pause?");
                        Debug.Log(allowedToPause);
                        if (allowedToPause)
                        { 
                            
                            GameManagerScript.Instance.GetComponent<PauseManagerScript>().Pause(joystick);
                        }
                    }


                }
                if (Mathf.Abs(transform.position.y) < 4f)
                {
                    isJumping = true;
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

    public void WinRumble()
    {
        // This joystick - 1 shit has got to go.
        if (JoystickLayerManager.Instance != null && !isAI)
        {
            JoystickLayerManager.Instance.ActivateLargeRumble(joystick - 1, 2f);
        }
    }

    public void SmallRumblePulse()
    {
        // This joystick - 1 shit has got to go.
        if (JoystickLayerManager.Instance != null && !isAI)
        {
            JoystickLayerManager.Instance.SmallRumblePulse(joystick - 1, .25f);
        }
    }

    public void TinyRumblePulse(){
         // This joystick - 1 shit has got to go.
        if (JoystickLayerManager.Instance != null && !isAI)
        {
            JoystickLayerManager.Instance.SmallRumblePulse(joystick - 1, .15f);
        }
    }

    public void StartTinyRumble()
    {
        Debug.Log("Starting tiny rumble");
        // This joystick - 1 shit has got to go.
        if (JoystickLayerManager.Instance != null && !isAI)
        {
            JoystickLayerManager.Instance.BeginSmallRumble(joystick - 1);
        }
    }

    public void StopAllRumble()
    {
        // This joystick - 1 shit has got to go.
        if (JoystickLayerManager.Instance != null && !isAI)
        {
            JoystickLayerManager.Instance.StopRumble(joystick - 1);
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

        // if it's on, turn off innershape as well
        innerShape.SetActive(false);
	}

	void EnableShapeAndCollider()  {
        Debug.Log("Enabling shape");
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
            coll.gameObject.GetComponent<BallScript>().secondToLastTouch = coll.gameObject.GetComponent<BallScript>().lastTouch;
            coll.gameObject.GetComponent<BallScript>().lastTouch = playerID;
            
            // Send a tiny rumble
            TinyRumblePulse();
            
            if (GameObject.FindWithTag("StatsModule"))
            {
                GameObject.FindWithTag("StatsModule").GetComponent<StatsModuleScript>().secondToLastTouch = coll.gameObject.GetComponent<BallScript>().lastTouch;
                GameObject.FindWithTag("StatsModule").GetComponent<StatsModuleScript>().lastTouch = playerID;

            }
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
				pos.x = Mathf.Clamp (transform.position.x, -17f, -1.0f);
				transform.position = pos;
			} else if (team == 2) {
				pos.x = Mathf.Clamp (transform.position.x, 1f, 17f);
				transform.position = pos;
			}
		}

        if (rb.angularVelocity < -2f) { rb.angularVelocity = -2f; }
        if (rb.angularVelocity > 2f) { rb.angularVelocity = 2f; } // was .2
    }

    void ManagePenalty()
    {
        Debug.Log(penaltyTimerActive);
        if (penaltyTimerActive)
        {
            Debug.Log("Penalty timer going down");
            Debug.Log(penaltyTimer);
            penaltyTimer -= Time.deltaTime;

            if (penaltyTimer <= 0f)
            {
                penaltyTimerActive = false;
                ReturnFromPenalty();
            }
        }
    }

    void ManagePowerups(){
       //Debug.Log("Managing powerups");
		if (speedPowerupActive) {
			speedPowerupTimer -= Time.deltaTime;

			if (speedPowerupTimer <= 0){
				speedPowerupActive = false;
				speed = startSpeed;

				if (ps != null) {
					ps.Stop();
				}
			}
		}

		if (sizePowerupActive) {
			sizePowerupTimer -= Time.deltaTime;

			if (sizePowerupTimer <= 0){
				sizePowerupActive = false;
                // Restore scale to starting size
                gameObject.transform.localScale = startingScale;
				rb.mass = startMass;
				jumpPower = startJumpPower;

			}
		}

		if (pandemoniumPowerupActive){
			pandemoniumTimer -= Time.deltaTime;
            if (Mathf.Sign(rb.gravityScale) > 0)
            {
                pandemoniumCounter.GetComponent<TextMesh>().color = new Vector4(1f, 1f, 1f, .85f);
            } else
            {
                pandemoniumCounter.GetComponent<TextMesh>().color = new Vector4(1f, 1f, 1f, .85f);
            }
			
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
	}

	void ReturnFromPenalty() {
        Debug.Log("Returning from penalty");
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
			if (ps != null) {
				ps.Play();
			}

            speedPowerupActive = true;
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
