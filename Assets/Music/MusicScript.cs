using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public List<AudioClip> audioFiles;
    private AudioSource audioSource;
    private float lastSoundTime;

    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioFiles.Count > 0)
        {
            //Play intro-ish sound
            PlaySound(audioFiles[Random.Range(0, audioFiles.Count)]);
        }
        
        lastSoundTime = Time.time;
    }

    void Update()
    {
        float timeSinceLastSound = Time.time - lastSoundTime;

        float howLongToWait = Random.Range(45.0f, 110.0f);

        if (timeSinceLastSound > howLongToWait)
        {
            if (audioFiles.Count > 0)
            {
                PlaySound(audioFiles[Random.Range(0, audioFiles.Count)]);
            }
            
            lastSoundTime = Time.time;
        }
    }
}
