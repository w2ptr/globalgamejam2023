using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject movingRootPrefab;
    public GameObject lastRoot;
    public int playerNr = 0;
    public GameObject leftBrenchPrefab;
    public GameObject rightBrenchPrefab;
    public Transform trailTarget;

    private Transform nextSpawn;
    private GameObject tempSpawn;

    private int rootCounter = 0;

    private GameObject[] movingBranches;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.KeyDown)
        {
            switch (m_Event.keyCode)
            {
                case KeyCode.JoystickButton1:
                    Debug.Log("lol");
                    break;

                case KeyCode.Space:
                    createRoot();
                    break;
                default:

                    break;
            }
        }
    }

    public void createRoot()
    {
        tempSpawn = Instantiate(rootPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);
        Destroy(lastRoot);
        nextSpawn = tempSpawn.transform.Find("nextTarget");
        lastRoot = Instantiate(movingRootPrefab, nextSpawn.transform.position, nextSpawn.transform.rotation, transform);

        trailTarget.transform.position = lastRoot.transform.position;

        rootCounter += 1;

        if (rootCounter == 6) // split off
        {
            createBranch(leftBrenchPrefab);
            createBranch(rightBrenchPrefab);
        }

        if (rootCounter == 3) // build onto side branches
        {
            movingBranches = GameObject.FindGameObjectsWithTag("Player" + playerNr + "Brench");

            for (int i = 0; i < movingBranches.Length; i++)
            {
                Brench tempBranch = movingBranches[i].GetComponent<Brench>();
                tempBranch.growBranch();
            }
        }

        if (rootCounter == 6)
        {
            rootCounter = 0;
        }
    }

    public void createBranch(GameObject branchPrefab)
    {
        Instantiate(branchPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);
    }
}
