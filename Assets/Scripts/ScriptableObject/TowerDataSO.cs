using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataSO", menuName = "Tower/TowerDataSO")]
public class TowerDataSO : ScriptableObject
{
    [Header("Tower Stats")]
    public int damage;
    public int range;
    public float fireRate;
    public int bulletSpeed;
}
