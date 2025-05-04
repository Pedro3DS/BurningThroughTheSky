using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource source;
    public static AudioController instance = null;
    void Awake()
    {
        if(!instance){
            instance = this;
        }else{
            Destroy(instance);
        }
    }

    public void PlayAudio(AudioClip audio){
        source.clip = audio;
        source.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
