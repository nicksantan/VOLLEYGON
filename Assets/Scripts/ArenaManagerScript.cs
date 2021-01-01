using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

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
    public AudioClip tickUp;
    public AudioClip tickDown;

    public CarouselScript carousel;

    private new AudioSource audio;

    Axis va1;
    Axis va2;
    Axis va3;
    Axis va4;

    Axis va1_Xbox;
    Axis va2_Xbox;
    Axis va3_Xbox;
    Axis va4_Xbox;

    Axis ha1;
    Axis ha2;
    Axis ha3;
    Axis ha4;

    Axis ha1_Xbox;
    Axis ha2_Xbox;
    Axis ha3_Xbox;
    Axis ha4_Xbox;

    //VertAxis[] verticalAxes;
    List<Axis> verticalAxes = new List<Axis>();
    List<Axis> horizontalAxes = new List<Axis>();

    List<string> buttons = new List<string>();

    private EventSystem es;
    public GameObject curtain;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        float sfxVolume = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 10f;
        float masterVolume = PlayerPrefs.HasKey("masterVolume") ? PlayerPrefs.GetFloat("masterVolume") : 10f;
        audio.volume = audio.volume * masterVolume / 10f * sfxVolume / 10f;
        locked = false;

        va1 = new Axis("Vertical_P1");
        va2 = new Axis("Vertical_P2");
        va3 = new Axis("Vertical_P3");
        va4 = new Axis("Vertical_P4");

        va1_Xbox = new Axis("Vertical_P1_Xbox");
        va2_Xbox = new Axis("Vertical_P2_Xbox");
        va3_Xbox = new Axis("Vertical_P3_Xbox");
        va4_Xbox = new Axis("Vertical_P4_Xbox");

        verticalAxes.Add(va1);
        verticalAxes.Add(va2);
        verticalAxes.Add(va3);
        verticalAxes.Add(va4);

        verticalAxes.Add(va1_Xbox);
        verticalAxes.Add(va2_Xbox);
        verticalAxes.Add(va3_Xbox);
        verticalAxes.Add(va4_Xbox);

        ha1 = new Axis("Horizontal_P1");
        ha2 = new Axis("Horizontal_P2");
        ha3 = new Axis("Horizontal_P3");
        ha4 = new Axis("Horizontal_P4");

        ha1_Xbox = new Axis("Horizontal_P1_Xbox");
        ha2_Xbox = new Axis("Horizontal_P2_Xbox");
        ha3_Xbox = new Axis("Horizontal_P3_Xbox");
        ha4_Xbox = new Axis("Horizontal_P4_Xbox");

        horizontalAxes.Add(ha1);
        horizontalAxes.Add(ha2);
        horizontalAxes.Add(ha3);
        horizontalAxes.Add(ha4);

        horizontalAxes.Add(ha1_Xbox);
        horizontalAxes.Add(ha2_Xbox);
        horizontalAxes.Add(ha3_Xbox);
        horizontalAxes.Add(ha4_Xbox);

        buttons.Add(jumpButton1);
        buttons.Add(gravButton1);
        buttons.Add(jumpButton2);
        buttons.Add(gravButton2);
        buttons.Add(jumpButton3);
        buttons.Add(gravButton3);
        buttons.Add(jumpButton4);
        buttons.Add(gravButton4);

        buttons.Add(jumpButton1_Xbox);
        buttons.Add(gravButton1_Xbox);
        buttons.Add(jumpButton2_Xbox);
        buttons.Add(gravButton2_Xbox);
        buttons.Add(jumpButton3_Xbox);
        buttons.Add(gravButton3_Xbox);
        buttons.Add(jumpButton4_Xbox);
        buttons.Add(gravButton4_Xbox);

        Vector3 tempPos = new Vector3(markerXPositions[0], markerYPositions[0], 1f);
        marker.transform.position = tempPos;

        es = EventSystem.current;

		// Fade in
		curtain.SetActive(true);
        curtain.GetComponent<NewFadeScript>().Fade(0f);
    }

    void IncreasePlayCount(string whichType)
    {
        int tempTotal = PlayerPrefs.GetInt(whichType);
        tempTotal += 1;
        PlayerPrefs.SetInt(whichType, tempTotal);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && es.currentSelectedGameObject) {
			int selectedIndex = es.currentSelectedGameObject.transform.GetSiblingIndex();
            // Debug.Log("selected index is");
            //Debug.Log(selectedIndex);

            int whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
            JoystickButtons joystick = new JoystickButtons(whichPlayerIsControlling);
            //foreach (string butt in buttons) {
            if (Input.GetButtonDown(joystick.jump)) {

                    if (selectedIndex == 0) {

                        // Get and log random arena type
                        DataManagerScript.arenaType = Random.Range(0, numberOfArenas);
                        IncreasePlayCount("randomArenaPlays"); // log which arena

                    } else {

                        // Set and log chosen arena type
                        Debug.Log("selected index is");
                        Debug.Log(selectedIndex);
                        DataManagerScript.arenaType = selectedIndex;
                        IncreasePlayCount("arena" + selectedIndex + "Plays"); // log which arena

                    }

                    // Start fade to next scene
                    StartCoroutine("NextScene");
                }

        }
    }

    IEnumerator NextScene()
    {
        if (!locked) {
            locked = true;
            GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync("proTipScene");
        }
    }
}
