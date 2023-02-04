using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translation1 = Input.GetAxis("Vertical1") * Time.deltaTime * 20;
        transform.Translate(0, 0, translation1);
        float translation2 = Input.GetAxis("Horizontal1") * Time.deltaTime * 20;
        transform.Translate(translation2, 0, 0);
    }
}
