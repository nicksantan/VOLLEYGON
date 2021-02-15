using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Rewired;

public enum medalTypes
{
    none,
    bronze,
    silver,
    gold
}
public class ChallengeManagerScript : MonoBehaviour
{

    public GameObject ballPrefab;

    // Store which challenge is being played
    private int currentChallenge;

    // Store reference to challenge-level UI elements
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject instructionPanel;
    public GameObject challengeTitle;
    public GameObject challengeNumber;
    public GameObject timerText;
    public GameObject bestText;
    private Text timerTextObj;
    private Text bestTextObj;
    private bool musicChanged = false;
    private bool musicChangedTwice = false;
    private bool musicChangedThrice = false;

    public GameObject losePlayAgain;
    public GameObject winNextChallenge;

    public GameObject bestTimeWinText;
    public GameObject bestTimeLoseText;
    public GameObject newText;

    public GameObject winMedal;
    public GameObject loseMedal;

    public Sprite bronzeMedalImage;
    public Sprite silverMedalImage;
    public Sprite goldMedalImage;
    public Sprite noMedalImage;

    // Store a flag the individual challenge can reference to know whether to start or stop the challenge
    public bool challengeRunning = false;

    // Store a reference to the challenges container so we can activate the correct challenge
    public GameObject challengesContainer;

    // Manage the time of the challenge
    private float rawTimer = 0f;

    // Static singleton property
    public static ChallengeManagerScript Instance { get; private set; }

    private EventSystem es;

    private Player player;


    void Awake()
    {
        Instance = this;
        // Load the challenge the user requested
        Debug.Log("Switching to challenge " + DataManagerScript.challengeType);
        SwitchToChallenge(DataManagerScript.challengeType);
        currentChallenge = DataManagerScript.challengeType;
        DataManagerScript.lastViewedChallenge = DataManagerScript.challengeType;
        timerTextObj = timerText.GetComponent<Text>();
        bestTextObj = bestText.GetComponent<Text>();




    }

    void Start()
    {


        // Display instruction panel
        DisplayChallengeInstructions();
        MusicManagerScript.Instance.TurnOffEverything();
        MusicManagerScript.Instance.whichSource += 1;
        MusicManagerScript.Instance.whichSource = MusicManagerScript.Instance.whichSource % 2;
        MusicManagerScript.Instance.SwitchToSource();
        MusicManagerScript.Instance.StartMusic();
        es = EventSystem.current;
        // Assign joystick to player
        int joystickIdentifier = DataManagerScript.gamepadControllingMenus;
        
        if (JoystickLayerManager.Instance != null)
        {
            JoystickLayerManager.Instance.AssignPlayerToEventSystem(joystickIdentifier);
        }
        //JoystickButtons buttons = new JoystickButtons(joystickIdentifier);

        //es.GetComponent<StandaloneInputModule>().horizontalAxis = buttons.horizontal;
        //es.GetComponent<StandaloneInputModule>().verticalAxis = buttons.vertical;
        //es.GetComponent<StandaloneInputModule>().submitButton = buttons.jump;
        //es.GetComponent<StandaloneInputModule>().cancelButton = buttons.grav;
        // Load the best time for this challenge
        GameObject ICM = GameObject.FindWithTag("IndividualChallengeManager");
        if (ICM)
        {
            float bestTime = ICM.GetComponent<SaveChallengeTimeScript>().challengeTime;
            Debug.Log("Best time is ");
            Debug.Log(bestTime);
            if (bestTime < 999f)
            {
                Debug.Log(FormatTime(bestTime));
                bestTextObj.text = "BEST " + FormatTime(bestTime);
            }
            else
            {
                bestText.SetActive(false);
            }
        }

        // For now, just hide the panel in 3 seconds
        Invoke("HideChallengeInstructions", 3f);
    }

    void Update()
    {

        //Update the challenge timer 
        if (challengeRunning)
        {
            rawTimer += Time.deltaTime;

            timerTextObj.text = FormatTime(rawTimer);
            // Debug.Log(rawTimer);
            if (rawTimer > 20 && !musicChanged)
            {
                Debug.Log("music changed!");
                MusicManagerScript.Instance.SwitchMusic(2);
                musicChanged = true;
            }
            if (rawTimer > 40 && !musicChangedTwice)
            {
                Debug.Log("music changed!");
                MusicManagerScript.Instance.StartFourth();
                musicChangedTwice = true;
            }
            if (rawTimer > 70 && !musicChangedThrice)
            {
                Debug.Log("music changed!");
                MusicManagerScript.Instance.StartFifth();
                musicChangedThrice = true;
            }
        }
    }

    public void UpdateChallengeText(string newText)
    {
        challengeTitle.GetComponent<Text>().text = newText;
        // TODO: Make a helper function to format the challenge number string
        challengeNumber.GetComponent<Text>().text = "CHALLENGE 0" + (DataManagerScript.challengeType + 1);
        // Special case for challenge 10
        if (DataManagerScript.challengeType == 9)
        {
            challengeNumber.GetComponent<Text>().text = "CHALLENGE 10";
        }
    }

