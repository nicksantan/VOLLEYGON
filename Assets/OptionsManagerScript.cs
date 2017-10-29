using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManagerScript : MonoBehaviour {

	private int whichPlayerIsControlling;
	private JoystickButtons joyButts;
	// Use this for initialization
	void Start () {
		
		// determine which controller is 'in control'. For now, let's just say player one
		whichPlayerIsControlling = 1;
		joyButts = new JoystickButtons (1);
		//TODO: Will need to handle controller disconnects, etc.
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (joyButts.grav)) {
			Application.LoadLevel ("titleScene");
		}

		// Check for cancel button to go to previous scene
				
	}
}
