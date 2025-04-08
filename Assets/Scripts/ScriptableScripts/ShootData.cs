using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootData", menuName = "ScriptableObjects/ShootData", order = 1)]
public class ShootData : ScriptableObject
{
    [Header("ShootData")]
    public int minDamage;
    public int maxDamage;
    public float cadence;
    public float maxChargeTime = 2f; // tempo máximo para carregar o tiro

    
}
