using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_Text[] nameText;
    [SerializeField]
    private TMP_Text[] scoreText;
    [SerializeField]
    private TMP_Text[] deathsText;
    [SerializeField]
    private int scores;

    void Start()
    {
        var top = ScoreManager.Instance.GetTopScores(scores);
        for(int i = 0; i<=scores; i++){
            nameText[i].text = $"{top[i].player1Name}"; 
            scoreText[i].text = $"{top[i].score}"; 
            deathsText[i].text = $"{top[i].deaths}"; 
            // scoreText[i].text = top5[i].playerName; 
        }


    }

    // Update is called once per frame
    void Update()
    {
        // var activeGamepad = ControllersManager.Instance.GetActiveGamepad();
        if(Input.GetKeyDown(KeyCode.JoystickButton7)){
            TransitionController.Instance.LoadStartTransition();
        }   
    }
}
