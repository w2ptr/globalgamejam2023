using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    private bool playerFound = false;

    private GameObject player;

    private MovingBrench tempBrench;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider thing)
    {
        if(thing.transform.tag == "MovingRoot1"){
        
            tempBrench = thing.transform.parent.gameObject.GetComponent("MovingBrench") as MovingBrench;
            if(tempBrench!=null){
                playerFound=true;
            }
            
        }
        
        if(thing.transform.tag == "MovingRoot2"){
             tempBrench = thing.transform.parent.gameObject.GetComponent("MovingBrench") as MovingBrench;
            if(tempBrench!=null){
                playerFound=true;
            }
        }

        if(!playerFound)
            return;

        tempBrench.parentBrench.disabled = true;

    }
}
