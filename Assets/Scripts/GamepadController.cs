﻿using UnityEngine;

public class GamepadController : MonoBehaviour {

    public int joystick;
    public int slot = 0; // default to nonexistent slot
    private JoystickButtons buttons;
    private Axis horizontalAxis;
    private bool slotSelected = false;
    private bool readiedUp = false;

    // Use this for initialization
    void Start () {

        // Get button strings from joystick number 
        buttons = new JoystickButtons(joystick);

        // Get horizontal axis
        horizontalAxis = new Axis(buttons.horizontal);

        // Start at slot to match joystick int
        slot = joystick;

        moveIcon(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Joystick movements
        checkHorizontalAxis(horizontalAxis);

        // Select slot
        if (Input.GetButtonDown(buttons.jump))
        {
            if (!slotSelected)
            {
                selectSlotForJoystick();
            } else
            {
                readiedUp = true;
            }
        }

        // Unselect slot
        if (Input.GetButtonDown(buttons.grav))
        {
            if (!readiedUp)
            {
                unselectSlotForJoystick();
            } else
            {
                readiedUp = false;
            }
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

        // Debug.Log("Slot assigned: " + joystick);

        // Gamepad has been assigned
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

        // Gamepad has been unassigned
        slotSelected = false;
        readiedUp = false;
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

                // Get new slot
                iterateSlot(goingRight);

                // Move the icon
                moveIcon(true);
            }
            
        }
        else if (Input.GetAxisRaw(axis.axisName) == 0)
        {
            // Reset boolean to prevent scrolling more than one tick per press when joystick returns to 0
            axis.axisInUse = false;
        }
    }
    
    // Function for looping over availible slots and selecting next open slot
    void iterateSlot(bool goingRight)
    { 
        // Move up or down through joystick ints
        int difference = (goingRight) ? 1 : -1;

        // Look for next slot based on input direction
        int numberOfSlots = 4;
        int nextSlot = (slot + difference) % numberOfSlots;
        if (nextSlot == 0) nextSlot = numberOfSlots;

        // See if desired slot is taken
        bool slotTaken = false;
        switch (nextSlot)
        {
            case 1:
                slotTaken = DataManagerScript.playerOneJoystick != -1;
                break;
            case 2:
                slotTaken = DataManagerScript.playerTwoJoystick != -1;
                break;
            case 3:
                slotTaken = DataManagerScript.playerThreeJoystick != -1;
                break;
            case 4:
                slotTaken = DataManagerScript.playerFourJoystick != -1;
                break;
        }

        slot = nextSlot;

        if (slotTaken)
        {
            // Try again if slot taken
            iterateSlot(goingRight);
        }
    }

    void moveIcon(bool playSound)
    {
        // Get selected slot coordinates
        GameObject selectedSlotPlayer = GameObject.Find("Fake Player " + slot);
        float x = selectedSlotPlayer.transform.position.x + ((joystick * 2f) - 5f);

        // Move icon
        float y = gameObject.transform.position.y;
        gameObject.transform.position = new Vector3(x, y, 0f);

        if (playSound)
        {
            // TODO: Play sound effect
            // AudioClip tick = (goingUp) ? tickUp : tickDown;
            // audio.PlayOneShot(tick);
        }
    }
}
