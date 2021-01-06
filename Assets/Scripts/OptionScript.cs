using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    public Slider slider;
    public float slideDurationSeconds = 0.1f;

    private bool isEnabled;
    private bool isSliding;
	private JoystickButtons joyButts;

    void Start() {
		joyButts = new JoystickButtons(DataManagerScript.gamepadControllingMenus);
    }

    void Update() {
        if (slider != null && isEnabled && !isSliding) {
            float horizontal = Input.GetAxis(joyButts.horizontal);
            if (horizontal != 0) {
                float newValue = horizontal > 0 ? slider.value + 1 : slider.value - 1;
				StartCoroutine(SlideTo(newValue));
            }
        }
    }

    public void enable() {
        isEnabled = true;
    }

    public void disable() {
		isEnabled = false;
    }

    IEnumerator SlideTo(float newValue) {
		isSliding = true;
		slider.value = newValue;
        yield return new WaitForSeconds(slideDurationSeconds);
		isSliding = false;
    }
}
