using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover : MonoBehaviour
{
    public float maxSpeed=3f;
    public float speed;
    public float range = 5f;
    public bool talking = false;
    public float idleTimer = 5f;
    public float force = 4f;
    public float wanderLimit;
    public float RotationSpeed = 40f;
    public Renderer manRenderer;
    public Vector3 wayPoint;
    public Transform leader;

    private Rigidbody rb;
    private Vector3 separationVec;




    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wayPoint = this.transform.position;
    
        manRenderer.material.SetColor("_Color", Color.clear);

        StartMoving();
        Wander();
       
       
    }

    private void FixedUpdate()
    {
        ResetToField();
       // Debug.DrawRay(this.transform.position, separationVec, Color.red);

        Vector3 movePosition = transform.position;
        movePosition.x = Mathf.MoveTowards(transform.position.x, wayPoint.x, speed * Time.deltaTime);
        movePosition.z = Mathf.MoveTowards(transform.position.z, wayPoint.z, speed * Time.deltaTime);
        movePosition.y = 1;
        transform.LookAt(movePosition);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        rb.MovePosition(movePosition);


        if (!talking)
        {
            if ((transform.position - wayPoint).magnitude < 2f)
            {
                StartMoving();
                Wander();
            }
            else
            {
                if (idleTimer > 0)
                {
                    idleTimer -= Time.deltaTime;
                }else
                {
                    StartMoving();
                    Wander();
                }

            }
        }
    }


    public void ResetToField()
    {
        float absX = Mathf.Abs(this.transform.position.x);
        float absZ = Mathf.Abs(this.transform.position.z);
        if (absZ>wanderLimit &&absX >wanderLimit)
        {
             wayPoint.x = UnityEngine.Random.Range(-wanderLimit,wanderLimit);
            wayPoint.z = UnityEngine.Random.Range(-wanderLimit,wanderLimit);
            this.transform.position = wayPoint;
        }

    }





    //Chooses a new random position to walk to
    public void Wander()
    {
         wayPoint.y = 1;
         wayPoint.x = UnityEngine.Random.Range(this.transform.position.x-range, this.transform.position.x+range);
        wayPoint.z = UnityEngine.Random.Range(this.transform.position.z - range, this.transform.position.z+range);
    }


    //When colliding, both stop wandering, turn to each other, and need to be separated by agent/player
    private void OnCollisionStay(Collision collision)
    {
        int collisionCount = 0;
        separationVec = new Vector3();

        foreach (ContactPoint contact in collision.contacts)
        {
            collisionCount++;
            separationVec += contact.normal;
            separationVec = separationVec / collisionCount;
            separationVec = separationVec.normalized;

           
        }
       

        if (collision.gameObject.tag == "Wanderer" || collision.gameObject.tag == "Talker")
        {
            this.transform.LookAt(collision.transform);
            speed = .1f;

       /*    Originally used to determine which will be leader and which will be follower, so that the group can continue to wander together
        *    if (collision.gameObject.GetInstanceID() < this.gameObject.GetInstanceID())
            {
               // following = true;
                leader = collision.gameObject.transform;
            }
            else
            {
                //speed = .02f;

            }*/
            manRenderer.material.SetColor("_Color", Color.red);

            foreach (Transform t in transform)
            {
                t.gameObject.tag = "Talker";
            }
            gameObject.tag = "Talker";
            talking = true;
            this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Talking",talking);
            //speed = 0;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }
           
    }


    //When pushed part, either separated by agent/player or something else, stops talking and begins wandering again
    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Wanderer" || collision.gameObject.tag == "Talker")
        {
            foreach (Transform t in transform)
            {
                t.gameObject.tag = "Wanderer";
            }
            gameObject.tag = "Wanderer";
            talking = false;
            this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Talking", talking);

          //  if ( leader!=null){ leader = null; }
            manRenderer.material.SetColor("_Color", Color.clear);
        }
      

    }




    //Resets idle mode
    public void StartMoving()
    {
        speed = maxSpeed;
        idleTimer = 5f;
        leader = null;

    }

    //Adds a jolt in the direction opposite of the combined normalised vector of everthing it is touching
    public void Separate()
    {
        rb.MovePosition( this.transform.position+separationVec*2f);
        wayPoint = this.transform.position;
        
    }

    
    public void SetWanderArea(float size)
    {
        wanderLimit = size;


    }


}
