﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
//using Unity.MLAgents.Actuators;

public class AgentScript : Agent
{
    Rigidbody2D rBody;
    public GameObject playerBeingControlled;
    public GameObject ballPrefab;
    public GameObject ball;
    public Transform Target;
    private GameObject mpm;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody2D>();
        //playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = -1;
        //startingPosition = transform.position;
        mpm = GameObject.FindWithTag("MidpointMarker");
    }

    public void FixedUpdate()
    {
        if (Target == null)
        {
            if (GameObject.FindWithTag("Ball"))
            {
                Target = GameObject.FindWithTag("Ball").transform;
            }
        }
        //RequestDecision();
      
        AddReward(.001f);


    }
  

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
          //  Debug.Log("trying to jump");
            Jump(true);
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 0)
        {
           // Debug.Log("trying to stop jump");
            Jump(false);
        }

        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            Debug.Log("Setting grav change to true;");
            GravChange(true);
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 0)
        {
            Debug.Log("Setting grav change to false;");
            GravChange(false);
        }

        if (Mathf.FloorToInt(vectorAction[2]) == 1)
        {
            Move(true, -10f);
        }


        if (Mathf.FloorToInt(vectorAction[3]) == 1)
        {
            Move(true, 10f);
        }
        if (Mathf.FloorToInt(vectorAction[3]) == 0 && Mathf.FloorToInt(vectorAction[2]) == 0)
        {
            Move(false, 10f);
        }

    }

    public void Move(bool isPressed, float dir)
    {

        if (playerBeingControlled != null)
        {
            if (isPressed)
            {
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = dir;
            }
            else
            {
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = 0;
            }
        }
    }

    public void MoveRight(bool isPressed)
    {
        if (playerBeingControlled != null)
        {
            if (isPressed)
            {
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = 10f;
            }
            else
            {
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = 0;
            }
        }
    }

    public void GravChange(bool isPressed)
    {
        if (playerBeingControlled != null)
        {
            //Debug.Log("GRAV ON?");
            //Debug.Log(isPressed);
            Debug.Log("ACTUALLY setting to " + isPressed);
            playerBeingControlled.GetComponent<PlayerController>().virtualButtons.grav = isPressed;
        }
    }

    public void Jump(bool isPressed)
    {
        if (playerBeingControlled != null)
        {
            playerBeingControlled.GetComponent<PlayerController>().virtualButtons.jump = isPressed;
        }
    }

    public override void OnEpisodeBegin()
    {
      // Debug.Log("Episode BEGIN!");
        if (playerBeingControlled.GetComponent<PlayerController>().team == 2) {
      
        ball = Instantiate(ballPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            ball.GetComponent<BallScript>().startWithRandomGrav = true;
            // ball.GetComponent<BallScript>().LaunchBall();
            IEnumerator coroutine_1 = ball.GetComponent<BallScript>().CustomLaunchBallWithDelay(.1f, 15f * Random.Range(.5f, 1.5f), -5f * Random.Range(-1.5f, 1.5f));
            //IEnumerator coroutine_1 = ball.GetComponent<BallScript>().LaunchBallWithDelay(.1f);
            StartCoroutine(coroutine_1);
            Target = ball.transform;
        } else
        {
            if (GameObject.FindWithTag("Ball"))
            {
                Target = GameObject.FindWithTag("Ball").transform;
            }
        }
        //// Move the target to a new spot
        //ball.transform.localPosition = new Vector3(-4f,
                                           //0f,
                                           //0f);

        /// move ai back to a new spot
        float startingPos = 9.76f;
        if (playerBeingControlled.GetComponent<PlayerController>().team == 1)
        {
            startingPos = -9.76f;
        }
        gameObject.transform.localPosition = new Vector3(startingPos * Random.Range(.75f, 1.25f), Random.Range(-8.55f,8.55f), 0f);
        rBody.velocity = Vector3.zero;
        rBody.gravityScale = 1;
        if (rBody.gravityScale < 0)
        {
            gameObject.GetComponent<PlayerController>().innerShape.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<PlayerController>().innerShape.SetActive(false);
        }

        // launch the ball
       // Debug.Log("trying to launch ball");
        
      
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        if (Target != null)

        {
            sensor.AddObservation(Target.localPosition);
            //// target velocity
            sensor.AddObservation(Target.GetComponent<Rigidbody2D>().velocity.x);
            sensor.AddObservation(Target.GetComponent<Rigidbody2D>().velocity.y);
            //// target (ball) grav scale
            sensor.AddObservation(Target.GetComponent<BallScript>().gravScale);
            sensor.AddObservation(Target.GetComponent<BallScript>().timer);
        } else
        {
            sensor.AddObservation(new Vector3(0, 0, 0));
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        if (walls[0] != null)
        {
            sensor.AddObservation(walls[0].transform.localPosition);
        } else
        {
            sensor.AddObservation(new Vector3(0f,0f,0f));
        }

        if (walls[1] != null)
        {
            sensor.AddObservation(walls[1].transform.localPosition);
        }
        else
        {
            sensor.AddObservation(new Vector3(0f, 0f, 0f));
        }

        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);

        // Agent is jumping status
        sensor.AddObservation(playerBeingControlled.GetComponent<PlayerController>().isJumping);
        sensor.AddObservation(playerBeingControlled.GetComponent<PlayerController>().team);
        sensor.AddObservation(playerBeingControlled.GetComponent<PlayerController>().framesSinceLastGravChange);
        sensor.AddObservation(mpm.transform.position.x);

    }

    public override void Heuristic(float[] actionsOut)
    {
      //  Debug.Log("is this fucking happening?");
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        actionsOut[2] = 0;
        actionsOut[3] = 0;

        if (Input.GetKey("down"))
        {
            actionsOut[0] = 1;
        }

        if (Input.GetKey("up"))
        {
            actionsOut[1] = 1;
        }

        if (Input.GetKey("left"))
        {
            actionsOut[2] = 1;
            Debug.Log("left test");
        }

        if (Input.GetKey("right"))
        {
            actionsOut[3] = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "Ball")
        {
            AddReward(0.25f);
        }
    }

    public void OnBallDied(int whichSide)
    {
        int whichSideToCareAbout;
        int ownSide;
        if (playerBeingControlled.GetComponent<PlayerController>().team == 1)
        {
            whichSideToCareAbout = 2;
            ownSide = 1;
        } else
        {
            whichSideToCareAbout = 1;
            ownSide = 2;
        }
        if (whichSide == ownSide)
        {
          //  Debug.Log("Till the next episode...");
            AddReward(-0.5f);
        } else if (whichSide == whichSideToCareAbout){
            AddReward(1f);
        }
        EndEpisode();
    }
}


