using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject deathCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        // Player.onPlayerDie += EnableDeathCanvas;
    }
    void OnDisable()
    {
        // Player.onPlayerDie -= EnableDeathCanvas;
        
    }
    void EnableDeathCanvas(){
        deathCanvas.SetActive(true);
    }
    
}
