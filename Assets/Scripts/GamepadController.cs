using UnityEngine;

public class GamepadController : MonoBehaviour {

    public int joystick;
    public int slot = 0;
    private JoystickButtons buttons;

	// Use this for initialization
	void Start () {

        // Get button strings from joystick number 
        buttons = new JoystickButtons(joystick);
		
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(buttons.jump))
        {

        }
    }
}
 