    private void SwitchToChallenge(int whichChallenge)
    {
        Transform challenge = challengesContainer.transform.GetChild(whichChallenge);
        challenge.gameObject.SetActive(true);
    }

    public void ReturnToChallengeMenu()
    {
        StartCoroutine("ReturnToChallengeMenuRoutine");
    }
    public IEnumerator ReturnToChallengeMenuRoutine()
    {

        GameObject.Find("FadeCurtainCanvas").GetComponent<NewFadeScript>().Fade(1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("chooseChallengeScene");
    }

    public string FormatTime(float rawTimer)
    {
        int minutes = Mathf.FloorToInt(rawTimer / 60F);
        int seconds = Mathf.FloorToInt(rawTimer - minutes * 60);
        float fraction = (rawTimer * 100) % 100;
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
        return niceTime;
    }

    public void DisplayChallengeInstructions()
    {
        instructionPanel.SetActive(true);
    }

    public void HideChallengeInstructions()
    {
        instructionPanel.SetActive(false);
        challengeRunning = true;
    }

    public void ChallengeFail()
    {
        // Display fail text
        losePanel.SetActive(true);
        challengeRunning = false;

        // set the next option to play again
        es.SetSelectedGameObject(losePlayAgain);

        // If there's a high score, show it on the lose panel.
        GameObject ICM = GameObject.FindWithTag("IndividualChallengeManager");
        float bestTime = ICM.GetComponent<SaveChallengeTimeScript>().challengeTime;
        Debug.Log("What was the best time?");
        Debug.Log(bestTime);
        if (bestTime <= 9998f || bestTime == 99999f)
        {
            bestTimeLoseText.SetActive(true);
            bestTimeLoseText.GetComponent<Text>().text = "BEST TIME: " + FormatTime(bestTime);

            MedalProvider mp = new MedalProvider(bestTime, currentChallenge);
            medalTypes whichMedal = mp.GetMedal();
            switch (whichMedal)
            {
                case medalTypes.none:
                    loseMedal.GetComponent<Image>().sprite = noMedalImage;
                    break;
                case medalTypes.bronze:
                    loseMedal.GetComponent<Image>().sprite = bronzeMedalImage;
                    break;
                case medalTypes.silver:
                    loseMedal.GetComponent<Image>().sprite = silverMedalImage;
                    break;
                case medalTypes.gold:
                    loseMedal.GetComponent<Image>().sprite = goldMedalImage;
                    break;
            }
            // Nudge over medal based on length of best time message. Provide a little delay because I think this is not running in time sometimes.
            Invoke("NudgeLoseMedal", .25f);
        }
        //For now, restart the challenge
        //   Invoke("RestartChallenge", 5f);
    }

    public void NudgeLoseMedal()
    {
        int[] widthOfChars = CalculateLengthOfMessage(bestTimeLoseText.GetComponent<Text>().text, bestTimeLoseText);
        float widthOfMedChars = widthOfChars[1] * 5f;
        float widthOfSmallChars = widthOfChars[0] * 10f;
        loseMedal.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(loseMedal.GetComponent<RectTransform>().localPosition.x - widthOfMedChars-widthOfSmallChars, -1.299999f, 0f); //Get rid of the magic numbers!
    }

    public void RestartChallenge()
    {
        Application.LoadLevel("challengeScene");
    }

    public void PlayNextChallenge()
    {

        DataManagerScript.challengeType = DataManagerScript.challengeType + 1;
        Debug.Log("INCREASED CHALLENGE NUM!");

        if (DataManagerScript.challengeType > 9)
        {
            DataManagerScript.lastViewedChallenge = 9;
            ReturnToChallengeMenu();
        }
        else
        {
            Application.LoadLevel("challengeScene");
        }

        ////TODO: Check for last challenge here

        ////Disable current challenge
        //Transform challenge = challengesContainer.transform.GetChild(currentChallenge);
        //challenge.gameObject.SetActive(false);

        ////Switch to new challenge
        //winPanel.SetActive(false);
        //currentChallenge += 1;
        //SwitchToChallenge(currentChallenge);

        //// Display instruction panel
        //DisplayChallengeInstructions();

        //// For now, just hide the panel in 3 seconds
        //Invoke("HideChallengeInstructions", 3f);
    }

    public void ChallengeSucceed()
    {

        // Display success text
        winPanel.SetActive(true);
        challengeRunning = false;

        // Make the winning player's gamepad rumble
        GameObject activePlayer = GameObject.FindGameObjectWithTag("Player");
        activePlayer.BroadcastMessage("WinRumble");

        // set the next option to play again
        Debug.Log("Setting next challenge");
        es.SetSelectedGameObject(winNextChallenge);

        // Find the ICM and log the time of the challenge
        GameObject ICM = GameObject.FindWithTag("IndividualChallengeManager");
        if (ICM)
        {
            ChallengeResult theResults = ICM.GetComponent<SaveChallengeTimeScript>().CompareTimes(rawTimer);
            Debug.Log("Checking time " + rawTimer + " against best time");
            bestTimeWinText.GetComponent<Text>().text = "BEST TIME: " + FormatTime(theResults.challengeTime);
            if (theResults.wasBestTime)
            {
                newText.SetActive(true);
                // Nudge over the whole text object slightly
                newText.transform.parent.parent.GetComponent<RectTransform>().localPosition = new Vector3(50f, 257.3f, 0f);
            }

            MedalProvider mp = new MedalProvider(theResults.challengeTime, currentChallenge);
            medalTypes whichMedal = mp.GetMedal();
            switch (whichMedal)
            {
                case medalTypes.none:
                    winMedal.GetComponent<Image>().sprite = noMedalImage;
                    break;
                case medalTypes.bronze:
                    winMedal.GetComponent<Image>().sprite = bronzeMedalImage;
                    break;
                case medalTypes.silver:
                    winMedal.GetComponent<Image>().sprite = silverMedalImage;
                    break;
                case medalTypes.gold:
                    winMedal.GetComponent<Image>().sprite = goldMedalImage;
                    break;
            }

            // Nudge over medal based on length of best time message.
            Invoke("NudgeWinMedal", .25f);

        }
        else
        {
            Debug.Log("Couldn't find ICM");
        }
        //For now, try playing the next challenge
        //Invoke("PlayNextChallenge", 5f);


    }

    void NudgeWinMedal()
    {
        int[] widthOfChars = CalculateLengthOfMessage(bestTimeWinText.GetComponent<Text>().text, bestTimeWinText);
        float widthOfMedChars = widthOfChars[1] * 5f;
        float widthOfSmallChars = widthOfChars[0] * 10f;
        winMedal.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(winMedal.GetComponent<RectTransform>().localPosition.x - widthOfSmallChars - widthOfMedChars, -1.299999f, 0f); //Get rid of the magic numbers!
    }

    int[] CalculateLengthOfMessage(string message, GameObject textObj)
    {
        int totalSmallChars = 0;
        int totalMedChars = 0;

        for (int i = 0; i < message.Length; i++)
        {
            char letter = message[i];

            if (letter.ToString() == "1")
            {
                totalSmallChars++;
            }

            if (letter.ToString() == "8" || letter.ToString() == "7" || letter.ToString() == "6" || letter.ToString() == "3" || letter.ToString() == "2")
            {
                totalMedChars++;
            }
        }

        int[] results = new int[2];
        results[0] = totalSmallChars;
        results[1] = totalMedChars;

        return results;
    }

}

public class MedalProvider
{
    public int whichChallenge;
    public float challengeTime;

