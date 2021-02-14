using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;
	public static SoundManagerScript instance = null;

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	private float baseSfxVolume = 1f;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}
		SetVolume();
        DontDestroyOnLoad (gameObject);
	}

	void SetVolume() {
        float sfxVolume = FileSystemLayer.Instance.sfxVolume / 10f;
        float masterVolume = FileSystemLayer.Instance.masterVolume / 10f;
        sfxSource.volume = baseSfxVolume * masterVolume * sfxVolume;
	}

	public void PlaySingle (AudioClip clip){
		SetVolume();
		sfxSource.PlayOneShot(clip);
	}

	public void muteSFX(){
		sfxSource.volume = 0;
	}

	public void unMuteSFX(){
		SetVolume();
	}

	public void RandomizeSfx(params AudioClip[] clips){
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		SetVolume();
		sfxSource.pitch = randomPitch;
		sfxSource.PlayOneShot(clips [randomIndex]);
	}
}
