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
    private List<(KeyCode, string, float, float)> inputs;

    public List<AudioSource> spawnSounds;

    // Start is called before the first frame update
    void Start()
    {
        speedCounter = speedCounterStart;

        var s = 20.0f;
        inputs = new List<(KeyCode, string, float, float)>() {
            (KeyCode.W, "Player1Target2", s, 0f),
            (KeyCode.A, "Player1Target2", 0f, -s),
            (KeyCode.S, "Player1Target2", -s, 0f),
            (KeyCode.D, "Player1Target2", 0f, s),
            (KeyCode.T, "Player1Target1", s, 0f),
            (KeyCode.F, "Player1Target1", 0f, -s),
            (KeyCode.G, "Player1Target1", -s, 0f),
            (KeyCode.H, "Player1Target1", 0f, s),
            (KeyCode.I, "Player2Target2", s, 0f),
            (KeyCode.J, "Player2Target2", 0f, -s),
            (KeyCode.K, "Player2Target2", -s, 0f),
            (KeyCode.L, "Player2Target2", 0f, s),
            (KeyCode.UpArrow, "Player2Target1", s, 0f),
            (KeyCode.LeftArrow, "Player2Target1", 0f, -s),
            (KeyCode.DownArrow, "Player2Target1", -s, 0f),
            (KeyCode.RightArrow, "Player2Target1", 0f, s),
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speedCounter -= 1;
        if (speedCounter == 0)
        {
            speedCounter = speedCounterStart / speed;
            createRoot();
        }
    }

    void OnGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyDown)
        {
            foreach (var (code, target, v, h) in inputs)
            {
                if (e.keyCode == code)
                {
                    var obj = GameObject.FindGameObjectWithTag(target);
                    var playerTarget = obj.GetComponent<PlayerTarget>();
                    playerTarget.Move(v, h);
                }
            }

            switch (e.keyCode)
            {
                case KeyCode.JoystickButton1:
                    Debug.Log("lol");
                    break;
                case KeyCode.Space:
                    Debug.Log("hi");
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
