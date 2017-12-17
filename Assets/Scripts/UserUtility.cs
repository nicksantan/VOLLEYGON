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

	void OnUsersChanged(int id,bool wasAdded)
	{
		Debug.Log("> Users List Changed ["+id.ToString()+"] ["+(wasAdded?"Added":"Removed")+"]\n");
	}

	void OnUserSignIn(int id)
	{
		Debug.Log("> User Signed In: [" + id.ToString() + "]\n");
	}

	void OnUserSignInComplete(int id, int otherId)
	{
		Debug.Log("> User Finished Signing In: [" + id.ToString() + " | " + otherId.ToString() + "]\n");
		if (DataManagerScript.shouldActivateMenu == true) {
			GameObject title = GameObject.Find("TitleManager");
			if (title != null) {
				TitleManagerScript titleScript = title.GetComponent<TitleManagerScript>();
				titleScript.activateMainMenu(id);
			}
			DataManagerScript.shouldActivateMenu = false;
		}
	}

	void OnUserSignOut(int id)
	{
		Debug.Log("> User Signed Out: [" + id.ToString() + "]\n");
	}

	void OnUserSignOutStarted(int id, IntPtr deferred)
	{
		Debug.Log("> User Signed Out Started: [" + id.ToString() + "]\n");
		deferral = new SignOutDeferral(deferred);
	}

	void OnUserDisplayInfoChange(int id)
	{
		Debug.Log("> User Display Info Changed: [" + id.ToString() + "]\n");
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
}
