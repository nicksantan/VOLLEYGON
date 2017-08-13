using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivateFirstButtonScript : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	// Use this for initialization
	void Start () {
		eventSystem.SetSelectedGameObject (selectedObject);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
