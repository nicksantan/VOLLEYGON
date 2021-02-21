using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class SoloManagerScript : MonoBehaviour
{
    private Player player;
    public GameObject winPanel;
    private EventSystem es;
    public GameObject PlayAgainButton;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(DataManagerScript.gamepadControllingMenus);
        es = EventSystem.current;
    }

    public void TurnOnMenu()
    {
        winPanel.SetActive(true);
        if (JoystickLayerManager.Instance != null)
        {
            JoystickLayerManager.Instance.AssignPlayerToEventSystem(DataManagerScript.gamepadControllingMenus);
            es.SetSelectedGameObject(PlayAgainButton);
        }
    }
}
