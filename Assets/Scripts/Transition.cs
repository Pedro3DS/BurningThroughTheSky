using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public delegate void OnTransitionEnd();
    public static event OnTransitionEnd onTransitionEnd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TransitionEnd(){
        onTransitionEnd?.Invoke();
        Transition.onTransitionEnd = null;
    }
}
