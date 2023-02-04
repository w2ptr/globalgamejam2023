using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brench : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject movingRootPrefab;
    public GameObject lastRoot;
    public Transform trailTarget;

    private Transform nextSpawn;
    private GameObject tempSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void growBranch()
    {
        tempSpawn = Instantiate(rootPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);
        Destroy(lastRoot);
        nextSpawn = tempSpawn.transform.Find("nextTarget");
        lastRoot = Instantiate(movingRootPrefab, nextSpawn.transform.position, nextSpawn.transform.rotation, transform);

        trailTarget.position = nextSpawn.transform.position;
    }
}
