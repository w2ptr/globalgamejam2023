using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRoot : MonoBehaviour
{
    public int playerNr = 0;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player" + playerNr + "TargetCenter").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
