using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public float howMuchBlood = 1.0f;
    public List<AudioSource> eatSounds;
    public List<GameObject> particleEffects;

    private bool triggered = false;
    private bool playerFound = false;
    
    void Start()
    {
        transform.localScale = Vector3.one * howMuchBlood;
    }


    void OnTriggerEnter(Collider thing)
    {
        if(thing.transform.tag == "MovingRoot1"||thing.transform.tag == "MovingRoot2"){
                playerFound=true;
        }

        if(!playerFound)
            return;

        if(!triggered){
            triggered=true;
        }else{
            return;
        }

        //Do sound



        if(eatSounds.Count>0)
             eatSounds[Random.Range(0, eatSounds.Count)].Play();

  

        //Create particle effect
        if(particleEffects.Count>0){
            int particleChoice = Random.Range(0, particleEffects.Count);
            GameObject particleSystem = Instantiate(particleEffects[particleChoice], transform.position, Quaternion.identity);
            particleSystem.GetComponent<ParticleSystem>().Play();
        }


        Invoke("SelfDestruct", 2.0f);
        
    }


    void SelfDestruct(){
        Destroy(gameObject);
    }
}
