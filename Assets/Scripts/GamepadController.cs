using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class GamepadController : MonoBehaviour {

    public int joystick;
    public int slot = 0; // default to nonexistent slot
    public JoystickButtons buttons;
    private GameObject selectedSlotPlayer;
    private vAxis horizontalAxis;
    private bool slotSelected = false;
    public bool enabled = false;
    private bool readiedUp = false;
    private bool playerReady = false;
    private bool playerTagged = false;
    private bool shouldActivate = false;

    private GameObject outline;
    private GameObject icon;
    private GameObject playerText;
    private GameObject prompt;

    private Color32 gray = new Color32(40, 40, 40, 255);
    private Color32 white = new Color32(255, 255, 255, 255);

    private Player player;

    public bool botMode = false;

    private bool axisInUse = false;

    // Use this for initialization
    void Start () {

        outline = transform.Find("Outline").gameObject;
        icon = transform.Find("Icon").gameObject;
        playerText = transform.Find("Text").gameObject;
        prompt = transform.Find("Prompt").gameObject;

        // Get button strings from joystick number
        //buttons = new JoystickButtons(joystick);
        player = ReInput.players.GetPlayer(joystick - 1);

        // Get horizontal axis
       // horizontalAxis = new vAxis(buttons.horizontal);

        // Start at slot to match joystick int
        slot = joystick;

        if (botMode)
        {
            slot = 99;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("I'm enabled?");
        //Debug.Log(enabled);
        //Debug.Log("I'm in slot");
        //Debug.Log(slot);
        // Get ready state from manager
        switch (slot)
        {
            case 1:
                playerReady = ChoosePlayerScript.Instance.player1Ready;
                playerTagged = DataManagerScript.playerOnePlaying;
                break;
            case 2:
                playerReady = ChoosePlayerScript.Instance.player2Ready;
                playerTagged = DataManagerScript.playerTwoPlaying;
                break;
            case 3:
                playerReady = ChoosePlayerScript.Instance.player3Ready;
                playerTagged = DataManagerScript.playerThreePlaying;
                break;
            case 4:
                playerReady = ChoosePlayerScript.Instance.player4Ready;
                playerTagged = DataManagerScript.playerFourPlaying;
                break;
        }

        // Joystick movements
        checkHorizontalAxis();

        // Select slot
        if (player.GetButtonDown("Jump")) {
            if (enabled && !slotSelected && !playerTagged) {
                selectSlotForJoystick();
                HideIcon();
            }
        }

        // Unselect slot
        if (player.GetButtonDown("Grav")) {
        	if (!slotSelected) {
        		ToggleIcon(false);
                if (botMode)
                {
                    ChoosePlayerScript.Instance.ShowNonTaggedInPlayers();
                }
        	} else if (!playerReady) {
                ToggleIcon(true);
                unselectSlotForJoystick();
            }
        }
    }

    public void HideIcon()
    {
        if (icon) icon.SetActive(false);
        if (playerText) prompt.SetActive(false);
        if (prompt) playerText.SetActive(false);
        if (outline) outline.SetActive(false);
    }

    public void ShowIcon()
    {
        if (icon) icon.SetActive(true);
        if (prompt) prompt.SetActive(true);
        if (outline) outline.SetActive(true);
    }

    public void ToggleIcon(bool turnOn)
    {
        enabled = turnOn;

        if (icon)
        {
            Image image = icon.GetComponent<Image>();
            icon.SetActive(true);
            image.color = turnOn ? white : gray;
        }
        if (playerText) prompt.SetActive(!turnOn);
        if (prompt) playerText.SetActive(turnOn);

        // TODO: show outline if inactive and game ready
        if (outline) outline.SetActive(false);

         //If bot mode, don't start at predetermined slot, just find the next available of the two.
        if (botMode && enabled && !slotSelected)
        {
            //iterateSlot(true);
            if (DataManagerScript.playerOnePlaying)
            {
                slot = 2;
                Debug.Log("Choosing slot 2");
            } else if (DataManagerScript.playerTwoPlaying)
            {
                slot = 1;
                Debug.Log("Choosing slot 1");
            } else
            {
                slot = 1;
                Debug.Log("Default. Choosing slot 1");
            }

        }
        moveIcon(false);
    }

    private void FixedUpdate()
    {
        if (shouldActivate)
        {
            activateFakePlayer();
        }
    }

    void selectSlotForJoystick() {
       //  Set joystick for player slot
        switch (slot)
        {
            // TODO: With Rewired, should we use player IDs instead? (index 0 instead of 1)
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

        // Tell fake player it has a joystick now, and activate
        selectedSlotPlayer = GameObject.Find("Fake Player " + slot);
        shouldActivate = true;

        // Gamepad has been assigned
        slotSelected = true;

		// Kick any other gamepads out of this slot
		transform.parent.BroadcastMessage("leaveOccupiedSlot", slot);
    }

	void leaveOccupiedSlot(int slotToLeave){
		if (!playerTagged && !playerReady && !slotSelected && slot == slotToLeave) {
            // Get new slot
            if (!botMode)
            {
                iterateSlot(true);
            } else
            {
                if (slotToLeave == 1)
                {
                    slot = 2;
                } else if(slotToLeave == 2){
                    slot = 1;
                }
            }
            Debug.Log("I'm in this slot");
            Debug.Log(joystick);
			// Move the icon
			moveIcon (true);
		}
	}

    void activateFakePlayer() {
        selectedSlotPlayer.GetComponent<FakePlayerScript>().checkForJoystick();
        selectedSlotPlayer.GetComponent<FakePlayerScript>().activateReadyState();
        shouldActivate = false;
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

    void checkHorizontalAxis()
    {
      

        // Left or right pressed
        if (player.GetAxisRaw("MoveX") > 0 || player.GetAxisRaw("MoveY") < 0)
        {

            // Only proceed if slot is not already selected and joystick not already pressed
            if (axisInUse == false && !slotSelected )
            {

                // Boolean to prevent scrolling more than one tick per press
                axisInUse = true;

                // See if going right or left
                bool goingRight = player.GetAxisRaw("MoveX") > 0;

                // Get new slot
                iterateSlot(goingRight);

                // Move the icon
                moveIcon(true);
            }

        }
        else if (player.GetAxisRaw("MoveX") == 0)
        {
            // Reset boolean to prevent scrolling more than one tick per press when joystick returns to 0
            axisInUse = false;
        }
    }

    // Function for looping over availible slots and selecting next open slot
    void iterateSlot(bool goingRight)
    {
        // Move up or down through joystick ints
        int difference = (goingRight) ? 1 : -1;

        // Look for next slot based on input direction

        int numberOfSlots = botMode ? 2 : 4;
        int nextSlot = (slot + difference) % numberOfSlots;
        if (nextSlot == 0) nextSlot = numberOfSlots;
        Debug.Log(nextSlot);
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
        if (!enabled) {
            gameObject.transform.position = new Vector3(0f, gameObject.transform.position.y, 0f);
        }
        else {

            // Get selected slot coordinates
            selectedSlotPlayer = GameObject.Find("Fake Player " + slot);
            float slotX = selectedSlotPlayer.transform.position.x;

            // Move icon
            float y = gameObject.transform.position.y;
            gameObject.transform.position = new Vector3(slotX, y, 0f);

            if (playSound)
            {
                // TODO: Play sound effect
                // AudioClip tick = (goingUp) ? tickUp : tickDown;
                // audio.PlayOneShot(tick);
            }
        }
    }
}