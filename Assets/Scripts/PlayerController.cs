using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject movingRootPrefab;
    public GameObject lastRoot;
    public int playerNr = 1;
    public GameObject leftBrenchPrefab;
    public GameObject rightBrenchPrefab;



    private Transform nextSpawn;
    private GameObject tempSpawn;


    private int rootCounter = 0;

    private GameObject[] movingBrenches;

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

        rootCounter += 1;

        if (rootCounter == 6)
        {
            createBrenches();
        }

        if (rootCounter == 3)
        {
            movingBrenches = GameObject.FindGameObjectsWithTag("Player"+ playerNr+"Brench");

            for (int i = 0; i < movingBrenches.Length; i++)
            {
                Brench tempBrench = movingBrenches[i].GetComponent("Brench") as Brench;
                tempBrench.growBrench();
            }
        }

        if (rootCounter == 6)
        {
            rootCounter = 0;
        }



    }


    public void createBrenches()
    {
        Instantiate(leftBrenchPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);
        Instantiate(rightBrenchPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);
    }
}
