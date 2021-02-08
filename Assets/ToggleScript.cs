using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{

    public Toggle toggle;
    public bool toggleStatus = false;
    public Text labelText;
    // Start is called before the first frame update
    void Start()
    {
        //Load initial values from datamanager.
        toggleStatus = DataManagerScript.vibrationOn == 1 ? true : false;
        toggle.isOn = toggleStatus;
        labelText.text = toggleStatus ? "ON" : "OFF";

    }

  
    public void Toggle()
    {
        toggleStatus = !toggleStatus;
        toggle.isOn = toggleStatus;
        labelText.text = toggleStatus ? "ON" : "OFF";

        // Save Togglestatus. TODO: This should report to a filesystem manager to make sure the variables are saved to disk.
        DataManagerScript.vibrationOn = toggleStatus ? 1 : 0;
        PlayerPrefs.SetInt("vibrationOn", DataManagerScript.vibrationOn);
    }
}
