using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeButtonTextColorScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	public Text t;
	private AudioSource selectSound;

	public void ChangeColor(Color whichColor)
	{
		t.color = whichColor;
	}
//
    void Start()
    { 
		selectSound = GetComponent<AudioSource>();
    }

    public void ChangeToWhite()
    {
        ChangeColor(Color.white);
    }
	public void OnDeselect(BaseEventData eventData){
		if (selectSound) selectSound.Play(); // play on deselect to avoid initial selection
		ChangeColor (Color.white);
	}
	public void OnSelect(BaseEventData eventData){
		ChangeColor (Color.yellow);
	}
}
