using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class JoystickLayerManager : MonoBehaviour
{
    private enum JoystickProvider { Rewired };
    private JoystickProvider currentJoystickProvider;

    // Rumble flags
    private bool smallRumbleOn = false;
    private float smallRumbleTimer = 0;

    // Static singleton property
    public static JoystickLayerManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // For now, hardcode to Rewired.
        currentJoystickProvider = JoystickProvider.Rewired;
    }

   
    void Start()
    {
        
    }

    void Update()
    {
        if (smallRumbleOn)
        {
            smallRumbleTimer += Time.deltaTime;

            if (smallRumbleTimer > .2)
            {
             //   FireSmallPulse();
                smallRumbleTimer = 0f;
            }
        }
    }

    //EventSystem / Menu methods
    public void AssignPlayerToEventSystem(int playerIndex)
    {
        switch (currentJoystickProvider)
        {
            case JoystickProvider.Rewired:
                var rsim = EventSystem.current.GetComponent<Rewired.Integration.UnityUI.RewiredStandaloneInputModule>();
                rsim.RewiredPlayerIds = new int[] { playerIndex };
                break;
        }
    }


    // Rumble methods
    public void ActivateLargeRumble(int playerIndex, float duration) // Note: playerIndex goes from 0 through 3.
    {
        if (FileSystemLayer.Instance.vibrationOn == 1)
        {
            switch (currentJoystickProvider)
            {
                case JoystickProvider.Rewired:
                    int motorIndex = 0; // the first motor
                    float motorLevel = 1.0f; // full motor speed
                    Player currentPlayer = ReInput.players.GetPlayer(playerIndex);
                    currentPlayer.SetVibration(motorIndex, motorLevel, duration);
                    break;

            }
        }
    }

    public void SmallRumblePulse(int playerIndex, float strength) // Note: this goes from 0 through 3.
    {
        Debug.Log("is rumble on?>");
        Debug.Log(FileSystemLayer.Instance.vibrationOn);
        if (FileSystemLayer.Instance.vibrationOn == 1)
        {
            switch (currentJoystickProvider)
            {
                case JoystickProvider.Rewired:
                    int motorIndex = 0; // the first motor
                    float motorLevel = strength; // full motor speed
                    Player currentPlayer = ReInput.players.GetPlayer(playerIndex);
                    currentPlayer.SetVibration(motorIndex, motorLevel, .15f);
                    break;

            }
        }
    }

    public void BeginSmallRumble(int playerIndex)
    {
        if (FileSystemLayer.Instance.vibrationOn == 1)
        {
            smallRumbleOn = true;
            Debug.Log(playerIndex);
            switch (currentJoystickProvider)
            {
                case JoystickProvider.Rewired:
                    Debug.Log("Setting tiny rumble");
                    int motorIndex = 1; // the first motor
                    float motorLevel = 0.1f; // low motor speed
                    Player currentPlayer = ReInput.players.GetPlayer(playerIndex);
                    currentPlayer.SetVibration(motorIndex, motorLevel, true);
                    break;
            }
        }
    }

    public void StopRumble(int playerIndex)
    {
        if (FileSystemLayer.Instance.vibrationOn == 1)
        {
            smallRumbleOn = false;
            switch (currentJoystickProvider)
            {
                case JoystickProvider.Rewired:
                    Player currentPlayer = ReInput.players.GetPlayer(playerIndex);
                    currentPlayer.StopVibration();
                    break;
            }
        }
    }
}
