using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
//using Unity.MLAgents.Actuators;

public class AgentScript : Agent
{
    Rigidbody2D rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        //// If the Agent fell, zero its momentum
        //if (this.transform.localPosition.y < 0)
        //{
        //    this.rBody.angularVelocity = Vector3.zero;
        //    this.rBody.velocity = Vector3.zero;
        //    this.transform.localPosition = new Vector3(0, 0.5f, 0);
        //}

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);
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
    }
}


