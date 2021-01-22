using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPositionAdjustScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TimerCanvas;
    void Start()
    {
        // check if this is challenge number 5 or 8, and if so, move the position slightly
        GameObject challengeContainer = GameObject.Find("Challenges");
        GameObject firstActiveGameObject;
        int whichChallenge = 0;
        for (int i = 0; i < challengeContainer.transform.childCount; i++)
        {
            if (challengeContainer.transform.GetChild(i).gameObject.activeSelf == true)
            {
                firstActiveGameObject = challengeContainer.transform.GetChild(i).gameObject;
                whichChallenge = i;
            }
        }
        Debug.Log("which challenge?");
        Debug.Log(whichChallenge);
        if (whichChallenge == 4 || whichChallenge == 7 )
        {
            if (TimerCanvas)
            {
                TimerCanvas.GetComponent<RectTransform>().position = new Vector3(3f, 0f, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
