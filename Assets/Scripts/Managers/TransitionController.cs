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
        StartCoroutine(InstantiateTransition(transition));
    }
    IEnumerator InstantiateTransition(GameObject trans){
        // float animTime = trans.GetComponent<Animator>().playbackTime;
        Instantiate(trans);
        yield return new WaitForSeconds(1f);
        AsyncSceneController.Instance.ChangeScene("Game");
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
