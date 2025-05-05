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
        if(PlayerPrefs.HasKey("Points")){
            int currentValue = PlayerPrefs.GetInt("Points");
            PlayerPrefs.SetInt("Points", currentValue += value);
        }else{
            PlayerPrefs.SetInt("Points", value);
        }
        onGetPoint?.Invoke();
        
    }
    public int GetPoints(){
        if(!PlayerPrefs.HasKey("Points")) return 0;
        return PlayerPrefs.GetInt("Points");
    }
}
