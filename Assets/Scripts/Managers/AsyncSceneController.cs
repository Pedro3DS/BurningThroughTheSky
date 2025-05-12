using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneController : MonoBehaviour
{
    public GameObject loadingWindow;
    public static AsyncSceneController Instance = null;

    void Awake()
    {
        if(!Instance){
            Instance = this;
        }else{
            Destroy(Instance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeScene(string scene){
        SceneManager.LoadScene(scene);
        Instantiate(loadingWindow);
        StartCoroutine(LoadSceneAsync(scene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadSceneAsync(string scene){
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene);
        while(!loadOperation.isDone){
            yield return null;
        }
    }
}