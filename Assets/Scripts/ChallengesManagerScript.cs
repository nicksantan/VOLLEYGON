using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengesManagerScript : MonoBehaviour {

    private int markerPos = 0;
    private float[] markerYPositions = { 7.4f, 3.7f, 0f, -3.7f, -7.4f };
    private float[] markerXPositions = { -17.58f, -1.81f };
	private int joystickIdentifier = 0;
    public GameObject marker;
    private bool axis1InUse = false;
    private bool axis2InUse = false;
    private bool axis3InUse = false;
    private bool axis4InUse = false;
    private int numberOfChallenges = 9;
    private bool locked = false;
    public AudioClip tickUp;
    public AudioClip tickDown;

    private new AudioSource audio;

	Axis verticalAxis;
	Axis horizontalAxis;

	public JoystickButtons buttons;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        locked = false;

        // Assign joystick to player
		joystickIdentifier = DataManagerScript.gamepadControllingMenus;
        buttons = new JoystickButtons(joystickIdentifier);

        // Get axis string from joystick class
        verticalAxis = new Axis(buttons.vertical);
		horizontalAxis = new Axis(buttons.horizontal);

        Vector3 tempPos = new Vector3(markerXPositions[0], markerYPositions[0], 1f);
        marker.transform.position = tempPos;
    }

    void Update()
    {
        if (!locked){

                if (Input.GetAxisRaw(verticalAxis.axisName) < 0){
                    if (verticalAxis.axisInUse == false)
                    {
                        // Call your event function here.
                        verticalAxis.axisInUse = true;
                        markerPos++;
                        updateMarkerPos();
                        audio.PlayOneShot(tickUp);
                    }
                }

                if (Input.GetAxisRaw(verticalAxis.axisName) > 0)
                {
                    if (verticalAxis.axisInUse == false)
                    {
                        // Call your event function here.
                        verticalAxis.axisInUse = true;
                        markerPos--;
                        audio.PlayOneShot(tickDown);
                        updateMarkerPos();
                    }
                }

                if (Input.GetAxisRaw(verticalAxis.axisName) == 0)
                {
                    verticalAxis.axisInUse = false;
                }



                if (Input.GetAxisRaw(horizontalAxis.axisName) < 0)
                {
				if (horizontalAxis.axisInUse == false)
                    {
                        // Call your event function here.
					horizontalAxis.axisInUse = true;
                        markerPos += 5;
                        updateMarkerPos();
                        audio.PlayOneShot(tickUp);

                    }
                }

			if (Input.GetAxisRaw(horizontalAxis.axisName) > 0)
                {
				if (horizontalAxis.axisInUse == false)
                    {
                        // Call your event function here.
					horizontalAxis.axisInUse = true;
                        markerPos += 5;
                        audio.PlayOneShot(tickDown);
                        updateMarkerPos();
                    }
                }

			if (Input.GetAxisRaw(horizontalAxis.axisName) == 0)
                {
				horizontalAxis.axisInUse = false;
                }

            {
				if (Input.GetButtonDown(buttons.jump))
                {
                    // Set chosen challenge
                    DataManagerScript.challengeType = markerPos;
                    StartCoroutine("NextScene");
                }
            }
        }
    }

    IEnumerator NextScene()
    {
        locked = true;
        float fadeTime = GameObject.Find("FadeCurtain").GetComponent<FadingScript>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
       SceneManager.LoadScene("challengeScene");
    }

    void updateMarkerPos()
    {

        if (markerPos < 0)
        {
            markerPos = numberOfChallenges;
        }
        markerPos = markerPos % (numberOfChallenges + 1);
        float posX;
        float posY;

        if (markerPos > 4)
        {
            posX = markerXPositions[1];
            marker.transform.localScale = new Vector3(-3f, marker.transform.localScale.y, marker.transform.localScale.z);
            posY = markerYPositions[markerPos - 5];

        }
        else
        {
            posX = markerXPositions[0];
            marker.transform.localScale = new Vector3(3f, marker.transform.localScale.y, marker.transform.localScale.z);
            posY = markerYPositions[markerPos];
        }

        Vector3 tempPos = new Vector3(posX, posY, 1f);
        marker.transform.position = tempPos;
    }
}