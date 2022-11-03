using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayCurrentPlayerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        int whichPlayerIsControlling = DataManagerScript.gamepadControllingMenus;
        Debug.Log("which player is controlling " + whichPlayerIsControlling.ToString());
        switch (whichPlayerIsControlling)
        {
            case 0:
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 1:
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 2:
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 3:
                gameObject.transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    public void ChangeActivePlayer(int newPlayer)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        switch (newPlayer)
        {
            case 0:
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 1:
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 2:
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 3:
                gameObject.transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }
}
