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

    public float speed = 1;
    public float speedCounterStart = 25;
    private float speedCounter;

    public List<AudioSource> spawnSounds;

    // Start is called before the first frame update
    void Start()
    {
        speedCounter = speedCounterStart;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         speedCounter-=1;
         if(speedCounter == 0 ){
            speedCounter=speedCounterStart/speed;
             createRoot();
         }
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

            if(spawnSounds.Count>0)
                spawnSounds[Random.Range(0, spawnSounds.Count)].Play();

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
        GameObject tempObj = Instantiate(branchPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);

        
    }
}
