using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControllersData", menuName = "ScriptableObjects/ControllersData", order = 1)]
public class ControllersData : ScriptableObject
{
    [Header("Controller Index")]
    public int controllerIndex = 0;
    
}
