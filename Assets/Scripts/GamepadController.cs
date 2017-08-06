﻿using UnityEngine;

public class GamepadController : MonoBehaviour {

    public int joystick;
    public int slot = 0; // default to nonexistent slot
    private JoystickButtons buttons;
    private Axis horizontalAxis;
    private bool slotSelected = false;

	// Use this for initialization
	void Start () {

        // Get button strings from joystick number 
        buttons = new JoystickButtons(joystick);

        // Get horizontal axis
        horizontalAxis = new Axis(buttons.horizontal);

        // TEMP: Start at slot 1
        slot = 1;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Joystick movements
        checkHorizontalAxis(horizontalAxis);

        // Select slot
        if (Input.GetButtonDown(buttons.jump))
        {
            selectSlotForJoystick();
        }

        // Unselect slot
        if (Input.GetButtonDown(buttons.grav))
        {
            unselectSlotForJoystick();
        }
    }

    void selectSlotForJoystick()
    {
        // Set joystick for player slot
        switch (slot)
        {

            case 1:
                DataManagerScript.playerOneJoystick = joystick;
                break;
            case 2:
                DataManagerScript.playerTwoJoystick = joystick;
                break;
            case 3:
                DataManagerScript.playerThreeJoystick = joystick;
                break;
            case 4:
                DataManagerScript.playerFourJoystick = joystick;
                break;

        }

        Debug.Log("Slot assigned: " + joystick);

        // Gamepage has been assigned
        slotSelected = true;
    }

    void unselectSlotForJoystick()
    {
        // Set joystick for player slot back to nonsense int
        switch (slot)
        {

            case 1:
                DataManagerScript.playerOneJoystick = -1;
                break;
            case 2:
                DataManagerScript.playerTwoJoystick = -1;
                break;
            case 3:
                DataManagerScript.playerThreeJoystick = -1;
                break;
            case 4:
                DataManagerScript.playerFourJoystick = -1;
                break;

        }

        // Gamepage has been unassigned
        slotSelected = false;
    }

    void checkHorizontalAxis(Axis axis)
    {
        // Left or right pressed
        if (Input.GetAxisRaw(axis.axisName) > 0 || Input.GetAxisRaw(axis.axisName) < 0)
        {

            // Only proceed if slot is not already selected and joystick not already pressed
            if (axis.axisInUse == false && !slotSelected )
            {

                // Boolean to prevent scrolling more than one tick per press
                axis.axisInUse = true;

                // See if going right or left
                bool goingRight = Input.GetAxisRaw(axis.axisName) > 0;

                // Move up or down through joystick ints
                int difference = (goingRight) ? 1 : -1;

                // TODO: Play sound effect
                // AudioClip tick = (goingUp) ? tickUp : tickDown;
                // audio.PlayOneShot(tick);

                // Update selected slot
                int numberOfSlots = 4;
                slot = ( slot + difference ) % numberOfSlots;
                if (slot == 0) slot = numberOfSlots;
                Debug.Log(slot);

                // Move icon
                gameObject.transform.Translate(200 * slot, 0, 0);
            }

        }
        else if (Input.GetAxisRaw(axis.axisName) == 0)
        {
            // Reset boolean to prevent scrolling more than one tick per press when joystick returns to 0
            axis.axisInUse = false;
        }
    }
}
