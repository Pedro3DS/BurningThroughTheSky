using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip explosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutoDestroy(){
        AudioController.instance.PlayAudio(explosionAudio);
        Destroy(gameObject);
    }
}
