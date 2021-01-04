using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LauncherManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		SceneManager.LoadSceneAsync("robysoftScene");
	}

	// Update is called once per frame
	void Update () {

	}
}
