using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionScript : MonoBehaviour
{
    public Slider slider;
    public float slideDurationSeconds = 0.1f;
    public int whichType = 0;
    private bool isEnabled;
    private bool isSliding;
	private JoystickButtons joyButts;
    private EventSystem es;
    public GameObject firstSelectedObject;
    public GameObject parentSlide;
    public GameObject carousel;

    public GameObject a_text;
    public GameObject a_desc;


    void Start() {
		joyButts = new JoystickButtons(DataManagerScript.gamepadControllingMenus);
        es = EventSystem.current;
    }

    void Update() {

        // This is a slider type option
        if (whichType == 0)
        {
            if (slider != null && isEnabled && !isSliding)
            {
                float horizontal = Input.GetAxis(joyButts.horizontal);
                if (horizontal != 0)
                {
                    float newValue = horizontal > 0 ? slider.value + 1 : slider.value - 1;
                    StartCoroutine(SlideTo(newValue));
                }
            }
        } else if (whichType == 1)
        {
            // This is an achievement browser.

        }
    }

    public void enable() {
        isEnabled = true;
        if (whichType == 1 && firstSelectedObject != null)
        {
            es.enabled = true;
            es.SetSelectedGameObject(firstSelectedObject);
            // temporarily disable the carousel
            carousel.GetComponent<CarouselScript>().disabled = true;
        }
    }

    public void disable() {
		isEnabled = false;
        if (whichType == 1)
        {
            es.SetSelectedGameObject(parentSlide);
            carousel.GetComponent<CarouselScript>().disabled = false;
            a_text.SetActive(false);
            a_desc.SetActive(false);
        }
        
    }

    IEnumerator SlideTo(float newValue) {
		isSliding = true;
		slider.value = newValue;
        yield return new WaitForSeconds(slideDurationSeconds);
		isSliding = false;
    }
}
