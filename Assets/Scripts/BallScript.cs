using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PigeonCoopToolkit.Effects.Trails;

public class BallScript : MonoBehaviour {

	public Text scoreText;
	public Text winByTwoText;
	public GameObject scoreboard;
	public GameObject background;
	public bool isOnePlayer;
	Rigidbody2D rb;
	float timer;
	float timeSinceLastFlash;
	float flashTime;
	private bool isTimerRunning;
	public float gravScale = 0.8f;
	private float originalGrav;
	public bool singleMode;
	private int bounces = 0;
	public int bouncesOnTop;
	public float baseTimeBetweenGravChanges = 10f;
	public float lastXPos;
	public Sprite originalSprite;
	public Sprite reverseGravSprite;
	public Sprite changingSprite;
	private Sprite theSprite;
	public GameObject prefab;
	public GameObject trail;
	public GameObject bounceImpact;
	public GameObject explosionPrefab;
	public int lastTouch;
	public int secondToLastTouch;
	public GameObject gravityIndicator;
	public GameObject redball;
	public GameObject blueball;
	public GameObject circleTrail;
	int lastScore;

	// Ball options
	public bool gravChangeMode = true;
	public bool scoringMode = true;

	GameObject CurrentArena;

	public AudioSource sfxSource;

	public AudioClip ballServedSound;
	public AudioClip pointScoredSound1;
	public AudioClip pointScoredSound2;
	public AudioClip gravityChangeSound;
	public AudioClip gravityIsAboutToChangeSound;
	public AudioClip bounceOffPlayerSound2;
	public AudioClip bounceOffWallSound;
	public AudioClip bounceOffScoringBoundarySound;
	public AudioClip largeHitSound;
	private float maxSpeed = 68f;
	private new AudioSource audio;
	public bool didSirenPlayAlready;
	// Use this for initialization

	void Awake(){
		// Debug.Log (GameManagerScript.Instance.CurrentArena);
		// CurrentArena = GameManagerScript.Instance.CurrentArena;
		// Debug.Log ("assign arena");
	}

	void Start () {
		audio = GetComponent<AudioSource> ();

		didSirenPlayAlready = false;
		flashTime = 0f;
		timeSinceLastFlash = 0f;
		rb = GetComponent<Rigidbody2D>();
		CurrentArena = GameObject.FindWithTag("Arena");
		theSprite = GetComponent<SpriteRenderer>().sprite;
		rb.isKinematic = true;
		//TODO: Replace invoke with a coroutine and add parameters
		Invoke("LaunchBall", 3f);
		timer = baseTimeBetweenGravChanges + Random.value * 10 ;
		rb.gravityScale = gravScale;
		originalGrav = gravScale;

		//	scoreText.text = GameManagerScript.Instance.teamOneScore.ToString () + " - " + GameManagerScript.Instance.teamTwoScore.ToString ();
	}

	void FixedUpdate(){

	}
	// Update is called once per frame
	void Update () {

		if(rb.velocity.magnitude > maxSpeed)
		{
			rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
		}

		// Only change gravity if gravchange mode is on, which it is by default. (Will be turned off for certain challenges). This should be in game manager.
		if (gravChangeMode) {
			if (isTimerRunning) {
				timer -= Time.deltaTime;
				//Debug.Log (timer);
			}

			timeSinceLastFlash = Time.time - flashTime;

			if (timer <= 3 && timeSinceLastFlash >= .25f) {

				if (!didSirenPlayAlready) {
					//SoundManagerScript.Instance.PlaySingle (gravityIsAboutToChangeSound);
					audio.Play ();
					didSirenPlayAlready = true;
				}
				if (redball.activeSelf) {
					redball.SetActive (false);
					blueball.SetActive (true);
				} else {
					redball.SetActive (true);
					//blueball.SetActive (false);
				}
				flashTime = Time.time;
				GameObject cTrail = Instantiate (circleTrail) as GameObject;
				cTrail.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 1f);
				cTrail.transform.parent = transform.Find ("CircleTrails");
				cTrail.GetComponent<Renderer> ().material.SetColor ("_Color", new Color32 (244, 244, 244, 100));
				cTrail.SendMessage ("Config", 2);
			}

			if (timer <= 0) {
				GravChange ();
				ResetTimer ();
			}
		}

