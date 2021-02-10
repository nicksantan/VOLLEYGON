using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class FileSystemLayer : MonoBehaviour
{
    private enum Platform { Native, Steam, Switch, Xbox };
    private Platform currentPlatform;

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

    }
    
    public void LoadData(string key)
    {
        switch (currentPlatform)
            {
            case Platform.Native:
                   
            break;
            }
    }

    public void SaveData(string key, int val)
    {
        switch (currentPlatform)
        {
            case Platform.Native:

                break;
        }
    }


}
