using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;


public class ChallengeCarouselScript : MonoBehaviour
{

    public GameObject curtain;
    public CarouselScript carousel;
    public GameObject options;
    public GameObject breadcrumb;
    public GameObject optionBreadcrumb;
    public GameObject optionEditButton;
    public GameObject optionPlayButton;
    public GameObject leftBackground;
    public GameObject rightBackground;

    private int whichPlayerIsControlling;
    private JoystickButtons joyButts;
    private Player player;

    private int selectedIndex = 0;
    private bool optionIsOpen = false;

    public AudioClip nextSceneSound;
    public AudioClip prevSceneSound;

    private EventSystem es;

    static private int[] validIndexes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    void Start()
    {
        // intro music?
      

        curtain.SetActive(true);
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 0f, .5f);
       // MusicManagerScript.Instance.FadeOutEverything();
        	MusicManagerScript.Instance.StartIntro ();
        es = EventSystem.current;
        if (es && es.currentSelectedGameObject)
        {
            selectedIndex = es.currentSelectedGameObject.transform.GetSiblingIndex();
            Debug.Log("selected index");
            Debug.Log(es.currentSelectedGameObject.transform.GetSiblingIndex());
           
        }

        // determine which controller is 'in control'.
        whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
        //joyButts = new JoystickButtons(whichPlayerIsControlling);
        player = ReInput.players.GetPlayer(whichPlayerIsControlling);

        if (DataManagerScript.lastViewedChallenge == 0)
        {
            ShowOption(0);
        }

        if (AchievementManagerScript.Instance != null)
        {

            // Report progress to achievement manager
            AchievementManagerScript.Instance.LogMedalProgress(HowManyMedals(), HowManyGoldMedals());
            Debug.Log("counting medals");
            if (IsAllMedals())
            {
                AchievementManagerScript.Instance.Achievements[8].Unlock();
            }
            if (IsAllGoldMedals())
            {
                AchievementManagerScript.Instance.Achievements[7].Unlock();
            }
        }
    
}

    public bool IsAllMedals()
    {
        Debug.Log("running all medals");
        for (int i = 1; i < 11; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i-1);
            medalTypes whichMedal = mp.GetMedal();
            Debug.Log("medal?");
            Debug.Log(whichMedal);
            if (whichMedal == medalTypes.none)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsAllGoldMedals()
    {

        for (int i = 1; i < 11; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i-1);
            medalTypes whichMedal = mp.GetMedal();
            if (whichMedal != medalTypes.gold)
            {
                return false;
            }
        }

        return true;
    }

    public int HowManyMedals()
    {
        int howMany = 0;
        for (int i = 1; i < 11; i++)
        {
            Debug.Log(i);
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i-1);
            medalTypes whichMedal = mp.GetMedal();
            if (whichMedal != medalTypes.none)
            {
                howMany++;
            }
        }
        return howMany;
    }

    public int HowManyGoldMedals()
    {
        int howMany = 0;
        for (int i = 1; i < 11; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i-1);
            medalTypes whichMedal = mp.GetMedal();
            if (whichMedal == medalTypes.gold)
            {
                howMany++;
            }
        }
        return howMany;
    }

    // Update is called once per frame
    void Update()
    {
        //MusicManagerScript.Instance.FadeOutEverything();
        // Check for selection to enable selectable option
        bool inputSelecting = player.GetButtonDown("Jump");
        bool optionIsSelectable = ChallengeCarouselScript.CheckSelectableOptionIndex(selectedIndex);
        optionPlayButton.SetActive(optionIsSelectable && !optionIsOpen);
        // Show options based on carousel slide selected
        if (es.currentSelectedGameObject)
        {
            int selectedSlideIndex = es.currentSelectedGameObject.transform.GetSiblingIndex();
            if (selectedIndex != selectedSlideIndex)
            {
                ShowOption(selectedSlideIndex);
            }
        }

        // Check for cancel button
        if (player.GetButtonDown("grav"))
        {

            // Show/hide UI
            if (optionBreadcrumb)
            {
                optionBreadcrumb.SetActive(false);
            }
            breadcrumb.SetActive(true);
     
    
            // Go to previous scene
            SoundManagerScript.instance.PlaySingle(prevSceneSound);
            LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("consoleTitle"); });
        }

        if (inputSelecting && !optionIsOpen && optionIsSelectable)
        {

            // Show/hide UI
            if (optionBreadcrumb)
            {
                optionBreadcrumb.SetActive(true);
            }
            breadcrumb.SetActive(false);
            leftBackground.SetActive(true);
            rightBackground.SetActive(false);

            // Activate option
            SelectShownOption();
            optionIsOpen = true;
        }

        // Disable carousel when we have an option open
        es.enabled = !optionIsOpen;
        if (es.enabled)
        {
            // Reset selected item on re-enable
            es.SetSelectedGameObject(carousel.slides[selectedIndex].gameObject);
        }
    }

    // TODO: move this to a property on the slides
    static bool CheckSelectableOptionIndex(int index)
    {
        int pos = Array.IndexOf(ChallengeCarouselScript.validIndexes, index);
        return pos > -1;
    }

    void ShowOption(int newIndex)
    {
        // Hide all other options
        foreach (Transform child in options.transform)
        {
            if (child.gameObject.activeSelf)
            {
                StartCoroutine(FadeOption(child, false));
            }
        }

        // Show new option
        Transform optionToShow = options.transform.GetChild(newIndex);
        StartCoroutine(FadeOption(optionToShow, true));

        // Save new index
        selectedIndex = newIndex;
    }

    void SelectShownOption()
    {
        Transform selectedOption = options.transform.GetChild(selectedIndex);
        // Set chosen challenge
        DataManagerScript.challengeType = selectedIndex;
        SoundManagerScript.instance.PlaySingle(nextSceneSound);

        NextScene();
    }

    void NextScene()
    {
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("challengeScene"); });
    }

    // Don't believe this is used. TODO: Remove
    public void SaveOption(float val)
    {
        // need to identify the option with the pref;
      // (used to set master volume to val)

    }

    IEnumerator FadeOption(Transform option, bool isAppearing)
    {
        CanvasGroup optionCanvasGroup = option.GetComponent<CanvasGroup>();
        float approxNoOfFrames = 20;

        // Start state
        optionCanvasGroup.alpha = isAppearing ? 0 : 1;
        if (isAppearing) option.gameObject.SetActive(true);

        // Delay fade in
        yield return new WaitForSeconds(isAppearing ? 0.15f : 0);

        for (float i = 0; i < approxNoOfFrames; i++)
        {
            yield return new WaitForEndOfFrame();
            float delta = i / approxNoOfFrames;
            float newAlpha = isAppearing ? delta : 1 - delta;
            optionCanvasGroup.alpha = newAlpha;
        }

        yield return new WaitForEndOfFrame();
        if (!isAppearing) option.gameObject.SetActive(false);
    }
}
