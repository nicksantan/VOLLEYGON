using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementStatusIconScript : MonoBehaviour
{
    public int achievementID;
    public bool status = false;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public Image BG;
    public Image borderFrame;

    // Start is called before the first frame update
    void Start()
    {
        // Look up the appropriate icon for this achievement
        activeSprite = AchievementManagerScript.Instance.icons[achievementID];
        // Assign it to bg.
        BG.GetComponent<Image>().sprite = activeSprite;
        // Check achievement manager for this achievement's status
        status = AchievementManagerScript.Instance.Achievements[achievementID].unlocked;


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
        
    }
}
