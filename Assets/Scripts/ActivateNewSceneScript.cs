using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateNewSceneScript : MonoBehaviour {

	public void LoadByString(string sceneName){
		Application.LoadLevel (sceneName);
	}
}
