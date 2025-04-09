using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private TMP_Text _cointText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player.onPlayerGetCoint += UpdateCoin;
    }
    void OnEnable()
    {
        // Player.onPlayerDie += EnableDeathCanvas;
    }
    void OnDisable()
    {
        // Player.onPlayerDie -= EnableDeathCanvas;
        
    }
    void EnableDeathCanvas(){
        deathCanvas.SetActive(true);
    }
    void UpdateCoin(){
        _cointText.text = $"{CoinsManager.Instance.GetCoins()} X";
    }
    
}
