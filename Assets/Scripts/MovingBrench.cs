using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBrench : MonoBehaviour
{
    public int targetCounter = 1;
    public int playerNr = 1;
    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player"+ playerNr+"Target" + targetCounter).transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }


}