		CheckForSideChange ();

	}

	void ResetTimer(){
		timer = baseTimeBetweenGravChanges + Random.value * 10;
		isTimerRunning = true;
		didSirenPlayAlready = false;
	}
	void LaunchBall(){
		rb.isKinematic = false;
		trail.SetActive (true);
		//Send the ball in a random direction
		Transform child = gameObject.transform.Find("CircleTrails");
		child.gameObject.SetActive (true);
		ResetTimer();
		//In the future, factor in the gravity factor;
		rb.velocity = new Vector2 (Random.Range(-10.0F, 10.0F), Random.Range(10f*rb.gravityScale, 20F*rb.gravityScale));
		//rb.velocity = new Vector2 (80f,0f);

		SoundManagerScript.instance.PlaySingle (ballServedSound);

	}

	void CheckForSideChange(){
		if (Mathf.Sign (transform.position.x) != Mathf.Sign (lastXPos) && lastXPos != 0 && transform.position.x !=0) {
			GameManagerScript.Instance.SideChange ();

			// Is this doing anything?
			GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
		}

		lastXPos = transform.position.x;

	}

	void FadeOutScore(){
		scoreText.CrossFadeAlpha(0f,.25f,false);
		winByTwoText.CrossFadeAlpha (0f, .25f, false);
	}

	public void ResetBall(){
		trail.GetComponent<Trail>().ClearSystem (true);
		trail.SetActive (false);
		rb.isKinematic = true;
		gameObject.transform.position = new Vector3 (0, 0, 0);
		rb.velocity = new Vector2 (0, 0);
		GameManagerScript.Instance.bounces = 0;
		GameManagerScript.Instance.bouncesOnBottom = 0;
		GameManagerScript.Instance.bouncesOnBottomLeft = 0;
		GameManagerScript.Instance.bouncesOnBottomRight = 0;
		GameManagerScript.Instance.bouncesOnTopLeft = 0;
		GameManagerScript.Instance.bouncesOnTopRight = 0;
		timer = 10; // arbitrary high number
		Transform child = gameObject.transform.Find("CircleTrails");
		child.gameObject.SetActive (false);
		// Reset last touch information
		lastTouch = 0;
		secondToLastTouch = 0;

		GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f) ;
		Invoke ("LaunchBall", 3f);
		//Invoke ("FadeOutScore", 2f);
		isTimerRunning = false;
		int signValue = Random.Range (0, 2) * 2 - 1;
		rb.gravityScale = originalGrav * signValue;

		if (Mathf.Sign (rb.gravityScale) < 0) {
			GetComponent<SpriteRenderer>().sprite = reverseGravSprite;
			redball.SetActive (true);
			//blueball.SetActive (false);
		} else {
			GetComponent<SpriteRenderer>().sprite = originalSprite;
			blueball.SetActive (true);
			redball.SetActive (false);
		}
	}

	void GravChange(){
		rb.gravityScale *= -1;
		SoundManagerScript.instance.PlaySingle (gravityChangeSound);
		// Debug.Log ("sign of gravity scale is " + Mathf.Sign (rb.gravityScale));
		if (Mathf.Sign (rb.gravityScale) < 0) {
			//Debug.Log ("changing sprite?");
			//GetComponent<SpriteRenderer>().sprite = reverseGravSprite;
			redball.SetActive(true);
			//blueball.SetActive(false);
			gravityIndicator.transform.localScale = new Vector3 (1f, -1f, 1f);
		} else {
			GetComponent<SpriteRenderer>().sprite = originalSprite;
			redball.SetActive(false);
			blueball.SetActive (true);
			gravityIndicator.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
		gravityIndicator.GetComponent<PlayAnimationScript> ().PlayAnimation ();
	}



	void CreateBounceImpact (Collision2D coll, int whichType, int whichNum){
		//Instantiate(bounceImpact, new Vector3(0f, 0, 0), Quaternion.identity);
		GameObject childObject = Instantiate(bounceImpact) as GameObject;
		Transform mask = coll.gameObject.transform.Find("Mask").transform;
		childObject.transform.parent = mask;
		// create a new position based on the y and x of the collision
		Vector3 impactPos = gameObject.transform.position;
		impactPos.z = -1;
		//TODO: this has to be a switch
		//impactPos.y = 0.5f;
		//Vector3 newPos = new Vector3(0f, 0.5f, -1f);
		//childObject.transform.TransformPoint(newPos);
		Vector3 newPos = mask.InverseTransformPoint(impactPos);
		newPos.z = -1f;
		var size = coll.gameObject.GetComponent<BoxCollider2D>().size.y;
		if (gameObject.transform.position.y < 0) {

			newPos.y = size/2;
		} else {
			newPos.y = -1f * size/2;
		}
		childObject.transform.localPosition = newPos;
		coll.gameObject.transform.Find("Mask").GetComponent <SpriteMask> ().updateSprites ();
		// control the color programatically
		switch (whichNum) {
		case 1:
			childObject.GetComponent<Renderer>().material.SetColor ("_Color", new Color32(160, 160, 160, 100));
			break;

		case 2:
			childObject.GetComponent<Renderer>().material.SetColor ("_Color", new Color32(183, 183, 183, 100));
			break;

		case 3:
			childObject.GetComponent<Renderer>().material.SetColor ("_Color", new Color32(194, 194, 194, 100));
			break;
		}
		//childObject.GetComponent<Renderer>().material.SetColor ("_Color", new Color32(214, 214, 214, 100));
		childObject.SendMessage("Config", whichType);
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	//TODO: Other elements play SFX through the sound manager. It's bad that the ball is different.
	public void Pause(){
		//audio.volume = 0;
		audio.Pause();
	}

	public void UnPause(){
		//		audio.volume = 1f;
		audio.UnPause ();
	}

	public void FireExplosion(){
		audio.Stop ();
		Vector3 newPos = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		Instantiate (explosionPrefab, newPos, Quaternion.identity);
		if (newPos.y > 0) {
			Instantiate (explosionPrefab, newPos, Quaternion.Euler (0, 0, -180));
		} else {
			Instantiate (explosionPrefab, newPos, Quaternion.Euler (0, 0, 0));
		}
		SoundManagerScript.instance.RandomizeSfx (pointScoredSound1, pointScoredSound2);
	}

	void OnCollisionEnter2D(Collision2D coll){

		if (coll.gameObject.tag == "Wall") {
			SoundManagerScript.instance.PlaySingle (bounceOffWallSound);
		}

		// If ball has hit a scoring boundary
		if (coll.gameObject.tag == "ScoringBoundary") {
			//Debug.Log ("a collision!");
			SoundManagerScript.instance.PlaySingle (bounceOffScoringBoundarySound);
			GameManagerScript.Instance.bounces += 1;
			if (coll.gameObject.transform.position.y > 0) {
				if (coll.gameObject.transform.position.x < 0) {
					//GameManagerScript.Instance.bouncesOnTop += 1;
					GameManagerScript.Instance.bouncesOnTopLeft += 1;
				}
				if (coll.gameObject.transform.position.x > 0) {
					GameManagerScript.Instance.bouncesOnTopRight += 1;
				}
			} else if (coll.gameObject.transform.position.y < 0) {
				//GameManagerScript.Instance.bouncesOnBottom += 1;
				if (coll.gameObject.transform.position.x < 0) {
					//GameManagerScript.Instance.bouncesOnTop += 1;
					GameManagerScript.Instance.bouncesOnBottomLeft += 1;
				}
				if (coll.gameObject.transform.position.x > 0) {
					GameManagerScript.Instance.bouncesOnBottomRight += 1;
				}
			}


			CreateBounceImpact (coll, 1, 1);
			CreateBounceImpact (coll, 2, 2);
			CreateBounceImpact (coll, 3, 3);
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, .8f);

			// If there were two bounces on a side, take action
			if (GameManagerScript.Instance.bounces >= 2 && singleMode || GameManagerScript.Instance.bouncesOnTopLeft >= 2 && !singleMode || GameManagerScript.Instance.bouncesOnTopRight >= 2 && !singleMode || GameManagerScript.Instance.bouncesOnBottomRight >= 2 && !singleMode || GameManagerScript.Instance.bouncesOnBottomLeft >= 2 && !singleMode) {

				// reset current rally count
				DataManagerScript.currentRallyCount = 0;

				FireExplosion ();

				GameManagerScript.Instance.ManageScore (this.transform.position.x);
			} else {

				coll.gameObject.GetComponent<BorderScript> ().ChangeColor ();
				}
		} else if (coll.gameObject.tag == "Player"){
			//SoundManagerScript.Instance.RandomizeSfx (bounceOffPlayerSound1, bounceOffPlayerSound2);
		} else if (coll.gameObject.tag == "Playfield"){
			SoundManagerScript.instance.PlaySingle (bounceOffWallSound);

		}
	}
}