    List<List<float>> medalRanges = new List<List<float>>();


     // List of List<int>
 
    public MedalProvider(float challengeTime, int whichChallenge)
        {
            this.challengeTime = challengeTime;
            this.whichChallenge = whichChallenge;

            medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
            medalRanges[0].Add(20);  // Add the integer '2' to the List<int> at index '0'
            medalRanges[0].Add(40);  // Add the integer '2' to the List<int> at index '0'
            medalRanges[0].Add(60);  // Add the integer '2' to the List<int> at index '0'

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[1].Add(10);  // Add the integer '2' to the List<int> at index '0'
        medalRanges[1].Add(20);  // Add the integer '2' to the List<int> at index '0'
        medalRanges[1].Add(30);  // Add the integer '2' to the List<int> at index '0'

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[2].Add(20);
        medalRanges[2].Add(40);
        medalRanges[2].Add(60);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[3].Add(20);
        medalRanges[3].Add(40);
        medalRanges[3].Add(60);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[4].Add(20);
        medalRanges[4].Add(40);
        medalRanges[4].Add(60);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[5].Add(30);
        medalRanges[5].Add(60);
        medalRanges[5].Add(90);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[6].Add(15);
        medalRanges[6].Add(30);
        medalRanges[6].Add(60);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[7].Add(30);
        medalRanges[7].Add(60);
        medalRanges[7].Add(90);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[8].Add(30);
        medalRanges[8].Add(60);
        medalRanges[8].Add(90);

        medalRanges.Add(new List<float>());  // Adding a new List<int> to the List.
        medalRanges[9].Add(60);
        medalRanges[9].Add(90);
        medalRanges[9].Add(120);
    }


    public medalTypes GetMedal()
    {
        medalTypes medalReached = medalTypes.none;
        if (challengeTime < medalRanges[whichChallenge][2])
        {
            medalReached = medalTypes.bronze;
        }
        if (challengeTime < medalRanges[whichChallenge][1])
        {
            medalReached = medalTypes.silver;
        }
        if (challengeTime < medalRanges[whichChallenge][0])
        {
            medalReached = medalTypes.gold;
        }

        return medalReached;
    }

}