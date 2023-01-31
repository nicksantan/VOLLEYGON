using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Rewired;

public class CutsceneScript : MonoBehaviour {

	public string nextScene;
	public float sceneDuration;
    public bool isSkippable = false;
    public bool isSkippableToTitle = true;
    private int whichPlayerIsControlling;
    private JoystickButtons joyButts;
    private Player player;
    private Player playerTwo;
    private Player playerThree;
    private Player playerFour;
    public bool playerWantsToSkipToTitle = false;

    // Use this for initialization
    void OnEnable () {
		Invoke ("NextScene", sceneDuration);
	}

	void NextScene(){
        if (gameObject.activeSelf == true){
         if (playerWantsToSkipToTitle){
                SceneManager.LoadSceneAsync("titleScene");
         } else {
	    	SceneManager.LoadSceneAsync(nextScene);
          }
        }
	}
    // Update is called once per frame
    void Update() {
    //    whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
        player = ReInput.players.GetPlayer(0);
        playerTwo = ReInput.players.GetPlayer(1);
        playerThree = ReInput.players.GetPlayer(2);
        playerFour = ReInput.players.GetPlayer(3);
      //  joyButts = new JoystickButtons(whichPlayerIsControlling);
        if (isSkippable)
        {
            if (player.GetButtonDown("Grav") || player.GetButtonDown("Jump") || player.GetButtonDown("Start")
            || playerTwo.GetButtonDown("Grav") || playerTwo.GetButtonDown("Jump") || playerTwo.GetButtonDown("Start")
            || playerThree.GetButtonDown("Grav") || playerThree.GetButtonDown("Jump") || playerThree.GetButtonDown("Start")
            || playerFour.GetButtonDown("Grav") || playerFour.GetButtonDown("Jump") || playerFour.GetButtonDown("Start"))
            {
                if (isSkippableToTitle){
                playerWantsToSkipToTitle = true;
                }
                NextScene();
            }
        }
    }
}
