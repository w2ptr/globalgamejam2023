using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestruct",3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfDestruct(){
        Destroy(gameObject);
    }
}
