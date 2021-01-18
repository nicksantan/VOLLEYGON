using UnityEngine;
using System.Collections;
using Steamworks;
using System.Collections.Generic;
using UnityEngine.UI;

public class AchievementManagerScript : MonoBehaviour
{
    [SerializeField]
    public List<Achievement> Achievements = new List<Achievement>();
    public int numberOfAchievements = 12;

    public int totalReturns = 0;
    public List<Sprite> icons = new List<Sprite>();
    // Static singleton property
    public static AchievementManagerScript Instance { get; private set; }
    public string[] AchievementNames = { "First Achievement", "Second Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement" };

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {

        // load achievement status from playerprefs or other source (Steam cloud save?)

        // Are we on steam?
        if (SteamManager.Initialized)
        {
            string steamname = SteamFriends.GetPersonaName();
            Debug.Log(steamname);
        } else {
            Debug.Log("Not on steam");
        }

        // Check for achievements that have 'progress'
        if (PlayerPrefs.HasKey("totalReturns"))
        {
            totalReturns = PlayerPrefs.GetInt("totalReturns");
        }
        else
        {
            PlayerPrefs.SetInt("totalReturns", 0);
        }

        // Populate local achievement status
        for (int i = 0; i < numberOfAchievements; i++)
        {

            int thisAchievementUnlocked = 0;
            int thisAchievementUsesProgress = 0;
            int thisAchievementProgress = 0;

            // Assuming local pc build for now
            if (PlayerPrefs.HasKey("a-" + i + "-unlocked"))
            {
                thisAchievementUnlocked = PlayerPrefs.GetInt("a-" + i + "-unlocked");
            } else
            {
                PlayerPrefs.SetInt("a-" + i + "-unlocked", 0);
            }

            if (PlayerPrefs.HasKey("a-" + i + "-usesProgress"))
            {
                thisAchievementUsesProgress = PlayerPrefs.GetInt("a-" + i + "-usesProgress");
            }
            else
            {
                PlayerPrefs.SetInt("a-" + i + "-usesProgress", 0);
            }

            if (PlayerPrefs.HasKey("a-" + i + "-progress"))
            {
                thisAchievementProgress = PlayerPrefs.GetInt("a-" + i + "-progress");
            } else
            {
                PlayerPrefs.SetInt("a-" + i + "-progress", 0);
            }

          


                Achievements.Add(new Achievement(AchievementNames[i], thisAchievementUnlocked == 1, thisAchievementUsesProgress == 1, i, thisAchievementProgress));
        }
    }

    public void SaveAchievements()
    {
        Debug.Log("Will save all achievements here");
    }

    public void LogReturn()
    {
        this.totalReturns += 1;
        PlayerPrefs.SetInt("totalReturns", this.totalReturns);

        if (this.totalReturns >= 100)
        {
            this.Achievements[1].Unlock();
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
    public int id;

    //List<List<float>> medalRanges = new List<List<float>>();


    // List of List<int>

    public Achievement(string achievementName, bool achievementUnlocked, bool usesProgress, int id, int progress = 0)
    {
        this.name = achievementName;
        this.unlocked = achievementUnlocked;
        this.usesProgress = usesProgress;
        this.progress = progress;
        this.id = id;

    }

    public void Unlock()
    {
        // trigger any animations
        // call steam to save this if applicable
        // call playerpref to save it

        if (!this.unlocked)
        {
            this.unlocked = true;

            // Switch per platform here maybe. For now, save playerprefs
            PlayerPrefs.SetInt("a-" + this.id + "-unlocked", 1);


            // Save all status
            AchievementManagerScript.Instance.SaveAchievements();
        } else
        {
            Debug.Log("Achievement already unlocked!");
        }

    }
}