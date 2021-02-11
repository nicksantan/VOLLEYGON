using UnityEngine;
using System.Collections;
using Steamworks;
using System.Collections.Generic;
using UnityEngine.UI;

public class AchievementManagerScript : MonoBehaviour
{
    private enum Platform { Native, Steam, Switch, Xbox };
    private Platform currentPlatform;

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
        currentPlatform = Platform.Native;

        // load achievement status from playerprefs or other source (Steam cloud save?)

        // Are we on steam?
        if (currentPlatform == Platform.Steam)
        {
            if (SteamManager.Initialized)
            {
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

    public void QueueToPop(int whichAchievement)
    {
        // create a new achievement popup
        GameObject newPopup = Instantiate(popupPrefab, a_canvas.transform);
        newPopup.GetComponent<AchivementPopupScript>().whichAchievement = whichAchievement;
        AchievementsToPop.Add(newPopup);
    }

    public void SaveAchievementUnlock(int achievementID)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
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

    //List<List<float>> medalRanges = new List<List<float>>();


    // List of List<int>

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

