using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
//using Unity.MLAgents.Actuators;

public class AgentScript : Agent
{
    Rigidbody2D rBody;
    public GameObject playerBeingControlled;


    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody2D>();
        //playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = -1;
        //startingPosition = transform.position;
    }

    public void FixedUpdate()
    {
        RequestDecision();
    }
    public Transform Target;

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            Debug.Log("trying to jump");
            Jump(true);
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 0)
        {
            Debug.Log("trying to stop jump");
            Jump(false);
        }

        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            GravChange(true);
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 0)
        {
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


        // Move the target to a new spot
        Target.localPosition = new Vector3(-4f,
                                           0f,
                                           0f);

        /// move ai back to a new spot

        gameObject.transform.localPosition = new Vector3(9.76f, -8.42f, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);

        // target velocity
        sensor.AddObservation(Target.GetComponent<Rigidbody2D>().velocity.x);
        sensor.AddObservation(Target.GetComponent<Rigidbody2D>().velocity.y);

        // target (ball) grav scale
        sensor.AddObservation(Target.GetComponent<BallScript>().gravScale);
        sensor.AddObservation(Target.GetComponent<BallScript>().timer);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        actionsOut[2] = 0;
        actionsOut[3] = 0;

        if (Input.GetKey("down"))
        {
            actionsOut[0] = 1;
        }

        if (Input.GetKeyDown("up"))
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

    public void OnBallDied()
    {
        AddReward(-0.5f);
        EndEpisode();
    }
}


