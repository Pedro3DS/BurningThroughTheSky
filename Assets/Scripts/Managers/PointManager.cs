using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    

    public static PointManager Instance = null;

    public delegate void OnGetPoint();
    public static OnGetPoint onGetPoint;
    // Start is called before the first frame update
    void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(Instance);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPoints(int value){
        if(PlayerPrefs.HasKey("CurrentPoints")){
            int currentValue = PlayerPrefs.GetInt("CurrentPoints");
            PlayerPrefs.SetInt("CurrentPoints", currentValue += value);
        }else{
            PlayerPrefs.SetInt("CurrentPoints", value);
        }
        onGetPoint?.Invoke();
        
    }
    public int GetPoints(){
        if(!PlayerPrefs.HasKey("CurrentPoints")) return 0;
        return PlayerPrefs.GetInt("CurrentPoints");
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("CurrentPoints");
    }
}
