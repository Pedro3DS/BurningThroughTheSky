using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectName : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] textsFields;

    private string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZÇ0123456789-_|/*";

    private bool _p1Selected = false, _p2Selected = false;
    private float _verticalMovement,_horizontalMovement;
     // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_p1Selected){
            _verticalMovement = ControllersManager.Instance.VerticalMovement(0);
            _horizontalMovement = ControllersManager.Instance.HorizontalMovement(0);
        }else if(!_p2Selected){

            _verticalMovement = ControllersManager.Instance.VerticalMovement(1);
            _horizontalMovement = ControllersManager.Instance.HorizontalMovement(1);
        }
    }
}
