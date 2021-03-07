using UnityEngine;
using System.Collections;
using PigeonCoopToolkit.Effects.Trails;

public class DemoPlayerController : MonoBehaviour {

	public float speed = 5f;
	public float spinPower = -150f;
	private float startSpeed;
	private float startMass;
	private float startJumpPower;
	public float jumpPower = 750f;
	public bool isJumping = false;
	public string horiz = "Horizontal_P1";
	public string jumpButton = "Jump_P1";
	public string gravButton = "Grav_P1";
	public int team = 1;
	public float startingGrav = 1;
	public int playerID;
	public Mesh meshTypeOne;
	public Mesh meshTypeTwo;
	public int playerType = 0;
	public PolygonCollider2D trianglePC, trapezoidPC;
	private bool canMove;
	private string playerColor;
	private float repeatFreq;
	public GameObject penaltyExplosion;
	public float startDelay = 0.1f;
	private bool moveToLeft;
	private bool moveToRight;
	public bool clampPosition = true;
	public AudioClip jumpSound1;
	public AudioClip jumpSound2;
	public AudioClip landSound;
	public AudioClip changeGravSound1;
	public AudioClip changeGravSound2;
	public AudioClip collideWithBallSound1;
	public AudioClip collideWithBallSound2;
	public AudioClip collideWithBallSoundBig;
	public AudioClip playerExplode;

	public TextMesh pandemoniumCounter;

	public GameObject trail;
	public int DemoType;
	public Sprite squareSprite;
	public Sprite circleSprite;
	public Sprite triangleSprite;
	public Sprite trapezoidSprite;

	public float penaltyTimer;
	private bool penaltyTimerActive = false;

	private float speedPowerupTimer;
	private bool speedPowerupActive = false;

	private float sizePowerupTimer;
	private bool sizePowerupActive = false;

	private float pandemoniumTimer;
	public bool pandemoniumPowerupActive = false;

	// Different base stats for each playerType

	private float squareJumpPower = 2000f;
	private float squareSpeed = 15f;
	private float squareSpinRate = -150f;

	private float circleJumpPower = 2200f;
	private float circleSpeed = 18f;
	private float circleSpinRate = -100f;

	private float triangleJumpPower = 2000f;
	private float triangleSpeed = 18f;
	private float triangleSpinRate = -200f;

	private float trapezoidJumpPower = 1800f;
	private float trapezoidSpeed = 12f;
	private float trapezoidSpinRate = -150f;

    public GameObject innerShape;

