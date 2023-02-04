using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public float howMuchBlood = 1.0f;
    public List<AudioSource> eatSounds;
    public List<GameObject> particleEffects;
    
    void Start()
    {
        transform.localScale = Vector3.one * howMuchBlood;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Do sound
        eatSounds[Random.Range(0, eatSounds.Count)].Play();

        //Create particle effect
        int particleChoice = Random.Range(0, particleEffects.Count);
        GameObject particleSystem = Instantiate(particleEffects[particleChoice], transform.position, Quaternion.identity);
        particleSystem.GetComponent<ParticleSystem>().Play();
    }
}
