using UnityEngine;
using System.Collections;
using Steamworks;
using System.Collections.Generic;
using UnityEngine.UI;

public class AchievementManagerScript : MonoBehaviour
{
    public enum Platform { Native, Steam, Switch, Xbox, Itch };
    [SerializeField]
    public Platform currentPlatform;

    [SerializeField]
    public List<Achievement> Achievements = new List<Achievement>();
    public List<GameObject> AchievementsToPop = new List<GameObject>();
    public int numberOfAchievements = 12;
    public Canvas a_canvas;
    public int totalReturns = 0;
    public List<Sprite> icons = new List<Sprite>();
    // Static singleton property
    public static AchievementManagerScript Instance { get; private set; }
    public string[] AchievementNames = { "First Achievement", "Second Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement" };
    public string[] AchievementDescriptions = { "First Achievement", "Second Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement", "Another Achievement" };
    public GameObject popupPrefab;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        //currentPlatform = Platform.Native;

        // load achievement status from playerprefs or other source (Steam cloud save?)
        Debug.Log("Achievement manager init");
        // Are we on steam?
        if (currentPlatform == Platform.Steam)
        {
            if (SteamManager.Initialized)
            {
                Debug.Log(SteamManager.Initialized);
                string steamname = SteamFriends.GetPersonaName();
                Debug.Log(steamname);
            }
            else
            {
                Debug.Log("Not on steam");
            }
        }

        PopulateAchievements();
    }

    private void Update()
    {
        PopNextAchievement();
    }

    public void PopulateAchievements()
    {
        switch (currentPlatform)
        {
            case Platform.Native:
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
                    }
                    else
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
                    }
                    else
                    {
                        PlayerPrefs.SetInt("a-" + i + "-progress", 0);
                    }

                    Achievements.Add(new Achievement(AchievementNames[i], AchievementDescriptions[i], thisAchievementUnlocked == 1, thisAchievementUsesProgress == 1, i, thisAchievementProgress));
                }
                break;
            case Platform.Steam:
                // TODO: Need to figure out how achievements with progress work. Perhaps use a logprogress method and poll for achievement complete?
                // read from steam achievements and populate local array
              
                for (int i = 0; i < numberOfAchievements; i++)
                {
                    bool thisAchievementUnlocked = false;
                    // NOTE: Currently unused.
                    int thisAchievementUsesProgress = 0;
                    int thisAchievementProgress = 0;

                    Steamworks.SteamUserStats.GetAchievement("ach_"+ (i+1).ToString(), out thisAchievementUnlocked);
                   
                    Achievements.Add(new Achievement(AchievementNames[i], AchievementDescriptions[i], thisAchievementUnlocked, thisAchievementUsesProgress == 1, i, thisAchievementProgress));
                }
                break;
        }
      
    }

    public void LogReturn()
    {
        switch (currentPlatform) {
            case Platform.Native:
            case Platform.Itch:
                this.totalReturns += 1;
                PlayerPrefs.SetInt("totalReturns", this.totalReturns);

                if (this.totalReturns >= 100)
                {
                    this.Achievements[1].Unlock();
                }
                break;
            case Platform.Steam:
                int currentTotalReturns;
                Steamworks.SteamUserStats.GetStat("stat_total_returns", out currentTotalReturns);
                currentTotalReturns++;
                Steamworks.SteamUserStats.SetStat("stat_total_returns", currentTotalReturns);
                Steamworks.SteamUserStats.StoreStats();
                // Poll for native achievement pop?
                if (currentTotalReturns >= 100)
                {
                    this.Achievements[1].Unlock();
                }
                break;
        }
    }

    public void LogMedalProgress(int totalMedals, int goldMedals)
    {
        // This is for platforms that support displaying achievement 'progress'. Right now, that's only Steam.
        switch (currentPlatform)
        {
            case Platform.Steam:
                Steamworks.SteamUserStats.SetStat("stat_total_medals", totalMedals);
                Steamworks.SteamUserStats.SetStat("stat_total_gold_medals", goldMedals);
                Steamworks.SteamUserStats.StoreStats();
                break;
        }
    }

    public void QueueToPop(int whichAchievement)
    {
        // create a new achievement popup 
        switch (currentPlatform)
        {
            default:
                    GameObject newPopup = Instantiate(popupPrefab, a_canvas.transform);
                    newPopup.GetComponent<AchivementPopupScript>().whichAchievement = whichAchievement;
                    AchievementsToPop.Add(newPopup);
                break;
        }
    }

    public void SaveAchievementUnlock(int achievementID)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                PlayerPrefs.SetInt("a-" + achievementID.ToString() + "-unlocked", 1);
                break;
            case Platform.Steam:
                // Need to look up achievement name here by index
                bool achievementUnlocked;
                Steamworks.SteamUserStats.GetAchievement("ach_" + (achievementID+1).ToString(), out achievementUnlocked);
                if (achievementUnlocked == false)
                {
                    Steamworks.SteamUserStats.SetAchievement("ach_" + (achievementID+1).ToString());
                }
                break;
            case Platform.Itch:
                PlayerPrefs.SetInt("a-" + achievementID.ToString() + "-unlocked", 1);
                break;

        }
    }

    public void PopNextAchievement()
    {
        // make sure there are achievements in the list
        if (AchievementsToPop.Count == 0) { return; }
        // get the first in a list
        GameObject nextInLine = AchievementsToPop[0];
        if (!nextInLine.GetComponent<AchivementPopupScript>().isMoving && !nextInLine.GetComponent<AchivementPopupScript>().isDone)
        {
            nextInLine.GetComponent<AchivementPopupScript>().BeginPop();
        } else if (!nextInLine.GetComponent<AchivementPopupScript>().isMoving && nextInLine.GetComponent<AchivementPopupScript>().isDone)
        {
            // remove this finished achievement from the list.
            AchievementsToPop.RemoveAt(0);
        }
    }
}

[System.Serializable]
public class Achievement
{
    public string name;
    public string description;
    public bool unlocked;
    public bool usesProgress;
    public int progress;
    public int id;

    public Achievement(string achievementName, string achievementDescription, bool achievementUnlocked, bool usesProgress, int id, int progress = 0)
    {
        this.name = achievementName;
        this.description = achievementDescription;
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

           // Save all status
           AchievementManagerScript.Instance.SaveAchievementUnlock(this.id);

            // Queue the achievement to be popped
            AchievementManagerScript.Instance.QueueToPop(this.id);
        }
        else
        {
            Debug.Log("Achievement already unlocked!");
        }
    }
}

