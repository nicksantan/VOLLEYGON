using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LifecycleManager : MonoBehaviour {

	//
	// Init
	//

	void Start () {

#if UNITY_XBOXONE
		// Initial Activation
		XboxOnePLM.OnActivationEvent += HandleActivation;

		// Contrained
		XboxOnePLM.OnResourceAvailabilityChangedEvent += HandleResourceAvailabilityChange;

		// Suspended
		XboxOnePLM.OnSuspendingEvent += HandleSuspension;

		// Resumed
		XboxOnePLM.OnResumingEvent += HandleResume;
#endif

	}

	//
	// Lifecycle methods
	//

	void HandleActivation(ActivatedEventArgs args) {

		// See if the app was simply (re)activated or launched from scratch
		if (args.Kind == XboxOneActivationKind.Protocol) {

			// The app was activated and may or way not have already been running
			ProtocolActivatedEventArgs pargs = (ProtocolActivatedEventArgs)args;
			// TODO: Do we need to do anything when the app is re-activated?

		}
		if (args.Kind == XboxOneActivationKind.Launch) {

			// The app was launched from either a Terminated or NotRunning state
			LaunchActivatedEventArgs largs = (LaunchActivatedEventArgs)args;
			// TODO: do we care if the app was terminated? Do we show a message about the game data being lost but stats saved?

		}
	}

	void HandleResourceAvailabilityChange(bool isConstrained) {

		// Pause or unpause depending on contraint status
		PauseOrUnpause(isConstrained);

	}

	void HandleSuspension() {

		// Save game state here, or do it across multiple frame if required.
		// +!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!
		// YOU MUST CALL THIS BEFORE WE TIME OUT.
		// This tells Unity that you are done all work
		// and it is now safe to suspend the application
		//
		// You have less than 1 second to call this method
		// +!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!+!

		// Pause or unpause depending on contraint status
		PauseOrUnpause(true);

		// Ready to suspend
		XboxOnePLM.AmReadyToSuspendNow();

	}

	void HandleResume(double span) {

		// Reinitialize network connections, etc etc.
		// Unity passes you back the wallclock time span that you were suspended for
		// You should tailor your actions to the amount of time that they application
		// was suspended. IE: do you bring the user back into furious action when they have
		// been suspended for a week? Probably not, perhaps you bring them back to a pause
		// screen or something like it.
		// TimeSpan timeSuspended = TimeSpan.FromSeconds(span);
		// Debug.Log("Away from app for " + timeSuspended);

		// Only unpause if not in game
		PauseOrUnpause(false);

	}

	//
	// Events for pausing and unpausing, or handing that to Game
	//

	void PauseOrUnpause(bool shouldPause) {

		// See if we're in a game and pause if so
		if (GameManagerScript.Instance != null) {

			// Hand pause reponsibility to game manager
			// Never unpause game, leave pause screen open if game in progress
			if (shouldPause) {
				JoystickButtons buttons = new JoystickButtons(DataManagerScript.gamepadControllingMenus);
				GameManagerScript.Instance.Pause(buttons);
			}

		} else {

			// If not in game, pause/unpause time and musix manually
			if (shouldPause) {
				if (MusicManagerScript.Instance != null) MusicManagerScript.Instance.TurnOffEverything ();
				if (SoundManagerScript.instance != null) SoundManagerScript.instance.muteSFX ();
				Time.timeScale = 0;
			} else {
				if (MusicManagerScript.Instance != null) MusicManagerScript.Instance.RestoreFromPause ();
				if (SoundManagerScript.instance != null) SoundManagerScript.instance.unMuteSFX ();
				Time.timeScale = 1;
			}
		}
	}
}
