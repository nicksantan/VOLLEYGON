using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class ArenaManagerScript : MonoBehaviour {


    private int markerPos = 0;
    private float[] markerYPositions = { 7.4f, 3.7f, 0f, -3.7f, -7.4f };
    private float[] markerXPositions = { -17.58f, -1.81f };

    // TODO: Use JoystickButtons object
    private string jumpButton1 = "Jump_P1";
    private string gravButton1 = "Grav_P1";
    private string vertAxis1 = "Vertical_P1";
    private string jumpButton2 = "Jump_P2";
    private string gravButton2 = "Grav_P2";
    private string vertAxis2 = "Vertical_P2";
    private string jumpButton3 = "Jump_P3";
    private string gravButton3 = "Grav_P3";
    private string vertAxis3 = "Vertical_P3";
    private string jumpButton4 = "Jump_P4";
    private string gravButton4 = "Grav_P4";
    private string vertAxis4 = "Vertical_P4";
    private string jumpButton1_Xbox = "Jump_P1_Xbox";
    private string jumpButton2_Xbox = "Jump_P2_Xbox";
    private string jumpButton3_Xbox = "Jump_P3_Xbox";
    private string jumpButton4_Xbox = "Jump_P4_Xbox";
    private string gravButton1_Xbox = "Grav_P1_Xbox";
    private string gravButton2_Xbox = "Grav_P2_Xbox";
    private string gravButton3_Xbox = "Grav_P3_Xbox";
    private string gravButton4_Xbox = "Grav_P4_Xbox";

    public GameObject marker;
    private bool axis1InUse = false;
    private bool axis2InUse = false;
    private bool axis3InUse = false;
    private bool axis4InUse = false;
    private int numberOfArenas = 9;
    private bool locked = false;

    public AudioClip nextSceneSound;
    public AudioClip prevSceneSound;

    public CarouselScript carousel;

    private Player player;

    vAxis va1;
    vAxis va2;
    vAxis va3;
    vAxis va4;

    vAxis va1_Xbox;
    vAxis va2_Xbox;
    vAxis va3_Xbox;
    vAxis va4_Xbox;

    vAxis ha1;
    vAxis ha2;
    vAxis ha3;
    vAxis ha4;

    vAxis ha1_Xbox;
    vAxis ha2_Xbox;
    vAxis ha3_Xbox;
    vAxis ha4_Xbox;

    //VertAxis[] verticalAxes;
    List<vAxis> verticalAxes = new List<vAxis>();
    List<vAxis> horizontalAxes = new List<vAxis>();

    List<string> buttons = new List<string>();

    private EventSystem es;
    public GameObject curtain;

    // Use this for initialization
    void Start()
    {

        player = ReInput.players.GetPlayer(DataManagerScript.gamepadControllingMenus);
        locked = false;

        Vector3 tempPos = new Vector3(markerXPositions[0], markerYPositions[0], 1f);
        marker.transform.position = tempPos;

        es = EventSystem.current;

		// Fade in
		curtain.SetActive(true);
        curtain.GetComponent<NewFadeScript>().Fade(0f);

        int whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
        // JoystickButtons joystick = new JoystickButtons(whichPlayerIsControlling);

        if (JoystickLayerManager.Instance != null){
            JoystickLayerManager.Instance.AssignPlayerToEventSystem(whichPlayerIsControlling);
        }
      
    }

    void IncreasePlayCount(int whichType)
    {
        int tempTotal = FileSystemLayer.Instance.arenaPlays[whichType];
        tempTotal += 1;
        FileSystemLayer.Instance.SaveArenaPlay(whichType, tempTotal);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && es != null && es.currentSelectedGameObject) {
			int selectedIndex = es.currentSelectedGameObject.transform.GetSiblingIndex();
     
            //foreach (string butt in buttons) {
            if (player.GetButtonDown("Jump")) {

                if (selectedIndex == 0) {

                    // Get and log random arena type
                    DataManagerScript.arenaType = Random.Range(0, numberOfArenas);
                    IncreasePlayCount(0); // log which arena

                } else {

                    // Set and log chosen arena type
                    Debug.Log("selected index is");
                    Debug.Log(selectedIndex);
                    DataManagerScript.arenaType = selectedIndex;
                    IncreasePlayCount(selectedIndex); // log which arena

                }

                // Start fade to next scene
                SoundManagerScript.instance.PlaySingle(nextSceneSound);
                StartCoroutine("NextScene");

            } else if (player.GetButtonDown("Grav")) {
                SoundManagerScript.instance.PlaySingle(prevSceneSound);
                StartCoroutine("PrevScene");
            }

        }
    }

    IEnumerator NextScene()
    {
        if (!locked) {
            locked = true;
            GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
            yield return new WaitForSeconds(1f);
            if (FileSystemLayer.Instance.protipsOn == 1)
            {
                SceneManager.LoadSceneAsync("proTipScene");
            } else
            {
                SceneManager.LoadSceneAsync("gameScene");
            }

        }
    }

    IEnumerator PrevScene()
    {
        if (!locked)
        {
            locked = true;
            GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync("choosePlayerScene");
        }
    }
}
