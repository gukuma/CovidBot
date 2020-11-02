using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public float distance=20f;


    void Start()
    {
        distance =Mathf.Max(Mathf.Abs( this.transform.position.x), Mathf.Abs(this.transform.position.z));

    }


    private void OnTriggerEnter(Collider other)
    {
        other.transform.position += transform.up * distance*1.8f;
    }
}
