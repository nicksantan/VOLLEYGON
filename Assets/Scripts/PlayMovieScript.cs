using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class PlayMovieScript : MonoBehaviour {
	private VideoPlayer vidPlayer;
	public GameObject pressStartAnimation;
	public GameObject stillLogo;
	public GameObject colorCyclingBG;

	private bool fadedOut;

	void Start () {
		fadedOut = false;
		vidPlayer = GetComponent<VideoPlayer> ();
		//GetComponent<Renderer>().material.mainTexture = movTexture;
		//movTexture.Play();
		Invoke("FadeOutVideo", 8f);
		Invoke ("StartColorCycling", 1f);
	}
	
	// Update is called once per frame
	void Update () {

//		if (!fadedOut && !vidPlayer.isPlaying) {
//			
//			fadedOut = true;
//			iTween.FadeTo (gameObject, 0f, .5f);
//			Invoke ("FireAnimation", 1f);
//			//gameObject.SetActive(false);
//		}
	}

	void StartColorCycling(){
		colorCyclingBG.SetActive (true);
	}

	void FadeOutVideo(){
		stillLogo.SetActive (true);

		iTween.FadeTo (gameObject, 0f, .5f);
		Invoke ("FireAnimation", 1f);
	}
	void FireAnimation(){
		pressStartAnimation.GetComponent<PlayAnimationScript> ().PlayAnimation ();
	}
}
