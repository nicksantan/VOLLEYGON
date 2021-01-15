using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Challenge_Script_3 : MonoBehaviour
{

    private GameObject ballPrefab;
    private int deadBalls;
    private int basketsScored = 0;
    public int basketsToWin = 3;

    public String challengeTitle;

    private bool challengeStarted = false;
    private bool challengeOver = false;

    public bool startWithRandomGrav = false;

    private float startTime = 0;

    void Awake()
    {

    }

    void Start()
    {
        deadBalls = 0;
        // TODO: Once we're done modularizing, we won't need a unique ball prefab
        ballPrefab = ChallengeManagerScript.Instance.GetComponent<ChallengeManagerScript>().ballPrefab;
        ChallengeManagerScript.Instance.UpdateChallengeText(challengeTitle);
    }

    // Update is called once per frame
    void Update()
    {

        if (!challengeStarted)
        {
            // check for challenge start
            if (ChallengeManagerScript.Instance.challengeRunning)
            {
                challengeStarted = true;
                LaunchBall(0f, 0f, 0f);
                startTime = Time.time;
            }
        }

        if (challengeStarted)
        {
            if (!challengeOver)
            {
                //Check for victory
                if (basketsScored == basketsToWin)
                {
                    ChallengeManagerScript.Instance.ChallengeSucceed();
                    challengeOver = true;
                }
            }
        }
    }

    void OnBasketScored()
    {
        basketsScored += 1;
        Debug.Log("basket scored");
        if (AchievementManagerScript.Instance != null)
        {
            float endTime = Time.time;
            if (endTime - startTime <= 5f)
            {
                AchievementManagerScript.Instance.Achievements[10].Unlock();
            }
        }
    }

    void BallDied(int whichSide)
    {
        Debug.Log("the ball has died");
        deadBalls += 1;

        // Launch a replacement ball if game is running
        if (!challengeOver)
        {
            LaunchBall(0f, 0f, 0f);
        }
    }

    public void LaunchBall(float x, float y, float z)
    {
        GameObject ball_1 = Instantiate(ballPrefab, new Vector3(x, y, z), Quaternion.identity);
        ball_1.transform.parent = gameObject.transform.parent;
        int gravScale = 1;
        if (startWithRandomGrav)
        {
            gravScale = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        }
        IEnumerator coroutine_1 = ball_1.GetComponent<BallScript>().CustomLaunchBallWithDelay(2f, -6f, 10f*gravScale);
        StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        Debug.Log("setting gravchange mode");
        ball_1.GetComponent<BallScript>().startWithRandomGrav = false;
        ball_1.GetComponent<BallScript>().gravScale = gravScale;
        ball_1.GetComponent<BallScript>().setAppropriateGravSprite(gravScale);
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
    }

}

