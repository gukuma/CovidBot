using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Unity.MLAgents

public class AreaManagement : MonoBehaviour
{
    public Camera cameraTransform;
    public GameObject wandererPrefab;
    
    public GameObject wallPrefab;
    public GameObject groundPrefab;

    [Range(3,10)]
    public float mapSize;

    private int wandererCount = 5;
    public List<GameObject> wandererList;
    public float clearedCounter = 0f;
    public int numAtRisk = 0;





    private void Start()
    {
        SetWandererDensity(.6f);
        ResetMap();
    }


    //reset map
    public void ResetMap()
    {
    //removes old map if there is one
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        SpawnMap(mapSize);
        SpawnWanderer(wandererCount);
    //Turn camera to map
        cameraTransform.transform.LookAt(this.transform);
        cameraTransform.orthographicSize= mapSize*1.5f;
    }








    private void SpawnMap(float _size)
    {
      //  GameObject agentObject = Instantiate<GameObject>(AgentPrefab.gameObject);
       // agentObject.transform.parent = this.transform;
       // agentObject.transform.position = new Vector3(0, .5f, 0);

        GameObject groundObject = Instantiate<GameObject>(groundPrefab.gameObject);
        groundObject.transform.parent = this.transform;
        groundObject.transform.position = new Vector3(0, 0, 0);
        groundObject.transform.localScale = new Vector3(mapSize/5, 1f, mapSize/5);

        for (int i = 0; i < 4; i++)
        {

            float x = 0;
            float z = 0;

            GameObject wallObject = Instantiate<GameObject>(wallPrefab.gameObject);
            wallObject.transform.parent = this.transform;

            x = Mathf.Cos(i* Mathf.PI/2);
            z = Mathf.Sin(i * Mathf.PI / 2);

            wallObject.transform.position = new Vector3(x*mapSize,0,z*mapSize);
            wallObject.transform.Rotate(-i * 90f,0, 0);
            wallObject.transform.localScale = new Vector3(mapSize, 0.1f, mapSize);


        }



    }

    private void SpawnWanderer(int count)
    {

        wandererList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            // Spawn and place the fish
            GameObject wandererObject = Instantiate<GameObject>(wandererPrefab.gameObject);
            float x = Random.Range(-mapSize,mapSize);
            float z = Random.Range(-mapSize, mapSize);

            wandererObject.transform.position = new Vector3(x, 0.5f,z) ;
            wandererObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            wandererObject.GetComponent<Mover>().SetWanderArea(mapSize);

            // Set the fish's parent to this area's transform
            wandererObject.transform.SetParent(transform);

            // Keep track of the fish
            wandererList.Add(wandererObject);

            // Set the fish speed
            wandererPrefab.GetComponent<Mover>().Wander();
        }
    }


    //At end of each update cycle, check how many are not social distancing
    private void DelayedUpdate()
    {
        numAtRisk= CheckAtRisk();

    }

    //Generate a random position that is on the map
    public static Vector3 ChooseRandomPosition(Vector3 center, float size)
    {

        float x = Random.Range(center.x - size, center.x + size);
        float z = Random.Range(center.z - size, center.z + size);

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        return new Vector3(x, .2f, z);
    }


    //Check how many on map are not social distancing
    public int CheckAtRisk()
    {
        int count = 0;
        foreach(GameObject wanderer in wandererList)
        {
            Mover mover = wanderer.GetComponent<Mover>();
            if (mover.talking)
            {
                count++;
            }

        }
        return count;
    }

    //Resize map during runtime
    public void ResizeMap(float _mapSize)
    {
        mapSize = _mapSize;
    }

    //Change wanderer count in map during runtime
    public void SetWandererDensity(float val)
    {
        wandererCount = (int)((mapSize * mapSize) * val);

    }

}
