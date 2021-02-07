using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementPopupScript : MonoBehaviour
{
    public bool isMoving = false;
    public bool isDone = false;
    public int whichAchievement = 8; // placeholder
    public string achievementName;
    public Text achievementNameText;
    public Sprite achievementIcon;
    public Image achievementIconFrame;
    // Start is called before the first frame update
    void Start()
    {

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginPop()
    {
        // Get achievement info
        achievementName = AchievementManagerScript.Instance.Achievements[whichAchievement].name;
        achievementIcon = AchievementManagerScript.Instance.icons[whichAchievement];


        // populate fields
        achievementNameText.GetComponent<Text>().text = achievementName.ToUpper();
        achievementIconFrame.GetComponent<Image>().sprite = achievementIcon;

        isMoving = true;
        SlideIn();
    }
    void SlideIn()
    {
        float currentY = gameObject.GetComponent<RectTransform>().localPosition.y;
        Debug.Log("current Y is");
        Debug.Log(currentY);
        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), currentY + 200f, .250f).setEase(LeanTweenType.easeOutCubic).setOnComplete(DelayAndSlideOut);
    }

    void DelayAndSlideOut() {
        Invoke("SlideOut", 3f);
    }

    void SlideOut()
    {
        float currentY = gameObject.GetComponent<RectTransform>().localPosition.y;
        Debug.Log("current Y is");
        Debug.Log(currentY);
        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), currentY - 200f, .250f).setEase(LeanTweenType.easeInCubic).setOnComplete(EndPop);
    }
    void EndPop()
    {
        isMoving = false;
        isDone = true;
    }
}
