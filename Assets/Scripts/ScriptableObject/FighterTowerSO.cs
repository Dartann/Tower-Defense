using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FighterTowerSO", menuName = "Tower/TowerDataSO/FighterTower")]
public class FighterTowerSO : TowerDataSO
{
    [Header("Tower Stats")]
    public int Damage;
    public int Range;
    public float FireRate;
    public int BulletSpeed;
}
