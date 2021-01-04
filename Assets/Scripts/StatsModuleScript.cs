using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsModuleScript : MonoBehaviour
{

    public int lastTouch;
    public int secondToLastTouch;

    private void Awake()
    {
        lastTouch = 0;
        secondToLastTouch = 0;
    }

    void Start()
    {
        
    }

    public void OnBallReturned(int lastTouch)
    {
        DataManagerScript.currentRallyCount += 1;
        if (DataManagerScript.currentRallyCount > DataManagerScript.longestRallyCount)
        {
            DataManagerScript.longestRallyCount = DataManagerScript.currentRallyCount;
            // Debug.Log ("longest rally count is now " + DataManagerScript.longestRallyCount);
        }

        // Credit a return to the last touch player
        switch (lastTouch)
        {
            case 1:
                DataManagerScript.playerOneReturns += 1;
                break;
            case 2:
                DataManagerScript.playerTwoReturns += 1;
                break;
            case 3:
                DataManagerScript.playerThreeReturns += 1;
                break;
            case 4:
                DataManagerScript.playerFourReturns += 1;
                break;
        }
    }

    // Listen for ball died to compute stats
    public void OnBallDied(int whichSide){
        Debug.Log("on ball died fired");
        if (whichSide == 1)
        {
            ComputeStat(2);
        } else if (whichSide == 2)
        {
            ComputeStat(1);
        }
    }

    public void ComputeStat(int whichTeamScored)
    {
        if (whichTeamScored == 1)
        {
            if (lastTouch == 1)
            {
                DataManagerScript.playerOneAces += 1;
                DataManagerScript.playerOneScores += 1;
            }
            if (lastTouch == 2)
            {
                DataManagerScript.playerTwoAces += 1;
                DataManagerScript.playerTwoScores += 1;
            }

            if (lastTouch == 3)
            {
                if (secondToLastTouch == 1)
                {
                    DataManagerScript.playerOneScores += 1;
                }
                if (secondToLastTouch == 2)
                {
                    DataManagerScript.playerTwoScores += 1;
                }
                if (secondToLastTouch == 3)
                {
                    DataManagerScript.playerThreeBumbles += 1;
                }
                if (secondToLastTouch == 4)
                {
                    DataManagerScript.playerThreeBumbles += 1;
                }
            }
            if (lastTouch == 4)
            {
                if (secondToLastTouch == 1)
                {
                    DataManagerScript.playerOneScores += 1;
                }
                if (secondToLastTouch == 2)
                {
                    DataManagerScript.playerTwoScores += 1;
                }
                if (secondToLastTouch == 3)
                {
                    DataManagerScript.playerFourBumbles += 1;
                }
                if (secondToLastTouch == 4)
                {
                    DataManagerScript.playerFourBumbles += 1;
                }
            }
        }
        if (whichTeamScored == 2)
        {
            if (lastTouch == 3)
            {
                DataManagerScript.playerThreeAces += 1;
                DataManagerScript.playerThreeScores += 1;
            }
            if (lastTouch == 4)
            {
                DataManagerScript.playerFourAces += 1;
                DataManagerScript.playerFourScores += 1;
            }

            if (lastTouch == 1)
            {
                if (secondToLastTouch == 1)
                {
                    DataManagerScript.playerOneBumbles += 1;
                }
                if (secondToLastTouch == 2)
                {
                    DataManagerScript.playerOneBumbles += 1;
                }
                if (secondToLastTouch == 3)
                {
                    DataManagerScript.playerThreeScores += 1;
                }
                if (secondToLastTouch == 4)
                {
                    DataManagerScript.playerFourScores += 1;
                }
            }
            if (lastTouch == 2)
            {
                if (secondToLastTouch == 1)
                {
                    DataManagerScript.playerTwoBumbles += 1;
                }
                if (secondToLastTouch == 2)
                {
                    DataManagerScript.playerTwoBumbles += 1;
                }
                if (secondToLastTouch == 3)
                {
                    DataManagerScript.playerThreeScores += 1;
                }
                if (secondToLastTouch == 4)
                {
                    DataManagerScript.playerFourScores += 1;
                }
            }
        }
    }
}
