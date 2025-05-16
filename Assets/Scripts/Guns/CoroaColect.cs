using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroaColect : MonoBehaviour
{
    [SerializeField] private GameObject transition;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tiger"))
        {
            GameManager.Instance.crownGeted = true;
            Instantiate(transition);
            // TransitionController.Instance.LoadTransition(transition, "Win")
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
