using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class FileSystemLayer : MonoBehaviour
{
    private enum Platform { Native, Steam, Switch, Xbox, Itch };
    private Platform currentPlatform;


    // User preferences
    public int vibrationOn = 1;
    public int protipsOn = 1;
    public float sfxVolume = 10f;
    public float masterVolume = 10f;
    public float musicVolume = 10f;

    // High scores and challenge best times
    public int soloRallyModeHighScore = 0;

    // Challenge best times
    int numberOfChallenges = 10;
    public List<float> bestTimes;

    // Various playstats
    public List<int> arenaPlays;
    public List<int> playerPlays;

    // Static singleton property
    public static FileSystemLayer Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // For now, hardcode to Native Unity.
        currentPlatform = Platform.Native;

        bestTimes = new List<float>();
        arenaPlays = new List<int>();
        playerPlays = new List<int>();
    }

    void Start()
    {
        LoadAllPrefs();
    }

    public void LoadAllPrefs()
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                // Prefs that use bools
                vibrationOn = PlayerPrefs.HasKey("vibrationOn") ? PlayerPrefs.GetInt("vibrationOn") : 1;
                protipsOn = PlayerPrefs.HasKey("protipsOn") ? PlayerPrefs.GetInt("protipsOn") : 1;

                // Prefs that use floats
                sfxVolume = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 10f;
                masterVolume = PlayerPrefs.HasKey("masterVolume") ? PlayerPrefs.GetFloat("masterVolume") : 10f;
                musicVolume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 10f;

                // High scores and best times that use ints
                soloRallyModeHighScore = PlayerPrefs.HasKey("soloRallyModeHighScore") ? PlayerPrefs.GetInt("soloRallyModeHighScore") : 0;

                // Load all challenge times
                for (int i = 1; i < numberOfChallenges+1; i++)
                {
                    float currentBestTime = PlayerPrefs.HasKey(i.ToString()) ? PlayerPrefs.GetFloat(i.ToString()) : 9999f;
                    bestTimes.Add(currentBestTime);
                }

                // Load various playstats
                for (int i = 0; i < 13; i++)
                {
                    int currentArenaPlays  = PlayerPrefs.HasKey("arena_"+i.ToString()+"_plays") ? PlayerPrefs.GetInt("arena_"+i.ToString()+"_plays") : 0;
                    arenaPlays.Add(currentArenaPlays);
                }

                for (int i = 0; i < 6; i++)
                {
                    int currentPlayerPlays  = PlayerPrefs.HasKey("player_"+i.ToString()+"_plays") ? PlayerPrefs.GetInt("player_"+i.ToString()+"_plays") : 0;
                    playerPlays.Add(currentPlayerPlays);
                }

            break;
            case Platform.Steam:
                // Prefs that use bools
                vibrationOn = FileBasedPrefs.HasKey("vibrationOn") ? FileBasedPrefs.GetInt("vibrationOn") : 1;
                protipsOn = FileBasedPrefs.HasKey("protipsOn") ? FileBasedPrefs.GetInt("protipsOn") : 1;

                // Prefs that use floats
                sfxVolume = FileBasedPrefs.HasKey("sfxVolume") ? FileBasedPrefs.GetFloat("sfxVolume") : 10f;
                masterVolume = FileBasedPrefs.HasKey("masterVolume") ? FileBasedPrefs.GetFloat("masterVolume") : 10f;
                musicVolume = FileBasedPrefs.HasKey("musicVolume") ? FileBasedPrefs.GetFloat("musicVolume") : 10f;

                // High scores and best times that use ints
                soloRallyModeHighScore = FileBasedPrefs.HasKey("soloRallyModeHighScore") ? FileBasedPrefs.GetInt("soloRallyModeHighScore") : 0;

                // Load all challenge times
                for (int i = 1; i < numberOfChallenges + 1; i++)
                {
                    float currentBestTime = FileBasedPrefs.HasKey(i.ToString()) ? FileBasedPrefs.GetFloat(i.ToString()) : 9999f;
                    bestTimes.Add(currentBestTime);
                }

                // Load various playstats
                for (int i = 0; i < 13; i++)
                {
                    int currentArenaPlays = FileBasedPrefs.HasKey("arena_" + i.ToString() + "_plays") ? FileBasedPrefs.GetInt("arena_" + i.ToString() + "_plays") : 0;
                    arenaPlays.Add(currentArenaPlays);
                }

                for (int i = 0; i < 6; i++)
                {
                    int currentPlayerPlays = FileBasedPrefs.HasKey("player_" + i.ToString() + "_plays") ? FileBasedPrefs.GetInt("player_" + i.ToString() + "_plays") : 0;
                    playerPlays.Add(currentPlayerPlays);
                }

                break;
            case Platform.Itch:
                // Prefs that use bools
                vibrationOn = FileBasedPrefs.HasKey("vibrationOn") ? FileBasedPrefs.GetInt("vibrationOn") : 1;
                protipsOn = FileBasedPrefs.HasKey("protipsOn") ? FileBasedPrefs.GetInt("protipsOn") : 1;

                // Prefs that use floats
                sfxVolume = FileBasedPrefs.HasKey("sfxVolume") ? FileBasedPrefs.GetFloat("sfxVolume") : 10f;
                masterVolume = FileBasedPrefs.HasKey("masterVolume") ? FileBasedPrefs.GetFloat("masterVolume") : 10f;
                musicVolume = FileBasedPrefs.HasKey("musicVolume") ? FileBasedPrefs.GetFloat("musicVolume") : 10f;

                // High scores and best times that use ints
                soloRallyModeHighScore = FileBasedPrefs.HasKey("soloRallyModeHighScore") ? FileBasedPrefs.GetInt("soloRallyModeHighScore") : 0;

                // Load all challenge times
                for (int i = 1; i < numberOfChallenges + 1; i++)
                {
                    float currentBestTime = FileBasedPrefs.HasKey(i.ToString()) ? FileBasedPrefs.GetFloat(i.ToString()) : 9999f;
                    bestTimes.Add(currentBestTime);
                }

                // Load various playstats
                for (int i = 0; i < 13; i++)
                {
                    int currentArenaPlays = FileBasedPrefs.HasKey("arena_" + i.ToString() + "_plays") ? FileBasedPrefs.GetInt("arena_" + i.ToString() + "_plays") : 0;
                    arenaPlays.Add(currentArenaPlays);
                }

                for (int i = 0; i < 6; i++)
                {
                    int currentPlayerPlays = FileBasedPrefs.HasKey("player_" + i.ToString() + "_plays") ? FileBasedPrefs.GetInt("player_" + i.ToString() + "_plays") : 0;
                    playerPlays.Add(currentPlayerPlays);
                }

                break;
        }
    }

    public void LoadData(string key)
    {
        switch (currentPlatform)
            {
            case Platform.Native:
                   
            break;
            }
    }

    public void SaveArenaPlay(int whichArena, int plays)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                arenaPlays[whichArena] = plays;
                PlayerPrefs.SetInt("arena_"+whichArena.ToString()+"_plays", plays);
                break;
            case Platform.Steam:
                Steamworks.SteamUserStats.SetStat("arena_" + whichArena.ToString() + "_plays", plays);
                Steamworks.SteamUserStats.StoreStats();
                break;
        }
    }

     public void SavePlayerPlay(int whichPlayer, int plays)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                playerPlays[whichPlayer] = plays;
                PlayerPrefs.SetInt("player_"+whichPlayer.ToString()+"_plays", plays);
                break;
            case Platform.Steam:
                Steamworks.SteamUserStats.SetStat("player_" + whichPlayer.ToString() + "_plays", plays);
                Steamworks.SteamUserStats.StoreStats();
                break;
        }
    }

    public void SaveChallengeTime(int whichChallenge, float time)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                bestTimes[whichChallenge - 1] = time;
                PlayerPrefs.SetFloat(whichChallenge.ToString(), time);
                break;
            case Platform.Steam:
                bestTimes[whichChallenge - 1] = time;
                FileBasedPrefs.SetFloat(whichChallenge.ToString(), time);
                break;
            case Platform.Itch:
                FileBasedPrefs.SetFloat(whichChallenge.ToString(), time);
                break;
        }
    }

    
    public float GetChallengeTime(int whichChallenge)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                return bestTimes[whichChallenge - 1];
                break;
            default:
                return bestTimes[whichChallenge - 1];
                break;
        }
    }

    public void SavePref(string key, int val)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                PlayerPrefs.SetInt(key, val);
                break;
            case Platform.Steam:
                FileBasedPrefs.SetInt(key, val);
                break;
            case Platform.Itch:
                FileBasedPrefs.SetInt(key, val);
                break;

        }
        
    }

    public void SaveFloatPref(string key, float val)
    {
        switch (currentPlatform)
        {
            case Platform.Native:
                PlayerPrefs.SetFloat(key, val);
                break;
            case Platform.Steam:
                FileBasedPrefs.SetFloat(key, val);
                break;
            case Platform.Itch:
                FileBasedPrefs.SetFloat(key, val);
                break;
        }
    }


}
