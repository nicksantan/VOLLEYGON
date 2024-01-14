using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class ProtipManagerScript : MonoBehaviour {

	public GameObject protipContainer;
	private int whichProtip;
	public float proTipTime;
	public Text numberText;
	public string startButton1 = "Start_P1";
	public string startButton2 = "Start_P2";
	public string startButton3 = "Start_P3";
	public string startButton4 = "Start_P4";

    private bool locked;

	// Use this for initialization
	void Start () {
		GameObject curtain = GameObject.Find ("FadeCurtainCanvas");
		LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 0f, .5f);
		MusicManagerScript.Instance.StartIntro ();
		Invoke ("StartGame", proTipTime);
		// make this a common shared function somehow
		int playersActive = 0;
		locked = false;

		// make this a common function in a class
		if (DataManagerScript.playerOnePlaying == true) {
			playersActive++;
		}
		if (DataManagerScript.playerTwoPlaying == true) {
			playersActive++;
		}
		if (DataManagerScript.playerThreePlaying == true) {
			playersActive++;
		}
		if (DataManagerScript.playerFourPlaying == true) {
			playersActive++;
		}

		if (playersActive == 1) {
			whichProtip = 13;
		} else {
			whichProtip = Random.Range (0, protipContainer.transform.childCount);
		}
		ChooseRandomProtip ();
	}

	void StartGame(){
		NextScene();
	}

	void Update(){
		if (Input.GetButton (startButton2) && Input.GetButton (startButton3)) {
			NextScene();
		}
	}

	void NextScene(){
		if (!locked) {
			locked = true;
            GameObject curtain = GameObject.Find("FadeCurtainCanvas");
			LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("gameScene"); });
		}
	}

	void ChooseRandomProtip(){
		Transform protip = protipContainer.transform.GetChild (whichProtip);
		int textNumber = whichProtip + 1;
		numberText.text = textNumber.ToString () + "/" + protipContainer.transform.childCount.ToString ();
		protip.gameObject.SetActive (true);
	}
}
