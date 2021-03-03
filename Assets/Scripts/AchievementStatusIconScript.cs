using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementStatusIconScript : MonoBehaviour, IDeselectHandler
{
    public int achievementID;
    public bool status = false;
    public string achievementName;
    public string achievementDescription;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public Image BG;
    public Image borderFrame;
    private EventSystem es;
    public Text a_title;
    public Text a_desc;

	public AudioClip selectSound;

    // Start is called before the first frame update
    void Start()
    {
        // Look up the appropriate icon for this achievement
        activeSprite = AchievementManagerScript.Instance.icons[achievementID];
        // Assign it to bg.
        BG.GetComponent<Image>().sprite = activeSprite;
        // Check achievement manager for this achievement's status
        status = AchievementManagerScript.Instance.Achievements[achievementID].unlocked;
        achievementName = AchievementManagerScript.Instance.Achievements[achievementID].name;
        achievementDescription = AchievementManagerScript.Instance.Achievements[achievementID].description;


        // either show false or achievement icon depending on status
        if (status == true)
        {
            BG.GetComponent<Image>().color = new Color(1f,1f,1f);
            borderFrame.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        } else
        {
            BG.GetComponent<Image>().color = new Color(.30f, .3f, .30f);
            borderFrame.GetComponent<Image>().color = new Color(.3f, .3f, .3f);
        }


    }
    
    // Update is called once per frame
    void Update()
    {
        es = EventSystem.current;
        // Compare selected gameObject with referenced Button gameObject
        if (es.currentSelectedGameObject == gameObject)
        {
            // Update the relevant fields with this info.
            if (!a_title.gameObject.activeInHierarchy) { a_title.gameObject.SetActive(true); };
            if (!a_desc.gameObject.activeInHierarchy) { a_desc.gameObject.SetActive(true); };
            a_title.text = achievementName.ToUpper();
            a_desc.text = achievementDescription.ToUpper();

            borderFrame.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        } else
        {
            if (!status)
            {
                borderFrame.GetComponent<Image>().color = new Color(.3f, .3f, .3f);
            }
        }
    }

	public void OnDeselect(BaseEventData eventData){
		SoundManagerScript.instance.PlaySingle(selectSound);
	}
}
