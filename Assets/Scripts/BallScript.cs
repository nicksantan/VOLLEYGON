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
	public bool onePlayerMode = false;
	private int bounces = 0;
	public int bouncesOnTop;
	public int bouncesOnBottom;
	public int bouncesOnTopLeft;
	public int bouncesOnTopRight;
	public int bouncesOnBottomRight;
	public int bouncesOnBottomLeft;
	public float baseTimeBetweenGravChanges = 10f;
	private float lastXPos;
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

	public GameObject Arena1;
	public GameObject Arena2;
	public GameObject Arena3;
	public GameObject Arena4;

	GameObject CurrentArena;

	public int singleModeBalls;

	public AudioSource sfxSource;

	public AudioClip ballServedSound;
	public AudioClip pointScoredSound1;
	public AudioClip pointScoredSound2;
	public AudioClip gravityChangeSound;
	public AudioClip gravityIsAboutToChangeSound;
	public AudioClip bounceOffPlayerSound1;
	public AudioClip bounceOffPlayerSound2;
	public AudioClip bounceOffWallSound;
	public AudioClip bounceOffScoringBoundarySound; 
	public AudioClip largeHitSound;
	private float maxSpeed = 68f;
	private AudioSource audio;
	public bool didSirenPlayAlready;
	// Use this for initialization

	void Awake(){
		// Debug.Log (GameManagerScript.Instance.CurrentArena);
	    // CurrentArena = GameManagerScript.Instance.CurrentArena;
		// Debug.Log ("assign arena");
	}

	void Start () {
		audio = GetComponent<AudioSource> ();
		lastScore = 0;
		didSirenPlayAlready = false;
		lastTouch = 0;
		flashTime = 0f;
		secondToLastTouch = 0;
		timeSinceLastFlash = 0f;
		singleModeBalls = 3;
		rb = GetComponent<Rigidbody2D>();
		CurrentArena = GameManagerScript.Instance.CurrentArena;
		theSprite = GetComponent<SpriteRenderer>().sprite;
		rb.isKinematic = true;
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
		//CurrentArena = GameManagerScript.Instance.CurrentArena;
		//Debug.Log (GameManagerScript.Instance.CurrentArena);
		//Debug.Log (CurrentArena);
		if (rb.velocity.magnitude > 80f) {

			// Debug.Log (rb.velocity.magnitude);
		}
		if(rb.velocity.magnitude > maxSpeed)
		{

			rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
		}
		if (isTimerRunning) {
			timer -= Time.deltaTime;
			//Debug.Log (timer);
		}
		timeSinceLastFlash = Time.time - flashTime;

		if (timer <= 3 && timeSinceLastFlash >=.25f) {

			if (!didSirenPlayAlready) {
				//SoundManagerScript.instance.PlaySingle (gravityIsAboutToChangeSound);
				audio.Play();
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
			GameObject cTrail = Instantiate(circleTrail) as GameObject;
			cTrail.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 1f);
			cTrail.transform.parent = transform.Find("CircleTrails");
			cTrail.GetComponent<Renderer>().material.SetColor ("_Color", new Color32(244, 244, 244, 100));
			// for yellow, use new Color32(255, 248, 15, 100));
			cTrail.SendMessage("Config", 2);
//			if (GetComponent<SpriteRenderer> ().sprite == reverseGravSprite) {
//				GetComponent<SpriteRenderer>().sprite = originalSprite;
//			} else {
//				GetComponent<SpriteRenderer> ().sprite = reverseGravSprite;
//			};
		}
		if (timer <= 0){
			GravChange ();
			ResetTimer ();

			//Debug.Log (timer);
		}

		CheckForSideChange ();
		lastXPos = transform.position.x; 
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
			bounces = 0;
			bouncesOnTop = 0;
			bouncesOnBottom = 0;
			bouncesOnTopLeft = 0;
			bouncesOnTopRight = 0;
			bouncesOnBottomLeft = 0;
			bouncesOnBottomRight = 0;
			CurrentArena.BroadcastMessage ("ReturnColor");

			DataManagerScript.currentRallyCount += 1;
			if (DataManagerScript.currentRallyCount > DataManagerScript.longestRallyCount) {
				DataManagerScript.longestRallyCount = DataManagerScript.currentRallyCount;
				Debug.Log ("longest rally count is now " + DataManagerScript.longestRallyCount);
			}

			// Credit a return to the last touch player
			switch (lastTouch) {
			case 1:
				DataManagerScript.playerOneReturns += 1;
				break;
			case 2:
				DataManagerScript.playerTwoReturns += 1;
				break;
			case 3:
				DataManagerScript.playerThreeReturns += 1;
				break;
			case 4:
				DataManagerScript.playerFourReturns += 1;
				break;
				
			}

			if (onePlayerMode && lastXPos != 0) {
				GameManagerScript.Instance.GetComponent<GameManagerScript>().rallyCount++;
			}
			GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
		}
		
	}

	void FadeOutScore(){
		scoreText.CrossFadeAlpha(0f,.25f,false);
		winByTwoText.CrossFadeAlpha (0f, .25f, false);
	}

	void ResetBall(){
		trail.GetComponent<Trail>().ClearSystem (true);
		trail.SetActive (false);
		rb.isKinematic = true;
		gameObject.transform.position = new Vector3 (0, 0, 0);
		rb.velocity = new Vector2 (0, 0);
		bounces = 0;
		bouncesOnTop = 0;
		bouncesOnBottom = 0;
		bouncesOnBottomLeft = 0;
		bouncesOnBottomRight = 0;
		bouncesOnTopLeft = 0;
		bouncesOnTopRight = 0;
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
	void GameOver(){
		//Application.LoadLevel ("titleScene");
	}
	void ComputeStat(int whichTeamScored){
		if (whichTeamScored == 1) {
			if (lastTouch == 1) {
				DataManagerScript.playerOneAces += 1;
				DataManagerScript.playerOneScores += 1;
			}
			if (lastTouch == 2) {
				DataManagerScript.playerTwoAces += 1;
				DataManagerScript.playerTwoScores += 1;
			}

			if (lastTouch == 3) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerOneScores += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerTwoScores += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerThreeBumbles += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerThreeBumbles += 1;
				}
			}
			if (lastTouch == 4) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerOneScores += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerTwoScores += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerFourBumbles += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerFourBumbles += 1;
				}
			}
		}
		if (whichTeamScored == 2) {
			if (lastTouch == 3) {
				DataManagerScript.playerThreeAces += 1;
				DataManagerScript.playerThreeScores += 1;
			}
			if (lastTouch == 4) {
				DataManagerScript.playerFourAces += 1;
				DataManagerScript.playerFourScores += 1;
			}

			if (lastTouch == 1) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerOneBumbles += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerOneBumbles += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerThreeScores += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerFourScores += 1;
				}
			}
			if (lastTouch == 2) {
				if (secondToLastTouch == 1) {
					DataManagerScript.playerTwoBumbles += 1;
				}
				if (secondToLastTouch == 2) {
					DataManagerScript.playerTwoBumbles += 1;
				}
				if (secondToLastTouch == 3) {
					DataManagerScript.playerThreeScores += 1;
				}
				if (secondToLastTouch == 4) {
					DataManagerScript.playerFourScores += 1;
				}
			}
		}
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
	void CheckForMatchPoint(){
		// check for match point
		if (GameManagerScript.Instance.teamTwoScore == GameManagerScript.Instance.teamOneScore) {
			background.GetComponent<BackgroundColorScript> ().TurnOffMatchPoint ();
			//MusicManagerScript.Instance.SwitchMusic ();
		} else if (GameManagerScript.Instance.teamOneScore == GameManagerScript.Instance.scorePlayedTo - 1 && GameManagerScript.Instance.teamTwoScore < GameManagerScript.Instance.scorePlayedTo) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (1);
			background.GetComponent<BackgroundColorScript> ().TurnOffDeuce();
			MusicManagerScript.Instance.StartFifth ();
		} else if (GameManagerScript.Instance.teamTwoScore == GameManagerScript.Instance.scorePlayedTo - 1 && GameManagerScript.Instance.teamOneScore < GameManagerScript.Instance.scorePlayedTo) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (2);
			background.GetComponent<BackgroundColorScript> ().TurnOffDeuce();
			MusicManagerScript.Instance.StartFifth ();

		} 
	}
	void OnCollisionEnter2D(Collision2D coll){

		if (coll.gameObject.tag == "Wall") {
			SoundManagerScript.instance.PlaySingle(bounceOffWallSound);
		}

		if (coll.gameObject.tag == "ScoringBoundary") {
			//Debug.Log ("a collision!");
			SoundManagerScript.instance.PlaySingle(bounceOffScoringBoundarySound);
			bounces += 1;
			if (coll.gameObject.transform.position.y > 0) {
				if (coll.gameObject.transform.position.x < 0) {
					//bouncesOnTop += 1;
					bouncesOnTopLeft += 1;
				}
				if (coll.gameObject.transform.position.x > 0) {
					bouncesOnTopRight += 1;
				}
			} else if (coll.gameObject.transform.position.y < 0) {
				//bouncesOnBottom += 1;
				if (coll.gameObject.transform.position.x < 0) {
					//bouncesOnTop += 1;
					bouncesOnBottomLeft += 1;
				}
				if (coll.gameObject.transform.position.x > 0) {
					bouncesOnBottomRight += 1;
				}
			}
			CreateBounceImpact (coll, 1, 1);
			CreateBounceImpact (coll, 2, 2);
			CreateBounceImpact (coll, 3, 3);
			GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, .8f);
			if (bounces >= 2 && singleMode || bouncesOnTopLeft >= 2 && !singleMode || bouncesOnTopRight >= 2 && !singleMode || bouncesOnBottomRight >= 2 && !singleMode || bouncesOnBottomLeft >= 2 && !singleMode) {

				// reset current rally count
				DataManagerScript.currentRallyCount = 0;

				// Fire an explosion
				audio.Stop ();
				Vector3 newPos = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
				Instantiate (explosionPrefab, newPos, Quaternion.identity);
				if (newPos.y > 0) {
					Instantiate (explosionPrefab, newPos, Quaternion.Euler (0, 0, -180));
				} else {
					Instantiate (explosionPrefab, newPos, Quaternion.Euler (0, 0, 0));
				}
				SoundManagerScript.instance.RandomizeSfx (pointScoredSound1, pointScoredSound2);
				// Award a score.
				CurrentArena.BroadcastMessage("ReturnColor");
//				Arena1.BroadcastMessage("ReturnColor");
//				Arena2.BroadcastMessage("ReturnColor");
//				Arena3.BroadcastMessage("ReturnColor");
//				Arena4.BroadcastMessage("ReturnColor");
				if (!onePlayerMode) {
					if (Mathf.Sign (transform.position.x) < 0) {
						GameManagerScript.Instance.teamTwoScore += 1; 
						ComputeStat (2); 
						if (lastScore != 2) {
							MusicManagerScript.Instance.SwitchMusic (2);
						}

						lastScore = 2;
					} else {
						GameManagerScript.Instance.teamOneScore += 1;
						ComputeStat (1);

						if (lastScore != 1) {
							MusicManagerScript.Instance.SwitchMusic (1);
						}


						lastScore = 1;
					}

					if (GameManagerScript.Instance.teamTwoScore < GameManagerScript.Instance.scorePlayedTo && GameManagerScript.Instance.teamOneScore < GameManagerScript.Instance.scorePlayedTo) {

						//Mathf.Abs(GameManagerScript.Instance.teamOneScore - GameManagerScript.Instance.teamTwoScore) < 2
						if (GameManagerScript.Instance.teamTwoScore == GameManagerScript.Instance.scorePlayedTo - 1 && GameManagerScript.Instance.teamOneScore == GameManagerScript.Instance.scorePlayedTo - 1) {
							scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
							background.GetComponent<BackgroundColorScript> ().TurnOnDeuce ();
						} else {
						
							scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, false);
						}

						CheckForMatchPoint ();
						//scoreboard.GetComponent<ScoreboardManagerScript> ().enableDash ();
//
//					if (GameManagerScript.Instance.teamTwoScore >= GameManagerScript.Instance.scorePlayedTo || GameManagerScript.Instance.teamOneScore >= GameManagerScript.Instance.scorePlayedTo) {
//						//winByTwoText.CrossFadeAlpha (0.6f, .25f, false);
//						scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
//					}
						ResetBall ();
						//Instantiate(prefab, new Vector3(0f, 0, 0), Quaternion.identity);
						//Destroy (gameObject);
					} else if (Mathf.Abs (GameManagerScript.Instance.teamOneScore - GameManagerScript.Instance.teamTwoScore) < 2) {
						if (GameManagerScript.Instance.teamTwoScore >= GameManagerScript.Instance.scorePlayedTo || GameManagerScript.Instance.teamOneScore >= GameManagerScript.Instance.scorePlayedTo) {
							//winByTwoText.CrossFadeAlpha (0.6f, .25f, false);
							MusicManagerScript.Instance.StartFifth ();
							CheckForMatchPoint ();
							scoreboard.GetComponent<ScoreboardManagerScript> ().enableNumbers (GameManagerScript.Instance.teamOneScore, GameManagerScript.Instance.teamTwoScore, true);
						}
						ResetBall ();

					} else {
						// GAME IS OVER
						transform.position = new Vector3 (0f, 0f, 0f);
						gameObject.SetActive (false);
						//Invoke ("GameOver", 5f);
					}
				} else {
					// single mode
					singleModeBalls--;
					// Debug.Log ("scored");
					// generate a random number between one and two
					int randomTrack = Random.Range(1,3);
					MusicManagerScript.Instance.SwitchMusic (randomTrack);
					if (singleModeBalls <= 0) {
						// GAME IS OVER
						transform.position = new Vector3 (0f, 0f, 0f);
						gameObject.SetActive (false);
						GameManagerScript.Instance.endGame ();
					} else {
						ResetBall ();
					}
				}
			} else {
				coll.gameObject.GetComponent<BorderScript>().ChangeColor ();
			}

		} else if (coll.gameObject.tag == "Player"){
			//SoundManagerScript.instance.RandomizeSfx (bounceOffPlayerSound1, bounceOffPlayerSound2);
		} else if (coll.gameObject.tag == "Playfield"){
			SoundManagerScript.instance.PlaySingle (bounceOffWallSound);

		}
	}
}
