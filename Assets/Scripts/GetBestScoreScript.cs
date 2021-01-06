using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetBestScoreScript : MonoBehaviour
{

    public float highScore;
    public float bronzeScore;
    public float silverScore;
    public float goldScore;
    public string scoreKey;

    public enum medalTypes
    {
        none,
        bronze,
        silver,
        gold
    }

    public medalTypes whichMedal = medalTypes.none;

    public Sprite bronzeMedalImage;
    public Sprite silverMedalImage;
    public Sprite goldMedalImage;

    void Start()
    {
        // Get the high score, if it exists.
        highScore = PlayerPrefs.GetFloat(scoreKey, 9999f);

        // Get the text component connected to this object and update the string to the high score, if there is one.
        if (highScore == 9999f)
        {
            gameObject.GetComponent<Text>().text = "BEST TIME: NONE";
        }
        else
        {
            gameObject.GetComponent<Text>().text = "BEST TIME: " + FormatTime(highScore);
        }

        // apply medal image if applicable
        if (highScore < bronzeScore)
        {
            whichMedal = medalTypes.bronze;
        }
        if (highScore < silverScore)
        {
            whichMedal = medalTypes.silver;
        }
        if (highScore < goldScore)
        {
            whichMedal = medalTypes.gold;
        }

        // If a medal container exists, apply the medal image

        //First, Find the Parent Object 
        Transform parent = transform.parent;

        //Now, Find it's MedalContainer Object
        Image medalImage = parent.Find("MedalContainer").gameObject.GetComponent<Image>();

        switch (whichMedal)
        {
            case medalTypes.none:
                break;
            case medalTypes.bronze:
                medalImage.sprite = bronzeMedalImage;
                break;
            case medalTypes.silver:
                medalImage.sprite = silverMedalImage;
                break;
            case medalTypes.gold:
                medalImage.sprite = goldMedalImage;
                break;

        }
    }

    public string FormatTime(float rawTimer)
    {
        int minutes = Mathf.FloorToInt(rawTimer / 60F);
        int seconds = Mathf.FloorToInt(rawTimer - minutes * 60);
        float fraction = (rawTimer * 100) % 100;
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
        return niceTime;
    }

}
