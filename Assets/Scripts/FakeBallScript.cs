using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PigeonCoopToolkit.Effects.Trails;

public class FakeBallScript : MonoBehaviour {

	public Text scoreText;
	public Text winByTwoText;
	public GameObject scoreboard;
	public GameObject background;
	Rigidbody2D rb;
	public float timer;
	float timeSinceLastFlash;
	float flashTime;
	private bool isTimerRunning;
	public float gravScale = 0.8f;
	private float originalGrav;
	private int bounces = 0;
	public float baseTimeBetweenGravChanges = 10f;
	private float lastXPos;
	public Sprite originalSprite;
	public Sprite reverseGravSprite;
	public Sprite changingSprite;
	public bool neverFire = false;
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
	public float startVel = 0f;
	public AudioSource sfxSource;
	public bool willGravChange = true;
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
	private int rallyCount = 12;
	public Text rallyCountText;
	private AudioSource audio;
	public bool didSirenPlayAlready;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();

		didSirenPlayAlready = false;
		lastTouch = 0;
		flashTime = 0f;
		secondToLastTouch = 0;
		timeSinceLastFlash = 0f;
		rb = GetComponent<Rigidbody2D>();

		rb.isKinematic = true;
        Invoke ("LaunchBall", 1f);
		
		timer = baseTimeBetweenGravChanges ; 
		rb.gravityScale = gravScale;
		originalGrav = gravScale;
	}

	// Update is called once per frame
	void Update () {

		if (willGravChange) {
			if (isTimerRunning) {
				timer -= Time.deltaTime;
				//Debug.Log (timer);
			}
			timeSinceLastFlash = Time.time - flashTime;

			if (timer <= 3 && timeSinceLastFlash >= .25f) {

				if (!didSirenPlayAlready) {
					// SoundManagerScript.instance.PlaySingle (gravityIsAboutToChangeSound);
					// audio.Play();
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
				// for yellow, use new Color32(255, 248, 15, 100));
				cTrail.SendMessage ("Config", 2);
				//			if (GetComponent<SpriteRenderer> ().sprite == reverseGravSprite) {
				//				GetComponent<SpriteRenderer>().sprite = originalSprite;
				//			} else {
				//				GetComponent<SpriteRenderer> ().sprite = reverseGravSprite;
				//			};
			}

			if (timer <= 0) {

				GravChange ();
				ResetTimer ();
			}
		}
		CheckForSideChange ();
		lastXPos = transform.position.x; 
	}

	void ResetTimer(){
		timer = baseTimeBetweenGravChanges * 2f;
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
		if (!neverFire) {
			rb.velocity = new Vector2 (Random.Range (0f, 0f), Random.Range (15f * rb.gravityScale, 15F * rb.gravityScale));
		}
			
	}

	void CheckForSideChange(){
		if (Mathf.Sign (transform.position.x) != Mathf.Sign (lastXPos) && lastXPos != 0 && transform.position.x != 0) {
			rallyCount++;
			rallyCountText.text = rallyCount.ToString();
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
		timer = 10; // arbitrary high number
		Transform child = gameObject.transform.Find("CircleTrails");
		child.gameObject.SetActive (false); 
		// Reset last touch information
		lastTouch = 0;
		secondToLastTouch = 0;

		GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f) ;
		Invoke ("LaunchBall", 3f);
		//Invoke ("FadeOutScore", 2f);
		Debug.Log("timer off");
		isTimerRunning = false;
		int signValue = Random.Range (0, 2) * 2 - 1;
		rb.gravityScale = originalGrav * signValue;

		if (Mathf.Sign (rb.gravityScale) < 0) {
			Debug.Log ("changing sprite?");
			//gravityIndicator.GetComponent<PlayAnimationScript> ().PlayAnimation ();
			GetComponent<SpriteRenderer>().sprite = reverseGravSprite;
			redball.SetActive (true);
			//blueball.SetActive (false);
		} else {
			//	gravityIndicator.GetComponent<PlayAnimationScript> ().PlayAnimation ();
			GetComponent<SpriteRenderer>().sprite = originalSprite;
			blueball.SetActive (true);
			redball.SetActive (false);
		}
	}

	void GravChange(){
		rb.gravityScale *= -1;
		//SoundManagerScript.instance.PlaySingle (gravityChangeSound);
		Debug.Log ("sign of gravity scale is " + Mathf.Sign (rb.gravityScale));
		if (Mathf.Sign (rb.gravityScale) < 0) {
			//Debug.Log ("changing sprite?");
			//GetComponent<SpriteRenderer>().sprite = reverseGravSprite;
			redball.SetActive(true);
			//blueball.SetActive(false);
			gravityIndicator.transform.localScale = new Vector3 (.8f, -.8f, 1f);
		} else {
			GetComponent<SpriteRenderer>().sprite = originalSprite;
			redball.SetActive(false);
			blueball.SetActive (true);
			gravityIndicator.transform.localScale = new Vector3 (.8f, .8f, 1f);
		}
		gravityIndicator.GetComponent<PlayAnimationScript> ().PlayAnimation ();
	}
	void GameOver(){
		//Application.LoadLevel ("titleScene");
	}
	void ComputeStat(int whichTeamScored){
		

	}

	void CreateBounceImpact (Collision2D coll, int whichType, int whichNum){
		
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
	void CheckForMatchPoint(){
		
	}
	void OnCollisionEnter2D(Collision2D coll){


	}
}
