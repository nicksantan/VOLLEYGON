using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveChallengeTimeScript : MonoBehaviour
{
    public int saveKey;
    public float challengeTime = 99999f;
    private GameObject bestTimeText;

    // Start is called before the first frame update
    void Awake()
    {
        LoadTime();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ChallengeResult CompareTimes(float newTime)
    {
        if (newTime < challengeTime)
        {
            // New best time, do something here. Return new time and flag on whether or not this is a new best time
            SaveTime(newTime);
            return new ChallengeResult(newTime, true);
        }
        else {
            return new ChallengeResult(challengeTime, false);
        }
    }

    void SaveTime(float newTime)
    {
        FileSystemLayer.Instance.SaveChallengeTime(saveKey, newTime);
    }

    void LoadTime()
    {
        challengeTime = FileSystemLayer.Instance.GetChallengeTime(saveKey);
        // make the default time really really high. I would prefer null but not sure how to do that right now.
        //challengeTime = PlayerPrefs.GetFloat(saveKey, 9999f);
    }
}

public class ChallengeResult
{
    public float challengeTime;
    public bool wasBestTime;

    public ChallengeResult(float challengeTime, bool wasBestTime)
    {
        this.challengeTime = challengeTime;
        this.wasBestTime = wasBestTime;
    }
}
