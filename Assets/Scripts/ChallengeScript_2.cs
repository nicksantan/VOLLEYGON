using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChallengeScript_2 : MonoBehaviour
{

    private GameObject ballPrefab;
    private int deadBalls;

    public String challengeTitle;

    private bool challengeStarted = false;
    private float startTime = 0f;

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
            //Check for victory
          
        }
    }

    void BallDied(int whichSide)
    {
        switch (whichSide)
        {
            case 1:
                ChallengeManagerScript.Instance.ChallengeFail();
                break;

            case 2:
                ChallengeManagerScript.Instance.ChallengeSucceed();
                if (AchievementManagerScript.Instance != null)
                {
                    float endTime = Time.time;
                    if (endTime - startTime <= 5f)
                    {
                        AchievementManagerScript.Instance.Achievements[10].Unlock();
                    }
                }
                break;
        }
    }

    public void LaunchBall(float x, float y, float z)
    {
        GameObject ball_1 = Instantiate(ballPrefab, new Vector3(x, y, z), Quaternion.identity);
        ball_1.transform.parent = gameObject.transform.parent;
        IEnumerator coroutine_1 = ball_1.GetComponent<BallScript>().CustomLaunchBallWithDelay(2f, -6f, 10f);
        StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
    }



    public IEnumerator LaunchBalls(float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            LaunchBall(0f, 0f, 0f);

            yield return new WaitForSeconds(interval);
        }
    }
}

