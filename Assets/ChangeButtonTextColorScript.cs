using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeButtonTextColorScript : MonoBehaviour, ISelectHandler, IDeselectHandler {

	public Text t;

	public void ChangeColor(Color whichColor)
	{
		t.color = whichColor;
	}
//

	public void OnDeselect(BaseEventData eventData){
		ChangeColor (Color.green);
		Debug.Log ("DISABLED!");
	}
	public void OnSelect(BaseEventData eventData){
		ChangeColor (Color.blue);
	}


}


