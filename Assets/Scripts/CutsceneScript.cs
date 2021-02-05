using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Rewired;

public class CutsceneScript : MonoBehaviour {

	public string nextScene;
	public float sceneDuration;
    public bool isSkippable = false;
    private int whichPlayerIsControlling;
    private JoystickButtons joyButts;
    private Player player;

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
        player = ReInput.players.GetPlayer(DataManagerScript.gamepadControllingMenus);
      //  joyButts = new JoystickButtons(whichPlayerIsControlling);
        if (isSkippable)
        {
            if (player.GetButtonDown("Grav") || player.GetButtonDown("Jump") || player.GetButtonDown("Start"))
            {
                NextScene();
            }
        }
    }
}
