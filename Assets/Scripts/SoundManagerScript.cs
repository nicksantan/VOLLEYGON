using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;
	public static SoundManagerScript instance = null;

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;
	// Use this for initialization

	void Awake(){

		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}
        float sfxVolume = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 10f;
        float masterVolume = PlayerPrefs.HasKey("masterVolume") ? PlayerPrefs.GetFloat("masterVolume") : 10f;
        sfxSource.volume = sfxSource.volume * masterVolume / 10f * sfxVolume / 10f;
        DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle (AudioClip clip){
		//sfxSource.clip = clip;
		sfxSource.PlayOneShot (clip);
	}

	public void muteSFX(){
		sfxSource.volume = 0;
	}

	public void unMuteSFX(){
		sfxSource.volume = 1f;
	}

	public void RandomizeSfx(params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		sfxSource.pitch = randomPitch;
		//sfxSource.clip = clips [randomIndex];
		//sfxSource.Play ();
		sfxSource.PlayOneShot (clips [randomIndex]);
	}
}
