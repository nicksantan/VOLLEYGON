using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeButtonTextColorScript : MonoBehaviour, ISelectHandler, IDeselectHandler, IUpdateSelectedHandler
{
	public Text t;
	public AudioClip selectSound;
	
	public Color highlightColor = new Color(1f, 0.87f, 0.33f, 1f);
	public Color defaultColor = Color.white;
	
	public GameObject highlight;

	void Start()
	{
		highlight = GameObject.Find("Highlight");
	}

	public void ChangeColor(Color whichColor)
	{
		t.color = whichColor;
	}

    public void Unhighlight()
    {
        ChangeColor(defaultColor);
		if (highlight) {
			highlight.transform.position = new Vector3(-1000f, -1000f, 0);
		}
    }

    public void Highlight()
    {
        ChangeColor(highlightColor);
		if (highlight != null && highlight.transform.position.y != transform.position.y) {
			highlight.transform.position = transform.position;
			highlight.BroadcastMessage("IncrementShape");
		}
    }
		
	public void OnSelect(BaseEventData eventData){
		Highlight();
	}

	public void OnDeselect(BaseEventData eventData){
        SoundManagerScript.instance.PlaySingle(selectSound); // play on deselect so so don't play on first mount
		Unhighlight();
	}

	public void OnUpdateSelected(BaseEventData eventData) {
		if (eventData.selectedObject == gameObject) {
			Highlight();
		}
	}
}