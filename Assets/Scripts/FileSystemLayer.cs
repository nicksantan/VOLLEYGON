using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class FileSystemLayer : MonoBehaviour
{
    private enum Platform { Native, Steam, Switch, Xbox };
    private Platform currentPlatform;


    // User preferences
    public int vibrationOn = 1;
    public int protipsOn = 1;


    // Static singleton property
    public static FileSystemLayer Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // For now, hardcode to Native Unity.
        currentPlatform = Platform.Native;
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
                vibrationOn = PlayerPrefs.HasKey("vibrationOn") ? PlayerPrefs.GetInt("vibrationOn") : 1;
                protipsOn = PlayerPrefs.HasKey("protipsOn") ? PlayerPrefs.GetInt("protipsOn") : 1;
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

    public void SaveTime(string key, int val)
    {
        switch (currentPlatform)
        {
            case Platform.Native:

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
        }
    }


}
