using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public float howMuchBlood = 1f;
    public List<AudioSource> eatSounds;
    public List<GameObject> particleEffects;
    public List<GameObject> soundEffects;
    public float score = 1f;

    public float speedAdjust = 0.1f;

    private bool triggered = false;
    private bool playerFound = false;

    private GameObject player;
    
    void Start()
    {
    //    transform.localScale = Vector3.one * howMuchBlood;
    }

    public void Update(){
        if(triggered){
            transform.localScale = new Vector3(transform.localScale.x+0.002f,transform.localScale.y+0.002f,transform.localScale.z+0.002f);
        }
    }


    void OnTriggerEnter(Collider thing)
    {
        if(thing.transform.tag == "MovingRoot1"){
            player = GameObject.FindGameObjectWithTag("Player1");
            playerFound=true;
        }
        
        if(thing.transform.tag == "MovingRoot2"){
            player = GameObject.FindGameObjectWithTag("Player2");
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

        PlayerController tempController = player.GetComponent("PlayerController") as PlayerController;
        tempController.speed += speedAdjust;
        tempController.AddScore(score);


        Invoke("SelfDestruct", 1.1f);
        
    }




    void SelfDestruct(){
            //Create particle effect
        if(particleEffects.Count>0){
            int particleChoice = Random.Range(0, particleEffects.Count);
            GameObject particleSystem = Instantiate(particleEffects[particleChoice], transform.position, Quaternion.identity);
            particleSystem.GetComponent<ParticleSystem>().Play();
        }



        if(soundEffects.Count>0){
            int soundChoice = Random.Range(0, soundEffects.Count);
            Instantiate(soundEffects[soundChoice], transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
