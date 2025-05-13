using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private GameObject transition;

    public static TransitionController Instance;

    void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(Instance);
    }

    public void LoadStartTransition(){
        StartCoroutine(InstantiateTransition(transition, "Game"));
    }
    public void LoadTransition(GameObject newTransition, string scene){
        StartCoroutine(InstantiateTransition(newTransition, scene));
    }
    IEnumerator InstantiateTransition(GameObject trans, string scene){
        // float animTime = trans.GetComponent<Animator>().playbackTime;
        Instantiate(trans);
        yield return new WaitForSeconds(1f);
        AsyncSceneController.Instance.ChangeScene(scene);
    }
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
    
}
