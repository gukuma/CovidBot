using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 20f;


    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    //couldn't be simpler
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rigidbody.MovePosition(transform.position + (transform.right * h + transform.forward * v) * speed);
    }

    //Tries to separate when bumping into individuals
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Talker")
        {
            Mover target = collision.gameObject.GetComponent<Mover>();
            target.Separate();


            

        }

    }

}
