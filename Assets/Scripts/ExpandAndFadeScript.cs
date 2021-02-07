using UnityEngine;
using System.Collections;

public class ExpandAndFadeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		iTween.FadeTo (gameObject, 0f, 1.0f);
//		iTween.ScaleTo (gameObject, new Vector3 (5f, 5f, 1f), 3.0f);
//	    Invoke ("Remove", 3f);
	}

	void Remove(){
		Destroy(gameObject);
	}
	// Update is called once per frame
	void Update () {

	}

	void Config(int whichType){
		iTween.FadeTo (gameObject, 0f, 1.0f);

        // If players object is active, have active players rumble slightly
        if (GameObject.FindGameObjectWithTag("PlayersContainer") != null)
        {
            GameObject.FindGameObjectWithTag("PlayersContainer").BroadcastMessage("SmallRumblePulse", SendMessageOptions.DontRequireReceiver); ;
        }
		switch (whichType) {

		case 1:
			iTween.ScaleTo (gameObject, new Vector3 (5f, 5f, 1f), 3.0f);
			break;
		case 2:
			iTween.ScaleTo (gameObject, new Vector3 (10f, 10f, 1f), 3.0f);
			break;

		case 3:
			iTween.ScaleTo (gameObject, new Vector3 (15f, 15f, 1f), 3.0f);
			break;
		}

		Invoke ("Remove", 3f);
	}
}
