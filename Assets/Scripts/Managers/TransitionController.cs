using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
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
        // Player.onPlayerDie += DieTransition;
    }
    void DieTransition(){
        GameObject newCanvas = deathCanvas;
        // newCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        Instantiate(newCanvas);
    }
}
