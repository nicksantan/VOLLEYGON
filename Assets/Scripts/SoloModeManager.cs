using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoloModeManager : MonoBehaviour {

    private GameObject ballPrefab;
    private int deadBalls;
    private int returnedBalls;
    public Text returnCountText;
    public int goalScore = 10;
    private bool challengeOver = false;
    public String challengeTitle;
    public bool canDie = true;
    public float baseTimeBetweenGravChanges = 5f;
    public float gravTimeRange = 4f;

    private bool challengeStarted = false;

    void Awake(){
        
    }
        
    void Start () {
        deadBalls = 0;
        // TODO: Once we're done modularizing, we won't need a unique ball prefab
        ballPrefab = ChallengeManagerScript.Instance.GetComponent<ChallengeManagerScript>().ballPrefab;
        ChallengeManagerScript.Instance.UpdateChallengeText(challengeTitle);
    }
    
    // Update is called once per frame
    void Update () {
        
        if (!challengeStarted){
            // check for challenge start
            if (ChallengeManagerScript.Instance.challengeRunning){
                challengeStarted = true;
                LaunchBall(0f,0f,0f);
            }
        }

        //if (challengeStarted){

        //    if (deadBalls >= 3 && !challengeOver)
        //    {
        //        ChallengeManagerScript.Instance.ChallengeFail();
        //    }

        //    if (returnedBalls == goalScore && !challengeOver)
        //    {
        //        ChallengeManagerScript.Instance.ChallengeSucceed();
        //        challengeOver = true;
        //    }
        //}
    }

    public void OnBallReturned()
    {
        Debug.Log("Ball returned");
        returnedBalls += 1;
        // Update UI here.
        returnCountText.text = returnedBalls.ToString();

        if (returnedBalls == goalScore && !challengeOver)
        {
            ChallengeManagerScript.Instance.ChallengeSucceed();
            challengeOver = true;
        }
    }

    void BallDied(int whichSide){
        Debug.Log ("the ball has died");
        // Only count dead balls if flag is set
        if (canDie)
        {
            deadBalls += 1;
        }
        //TODO: Perhaps a flag here to allow for continuous counting?
        returnedBalls = 0;
        returnCountText.text = returnedBalls.ToString();

        // Launch a replacement ball
        Debug.Log("is this challenge running?");
        Debug.Log(ChallengeManagerScript.Instance.challengeRunning);
        if (ChallengeManagerScript.Instance.challengeRunning && deadBalls < 3)
        {
            LaunchBall(0f, 0f, 0f);
        } else if (ChallengeManagerScript.Instance.challengeRunning)
        {
            ChallengeManagerScript.Instance.ChallengeFail();
        }
    }

    public void LaunchBall(float x, float y, float z){
        GameObject ball_1 = Instantiate(ballPrefab, new Vector3(x, y, z), Quaternion.identity);
        ball_1.transform.parent = gameObject.transform.parent;
        IEnumerator coroutine_1 = ball_1.GetComponent<BallScript> ().LaunchBallWithDelay (2f);
        StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        Debug.Log("setting gravchange mode to true");
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
        ball_1.GetComponent<BallScript>().baseTimeBetweenGravChanges = baseTimeBetweenGravChanges;
        ball_1.GetComponent<BallScript>().gravTimeRange = gravTimeRange;
        ball_1.GetComponent<BallScript>().startWithRandomGrav = true;
    }



    public IEnumerator LaunchBalls(float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            LaunchBall(0f,0f,0f);

            yield return new WaitForSeconds(interval);
        }
    }
}

