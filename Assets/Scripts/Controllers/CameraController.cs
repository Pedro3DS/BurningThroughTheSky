using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    private Animator _anim;
    // Start is called before the first frame update
    public static CameraController instance = null;
    void Awake()
    {
        if(!instance){
            instance = this;
        }else{
            Destroy(instance);
        }
    }
    void Start()
    {
        _anim = cam.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObjectDestroyed(){
        _anim.SetTrigger("DestroyObject");
    }
}
