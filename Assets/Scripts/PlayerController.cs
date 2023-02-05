using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject movingRootPrefab;
    public GameObject lastRoot;
    public int playerNr = 0;
    public GameObject leftBrenchPrefab;
    public GameObject rightBrenchPrefab;
    public Transform trailTarget;

    public int brenchGrowthRate = 2;
    public int brenchSpawnRate = 12;

    private Transform nextSpawn;
    private GameObject tempSpawn;

    private int rootCounter = 0;
    private int rootCounter2 = 0;

    private GameObject[] movingBranches;

    public float speed = 1;
    public float speedCounterStart = 25;
    private float speedCounter;
    private List<(KeyCode, string, float, float)> inputs;

    public List<AudioSource> spawnSounds;

    public float score = 0;

    public TMP_Text textfield;
    public Transform tree;

    public GameEnder gameEnder;

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
        if (speedCounter <= 0)
        {
            speedCounter = speedCounterStart / speed;
            createRoot();
        }
    }

    public void AddScore(float input){
        score+=input;
        textfield.text = ""+score;

        if(playerNr==1){
            gameEnder.AddScore1();
        }else{
            gameEnder.AddScore2();
        }

        tree.localScale = new Vector3(tree.localScale.x*1.05f,tree.localScale.y*1.05f,tree.localScale.z*1.05f);
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
        rootCounter2 += 1;

        if (rootCounter >= brenchSpawnRate) // split off
        {
            createBranch(leftBrenchPrefab);
            createBranch(rightBrenchPrefab);
            rootCounter = 0;

                        if(Random.Range(0, 2)==0){
             if(spawnSounds.Count>0)
                spawnSounds[Random.Range(0, spawnSounds.Count)].Play();
            }
        }

        if (rootCounter2 >= brenchGrowthRate) // build onto side branches
        {
            movingBranches = GameObject.FindGameObjectsWithTag("Player" + playerNr + "Brench");           

            for (int i = 0; i < movingBranches.Length; i++)
            {
                Brench tempBranch = movingBranches[i].GetComponent<Brench>();
                tempBranch.growBranch();
            }

            rootCounter2 = 0;
        }

    }

    public void createBranch(GameObject branchPrefab)
    {
        GameObject tempObj = Instantiate(branchPrefab, lastRoot.transform.position, lastRoot.transform.rotation, transform);

        
    }
}
