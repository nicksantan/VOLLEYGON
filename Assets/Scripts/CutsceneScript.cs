using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneScript : MonoBehaviour {

	public string nextScene;
	public float sceneDuration;
    public bool isSkippable = false;
    private int whichPlayerIsControlling;
    private JoystickButtons joyButts;

    // Use this for initialization
    void Start () {
		Invoke ("NextScene", sceneDuration);
	}

	void NextScene(){
		SceneManager.LoadSceneAsync(nextScene);
	}
    // Update is called once per frame
    void Update() {
        whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
        joyButts = new JoystickButtons(whichPlayerIsControlling);
        if (isSkippable)
        {
            if (Input.GetButtonDown(joyButts.grav) || Input.GetButtonDown(joyButts.jump) || Input.GetButtonDown(joyButts.start))
            {
                NextScene();
            }
        }
    }
}
