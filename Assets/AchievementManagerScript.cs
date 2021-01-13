using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class AchievementManagerScript : MonoBehaviour
{
    [SerializeField]
    public List<Achievement> Achievements = new List<Achievement>();
    public int numberOfAchievements = 12;

    // Static singleton property
    public static AchievementManagerScript Instance { get; private set; }


    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {

        // load achievement status from playerprefs or other source (Steam cloud save?)
        for (int i = 0; i < numberOfAchievements; i++)
        {

            Achievements.Add(new Achievement("Sample Achievement", false));
        }
    }



}

[System.Serializable]
public class Achievement
{
    public string name;
    public bool unlocked;
    public bool usesProgress;
    public int progress;

    //List<List<float>> medalRanges = new List<List<float>>();


    // List of List<int>

    public Achievement(string achivementName, bool achievementUnlocked, bool usesProgress, int progress = 0)
    {
        this.name = achivementName;
        this.unlocked = achievementUnlocked;
        this.usesProgress = usesProgress;
        this.progress = progress;

    }

    public void Unlock()
    {
        // trigger any animations
        // call steam to save this if applicable
        // call playerpref to save it
        this.unlocked = true;

    }
}