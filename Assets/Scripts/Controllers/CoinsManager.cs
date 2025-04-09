using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance = null;

    void Awake()
    {
        if(!Instance){
            Instance = this;
        }else{
            Destroy(Instance);
        }
    }
    public void SetCoins(int value){
        if(PlayerPrefs.HasKey("Coins")){
            int currentCoins = PlayerPrefs.GetInt("Coins");
            PlayerPrefs.SetInt("Coins", value += currentCoins);

        }else{
            PlayerPrefs.SetInt("Coins", value);

        }
    }
    public int GetCoins(){
        if(!PlayerPrefs.HasKey("Coins")) return 0;


        return PlayerPrefs.GetInt("Coins");
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Coins");
    }
}