	Rigidbody2D rb;
	// Use this for initialization
	void Start () {



		rb = GetComponent<Rigidbody2D>();
		PolygonCollider2D pg = GetComponent<PolygonCollider2D> ();
		rb.gravityScale = startingGrav;
		startMass = rb.mass;
		canMove = true;
		pandemoniumCounter.GetComponent<TextMesh> ().color = new Vector4(0f, 0f, 0f, 0f);

		// assign player color
		switch (playerID) {
		case 1:
			playerColor = "1069A8";
			break;
		case 2:
			playerColor = "7CBEE8";
			break;
		case 3:
			playerColor = "D63236";
			break;
		case 4:
			playerColor = "D97A7B";
			break;
		}

		if (playerType == 0) {
			gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			//gameObject.GetComponent<CircleCollider2D> ().enabled = true;
			//gameObject.GetComponent<MeshFilter> ().mesh = meshTypeOne;
			gameObject.GetComponent<SpriteRenderer> ().sprite = squareSprite;

			jumpPower = squareJumpPower;
			speed = squareSpeed;
			spinPower = squareSpinRate;
		}

		if (playerType == 1) {
			gameObject.GetComponent<CircleCollider2D> ().enabled = true;
		//	gameObject.GetComponent<MeshFilter> ().mesh = meshTypeTwo;
			gameObject.GetComponent<SpriteRenderer> ().sprite = circleSprite;
			jumpPower = circleJumpPower;
			speed = circleSpeed;
			spinPower = circleSpinRate;
		}

		if (playerType == 2) {
			trianglePC.enabled = true;
			//gameObject.GetComponent<MeshFilter> ().mesh = meshTypeTwo;
			gameObject.GetComponent<SpriteRenderer> ().sprite = triangleSprite;
			jumpPower = triangleJumpPower;
			speed = triangleSpeed;
			spinPower = triangleSpinRate;
		}

		if (playerType == 3) {
			trapezoidPC.enabled = true;
			//gameObject.GetComponent<MeshFilter> ().mesh = meshTypeTwo;
			gameObject.GetComponent<SpriteRenderer> ().sprite = trapezoidSprite;
			jumpPower = trapezoidJumpPower;
			speed = trapezoidSpeed;
			spinPower = trapezoidSpinRate;
		}

		startJumpPower = jumpPower;
		startSpeed = speed;
		Invoke ("Begin", startDelay);


        // Set default innershape
        innerShape = transform.Find("InnerShape").gameObject;

        //Special case for Star innershape:
        //0.06 y
        //.6 scale
    

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

	void Begin(){
		switch (DemoType) {
		case 1:

			JumpAndChangeGrav ();
			break;

		case 2:
			repeatFreq = 2f;
			RepeatedlyChangeGrav ();
			break;

		case 3:
			repeatFreq = .3f;
			RepeatedlyChangeGrav ();
			break;

		case 4:
			Invoke ("MoveRightAndJump", 1f);

			break;

		case 5:
			JumpRepeatedly ();
			break;
		case 6:
			float randomStartVal = Random.Range (0f, 2f);
			Invoke ("JumpRepeatedlySlowly", 1f);
			break;
		case 7:

			moveToLeft = true;
			Invoke ("StopMoving", 1.0f);

			break;

		case 8:
			moveToRight = true;
			Invoke ("StopMoving", 1.0f);

			break;

		case 9:
			moveToRight = true;
			Invoke ("ChangeDirection", 0.4f);
			break;
		}
	}

	void ChangeDirection(){

		if (moveToRight) {
			moveToRight = false;
			moveToLeft = true;
		} else {
			moveToRight = true;
			moveToLeft = false;
		}

		Invoke ("ChangeDirection", 0.4f);
	}

	void StopMoving(){
		moveToLeft = false;
		moveToRight = false;
		Invoke ("StopMoving", 0.2f);

		rb.velocity = new Vector2 (0f, 0f);
	}
	IEnumerator Flash() {
		for (float f = .8f; f >= 0; f -= 0.04f) {
			GetComponent<SpriteRenderer>().material.SetFloat ("_FlashAmount", f);
			yield return null;
		}
		yield break; //Is this even needed?
	}

	void MoveRightAndJump(){
		GetComponent<Rigidbody2D>().velocity = new Vector3 (10f, 20f, 0f);
		Invoke ("JumpRepeatedly", 2f);
	}

	void ChangeGrav(){
		rb.gravityScale *= -1f;
        //SoundManagerScript.instance.RandomizeSfx (changeGravSound1, changeGravSound2);
        //StartCoroutine ("Flash");
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
	void JumpRepeatedly(){
		Jump ();
		Invoke ("JumpRepeatedly", 1f);
	}

	void JumpRepeatedlySlowly(){
		Jump ();
		Invoke ("JumpRepeatedlySlowly", 2f);
	}


	void Jump(){
		Vector3 jumpForce = new Vector3(0f,jumpPower * rb.gravityScale,0f);
		rb.AddForce(jumpForce);
	}

	void JumpAndChangeGrav(){

		Jump ();
		Invoke ("ChangeGrav", .25f);
		Invoke ("JumpAndChangeGrav", 2f);
	}

	void RepeatedlyChangeGrav(){
		ChangeGrav ();
		Invoke ("RepeatedlyChangeGrav", repeatFreq);
	}
	void FixedUpdate () {

	//	float moveHorizontal = Input.GetAxis (horiz);
		//Debug.Log (moveHorizontal);
//		float moveVertical = Input.GetAxis ("Vertical"); // these return between 0 and 1
//		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
//		rigidbody.velocity.x = moveHorizontal * speed;
		//GetComponent<Rigidbody2D>().AddTorque(moveHorizontal * -8f);

//		if (isJumping) {
//			GetComponent<Rigidbody2D> ().angularVelocity = (moveHorizontal * spinPower * rb.gravityScale);
//		}
//		Vector3 v3 = GetComponent<Rigidbody2D>().velocity;
//		v3.x = moveHorizontal * speed;
////

		//v3.z = 0.0f;
		//if (canMove) {
	//		GetComponent<Rigidbody2D> ().velocity = v3;
		//}
//		Vector2 v2 = new Vector2(moveHorizontal*speed*100f,0f);
//		GetComponent<Rigidbody2D> ().AddForce (v2);

	//	float f = Mathf.Clamp(GetComponent<Rigidbody2D> ().velocity.x, -speed, speed);
//		Debug.Log (GetComponent<Rigidbody2D> ().velocity.x);
		//		rigidbody.position = new Vector3
//			(
//				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax),
//				0.5f,
//				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
//				);
//		Vector3 v4 = GetComponent<Rigidbody2D>().velocity;
//		v4.x = f;
//
//		GetComponent<Rigidbody2D>().velocity = v4;

	}

	void Update(){
		//Debug.Log (canMove);
//		if (Input.GetButtonDown (jumpButton)) {
//			//Debug.Log ("Jump hit");
//			if (isJumping == false){
//				Vector3 jumpForce = new Vector3(0f,jumpPower * rb.gravityScale,0f);
//				rb.AddForce(jumpForce);
//			//	SoundManagerScript.instance.RandomizeSfx (jumpSound1, jumpSound2);
//				isJumping = true;
//			}
//		}
//
//		if (Input.GetButtonDown (gravButton)) {
//			rb.gravityScale *= -1f;
//			//SoundManagerScript.instance.RandomizeSfx (changeGravSound1, changeGravSound2);
//			StartCoroutine ("Flash");
//
//		}
		if (moveToLeft) {
			rb.velocity = new Vector2 (-15f, 0f);
		}

		if (moveToRight) {
			rb.velocity = new Vector2 (15f, 0f);
		}
		ClampPosition ();


		ManagePowerups ();
	}

	void checkPenalty(){
		switch (team) {
		case 1:
			if (transform.position.x > -1.0f){
				penaltyTimerActive = true;
				penaltyTimer = 10f;
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				trail.GetComponent<Trail>().ClearSystem (true);
				trail.SetActive (false);
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;


			}
			break;

		case 2:
			if (transform.position.x < 1.0f){
				penaltyTimerActive = true;
				penaltyTimer = 10f;
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				trail.GetComponent<Trail>().ClearSystem (true);
				trail.SetActive (false);
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			}

			break;
		}

		// fire 'penalty' explosion
			Vector3 newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
			GameObject pe = (GameObject)Instantiate(penaltyExplosion, newPos, Quaternion.identity);
		//SoundManagerScript.instance.PlaySingle (playerExplode);
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
		if (coll.gameObject.tag == "ScoringBoundary" || coll.gameObject.tag == "Player") {
			//Debug.Log ("a collision!");
			isJumping = false;
		//	Debug.Log (isJumping);
			//SoundManagerScript.instance.PlaySingle (landSound);
		}

		if (coll.gameObject.tag == "Playfield") {
		//	GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			canMove = false;
			//SoundManagerScript.instance.PlaySingle (landSound);
		}

		if (coll.gameObject.tag == "Ball") {
			// update the ball's touch information
			BallScript ball = coll.gameObject.GetComponent<BallScript>();
            if (ball != null) {
                ball.secondToLastTouch = ball.lastTouch;
                ball.lastTouch = playerID;
            }

			// check relative velocity of collision
//			Debug.Log(coll.relativeVelocity.magnitude);
//			if (coll.relativeVelocity.magnitude > 40) {
//				SoundManagerScript.instance.PlaySingle (collideWithBallSoundBig);
//			} else {
//				SoundManagerScript.instance.RandomizeSfx (collideWithBallSound1, collideWithBallSound2);
//			}
		}

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Powerup") {
			//Script other = coll.gameObject.GetComponent<NewPowerUpScript> ();
			int whichPowerup = coll.gameObject.GetComponent<HardcodedPowerUpScript> ().powerupType;
			if (coll.gameObject.GetComponent<HardcodedPowerUpScript> ().isAvailable) {
				coll.gameObject.GetComponent<HardcodedPowerUpScript> ().FadeOut ();
				ApplyPowerup (whichPowerup);
				// fade out all other powerups
				foreach (GameObject otherPowerup in GameObject.FindGameObjectsWithTag("Powerup")) {
					otherPowerup.gameObject.GetComponent<HardcodedPowerUpScript> ().FadeOut (false);
				}
			}
			//Destroy (coll.gameObject);

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
			var mag = coll.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;
			if (mag > 30) {
				//SoundManagerScript.instance.PlaySingle (collideWithBallSoundBig);
			} else {
				//SoundManagerScript.instance.RandomizeSfx (collideWithBallSound1, collideWithBallSound2);
			}
		}
	}

	void ClampPosition(){
		//Debug.Log (pandemoniumPowerupActive);
		// Only clamp position if pandemonium is not active;
		if (clampPosition) {
			if (!pandemoniumPowerupActive) {

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
	}
	void ManagePowerups(){
		if (speedPowerupActive) {
			speedPowerupTimer -= Time.deltaTime;

			if (speedPowerupTimer <= 0){
				speedPowerupActive = false;
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
				Debug.Log ("setting to false");
				// run 'punishment' check if player is offsides.
				checkPenalty();
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

	void ReturnFromPenalty(){
		switch (team) {
		case 1:
			transform.position = new Vector3 (-6f, 0f, -0.5f);


			break;

		case 2:
			transform.position = new Vector3 (6f, 0f, -0.5f);

			break;
		}
			rb.velocity = Vector3.zero;
			rb.gravityScale = startingGrav;
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;

			trail.SetActive (true);
			trail.GetComponent<Trail>().ClearSystem (true);
			gameObject.GetComponent<BoxCollider2D> ().enabled = true;

	}

	void ApplyPowerup(int whichPowerup){
		switch (whichPowerup) {

		case 1:
			speedPowerupActive = true;
			speed = 22f;
			speedPowerupTimer = 20f;

			break;

		case 2:

			sizePowerupActive = true;
			gameObject.transform.localScale = new Vector3 (2f, 2f, 1f);
			rb.mass = startMass * 2f;
			jumpPower = startJumpPower * 1.75f;
			sizePowerupTimer = 20f;

			break;

		case 3:
			pandemoniumPowerupActive = true;
			pandemoniumTimer = 20f;
			break;

		case 4:
			int randomNum = Random.Range (1, 5);
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
			}
			break;

		}

	}
}
