using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Se quiser manter entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // void OnEnable()
    // {
    //     Player.OnPlayerGetCoint += SetCoins;
    // }
    public void SetCoins(int value){
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        int newTotal = currentCoins + value;
        PlayerPrefs.SetInt("Coins", newTotal);
        PlayerPrefs.Save();
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
