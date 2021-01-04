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
