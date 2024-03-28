using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Enemy/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Enemy Stats")]
    public string enemyName;
    public int health;
    public int speed;
    [Header("Enemy Rewards")]
    public int moneyReward;

}


