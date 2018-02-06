#if UNITY_XBOXONE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Users;

public class UserUtility : MonoBehaviour {
	SignOutDeferral deferral;

	void Start() {
		if (Application.platform == RuntimePlatform.XboxOne) {

			UsersManager.Create();

			UsersManager.OnUsersChanged          += OnUsersChanged;
			UsersManager.OnUserSignIn            += OnUserSignIn;
			UsersManager.OnSignInComplete        += OnUserSignInComplete;
			UsersManager.OnUserSignOut           += OnUserSignOut;
			UsersManager.OnSignOutStarted        += OnUserSignOutStarted;
			UsersManager.OnDisplayInfoChanged    += OnUserDisplayInfoChange;
			UsersManager.OnAppCurrentUserChanged += OnAppCurrentUserChange;

		}
	}

	void OnUsersChanged(int userId,bool wasAdded)
	{
		Debug.Log("> Users List Changed ["+userId.ToString()+"] ["+(wasAdded?"Added":"Removed")+"]\n");
	}

	void OnUserSignIn(int userId)
	{
		Debug.Log("> User Signed In: [" + userId.ToString() + "]\n");
	}

	void OnUserSignInComplete(int gamepad, int userId)
	{
		if (userId != -1) {
			Debug.Log("> User Finished Signing In: [" + gamepad.ToString() + " | " + userId.ToString() + "]\n");

			UpdateTitle(gamepad);
			UpdateLobby(userId);

		}
		else {
			Debug.Log("> No user selected, account picker cancelled.");
		}
	}

	void OnUserSignOut(int userId)
	{
		Debug.Log("> User Signed Out: [" + userId.ToString() + "]\n");
	}

	void OnUserSignOutStarted(int userId, IntPtr deferred)
	{
		Debug.Log("> User Signed Out Started: [" + userId.ToString() + "]\n");
		deferral = new SignOutDeferral(deferred);
	}

	void OnUserDisplayInfoChange(int userId)
	{
		Debug.Log("> User Display Info Changed: [" + userId.ToString() + "]\n");

		// Update lobby display info
		UpdateLobby(userId);
	}

	void OnAppCurrentUserChange()
	{
		User u = UsersManager.GetAppCurrentUser();
		string info = "no App Current User";
		if ( null != u )
		{
			info = u.Id + ": " + u.OnlineID;
		}
		Debug.Log("> App Current User Changed: [" + info + "]\n");
	}

	void UpdateTitle(int gamepad) {

		// Start main menu with picked user
		if (DataManagerScript.shouldActivateMenu == true) {
			GameObject title = GameObject.Find("TitleManager");
			if (title != null) {
				TitleManagerScript titleScript = title.GetComponent<TitleManagerScript>();
				titleScript.activateMainMenu(gamepad);
				DataManagerScript.shouldActivateMenu = false;
			}
		}

	}

	void UpdateLobby(int userId) {

		// Change slot on multiplayer lobby
		int slot = DataManagerScript.slotToUpdate;
		GameObject lobby = GameObject.Find("ChoosePlayerManager");

		if (slot != -1 && lobby != null) {
			ChoosePlayerScript lobbyScript = lobby.GetComponent<ChoosePlayerScript>();
			lobbyScript.showLoginPrompt(slot, userId);
			DataManagerScript.slotToUpdate = -1;
		}

	}
}

#endif