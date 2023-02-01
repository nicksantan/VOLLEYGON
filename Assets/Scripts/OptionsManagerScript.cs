using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;

public class OptionsManagerScript : MonoBehaviour
{

	public GameObject curtain;
	public CarouselScript carousel;
	public GameObject options;
	public GameObject breadcrumb;
	public GameObject optionBreadcrumb;
    public GameObject optionBackBreadcrumb;
    public GameObject optionEditButton;
    public GameObject optionPlayButton;
    public GameObject optionViewButton;
    public GameObject optionToggleButton;
    public GameObject leftBackground;
	public GameObject rightBackground;
    
	public AudioClip selectSound;
	public AudioClip confirmSound;
	public AudioClip cancelSound;

	private int whichPlayerIsControlling;
	private JoystickButtons joyButts;

    public bool inAchievementsMenu = false;
	private int selectedIndex = 0;
	private bool optionIsOpen = false;

	private EventSystem es;

	static private int[] validIndexes = { 0, 1, 2, 5, 6, 7, 8};
    static private int[] toggleIndexes = { 7 };

    private Player player;

	void Start()
	{
	

		curtain.SetActive(true);
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 0f, .5f);
        MusicManagerScript.Instance.TurnOffEverything();
        MusicManagerScript.Instance.whichSource += 1;
        MusicManagerScript.Instance.whichSource = MusicManagerScript.Instance.whichSource % 2;
        MusicManagerScript.Instance.SwitchToSource();
		MusicManagerScript.Instance.StartRoot();

		es = EventSystem.current;
		if (es && es.currentSelectedGameObject)
		{
			selectedIndex = es.currentSelectedGameObject.transform.GetSiblingIndex();
		}

		// determine which controller is 'in control'.
		whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
        //joyButts = new JoystickButtons(whichPlayerIsControlling);
        player = ReInput.players.GetPlayer(whichPlayerIsControlling);

		if (JoystickLayerManager.Instance != null){
            JoystickLayerManager.Instance.AssignPlayerToEventSystem(whichPlayerIsControlling);
        }
    
    }

	// Update is called once per frame
	void Update()
	{
		// Check for selection to enable selectable option
		bool inputSelecting = (player != null) && player.GetButtonDown("Jump");
		bool optionIsSelectable = OptionsManagerScript.CheckSelectableOptionIndex(selectedIndex);
        // special case for how-to video
        // TODO: This is stupid.
        bool optionIsPlayable = selectedIndex == 5;
        bool optionIsViewable = selectedIndex == 6;
        bool optionIsToggleable = selectedIndex == 7 || selectedIndex == 8;
		optionEditButton.SetActive(optionIsSelectable && !optionIsOpen && !optionIsPlayable && !optionIsViewable && !optionIsToggleable);
        optionPlayButton.SetActive(optionIsSelectable && !optionIsOpen && optionIsPlayable);
        optionViewButton.SetActive(optionIsSelectable && !optionIsOpen && !optionIsPlayable && optionIsViewable);
        optionToggleButton.SetActive(optionIsSelectable && !optionIsOpen && !optionIsPlayable && optionIsToggleable);
		
        // Show options based on carousel slide selected
        if (es.currentSelectedGameObject && !inAchievementsMenu)
		{
			int selectedSlideIndex = es.currentSelectedGameObject.transform.GetSiblingIndex();
			if (selectedIndex != selectedSlideIndex)
			{
				ShowOption(selectedSlideIndex);

			}
		}

		// Check for cancel button
		if (player.GetButtonDown("Grav"))
		{
			SoundManagerScript.instance.PlaySingle(cancelSound);

			// Show/hide UI
			optionBreadcrumb.SetActive(false);
            optionBackBreadcrumb.SetActive(false);
            breadcrumb.SetActive(true);
			leftBackground.SetActive(false);
			rightBackground.SetActive(true);

            inAchievementsMenu = false;
			if (optionIsOpen)
			{
				// Disable currently selected option
				Transform selectedOption = options.transform.GetChild(selectedIndex);
				selectedOption.GetComponent<OptionScript>().disable();

				// Go back to carousel controls
				optionIsOpen = false;
			}
			else
			{
                // Go to previous scene
                LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("consoleTitle"); });
               
			}
		}

		if (inputSelecting && !optionIsOpen && optionIsSelectable && !optionIsPlayable && !inAchievementsMenu && !optionIsToggleable)
		{
            // Show/hide UI
            if (selectedIndex != 6 && selectedIndex != 7)
            {
                optionBreadcrumb.SetActive(true);
            } else
            {
                inAchievementsMenu = true;
                optionBackBreadcrumb.SetActive(true);
            }
			breadcrumb.SetActive(false);
			leftBackground.SetActive(true);
			rightBackground.SetActive(false);

			// Activate option
			SelectShownOption();
			optionIsOpen = true;
		}

        if (inputSelecting && !optionIsOpen && optionIsSelectable && optionIsPlayable)
        {
            // Load how to video
            SceneManager.LoadSceneAsync("howToVideoScene");
			SoundManagerScript.instance.PlaySingle(confirmSound);
        }

        if (inputSelecting && !optionIsOpen && optionIsSelectable && optionIsToggleable)
        {
            // vibration toggle or protip toggle
            ToggleShownOption();

        }

        // Disable carousel when we have an option open... unless it's achievements
        es.enabled = (!optionIsOpen || selectedIndex == 6);
		
        if (es.enabled && selectedIndex!=6)
		{
			// Reset selected item on re-enable
			es.SetSelectedGameObject(carousel.slides[selectedIndex].gameObject);
		}
	}

	// TODO: move this to a property on the slides
	static bool CheckSelectableOptionIndex(int index)
	{
		int pos = Array.IndexOf(OptionsManagerScript.validIndexes, index);
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
		SoundManagerScript.instance.PlaySingle(confirmSound);
		Transform selectedOption = options.transform.GetChild(selectedIndex);
		selectedOption.GetComponent<OptionScript>().enable();
	}

    void ToggleShownOption()
    {
		SoundManagerScript.instance.PlaySingle(confirmSound);
        Transform selectedOption = options.transform.GetChild(selectedIndex);
        selectedOption.GetComponent<ToggleScript>().Toggle();
    }

	// Not used? TODO: Remove
    public void SaveOption(float val)
    {
        // need to identify the option with the pref;
        // Used to set master volume

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
