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

    public bool disabled = false;

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
        if(disabled)
            return;

        tempSpawn = Instantiate(rootPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);
        Destroy(lastRoot);
        nextSpawn = tempSpawn.transform.Find("nextTarget");
        lastRoot = Instantiate(movingRootPrefab, nextSpawn.transform.position, nextSpawn.transform.rotation, transform);
        MovingBrench tempBrench = lastRoot.GetComponent("MovingBrench") as MovingBrench;
        tempBrench.parentBrench = this;

        trailTarget.position = nextSpawn.transform.position;
    }
}
