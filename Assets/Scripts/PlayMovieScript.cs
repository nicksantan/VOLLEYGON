using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class PlayMovieScript : MonoBehaviour {
	public MovieTexture movTexture;
	public VideoPlayer vidPlayer;
	public GameObject pressStartAnimation;
	private bool fadedOut;

	void Start () {
		fadedOut = false;
		vidPlayer = GetComponent<VideoPlayer> ();
		//GetComponent<Renderer>().material.mainTexture = movTexture;
		//movTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {

		if (!fadedOut && !vidPlayer.isPlaying) {
			
			fadedOut = true;
			iTween.FadeTo (gameObject, 0f, .5f);
			Invoke ("FireAnimation", 1f);
			//gameObject.SetActive(false);
		}
			
	
	}

	void FireAnimation(){
		pressStartAnimation.GetComponent<PlayAnimationScript> ().PlayAnimation ();
	}
}
