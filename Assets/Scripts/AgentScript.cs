using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;



public class AgentScript : Agent
{

    //Grab Game Controller object
    private AreaManagement manager;

    //Basic movements
    public float speed = 1f;
    public float turnSpeed = 30f;
    private new Rigidbody rigidbody;

    //Track # infected 
    public int atRiskCount = 0;

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AreaManagement>();
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        //Keep track of own positions
          sensor.AddObservation(this.transform.position.x);
          sensor.AddObservation(this.transform.position.z);
        //Keep track of # of at risk
          sensor.AddObservation(atRiskCount);

    }


    public override void OnEpisodeBegin()
    {
        //Reset things when episode begins
        transform.position = new Vector3(0,0.5f,0);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        manager.ResetMap();

     
    }



    private void OnCollisionEnter(Collision collision)
    {
        // If harrassing solo person, punish
        if (collision.gameObject.tag == "Wanderer")
        {
            Mover target = collision.gameObject.GetComponent<Mover>();
            target.Separate();

            AddReward(-.5f);


        }

        // If breaking up a group, reward
        else if (collision.gameObject.tag == "Talker")
        {
            AddReward(1f);
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        // generate new direction vectors
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        rigidbody.AddForce(dirToGo*speed, ForceMode.VelocityChange);
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        // When new decision is made, punish if too many steps
        AddReward(-1f / MaxStep);

        // Punish more if too many grouped together
        if (atRiskCount>4)
        {
            SetReward(-.1f);
            

        }
        MoveAgent(actionBuffers.DiscreteActions);
    }







    //Update number of people grouping together on the board
    public void updateRiskCount(int _count)
    {
        atRiskCount = _count;
    }


}
