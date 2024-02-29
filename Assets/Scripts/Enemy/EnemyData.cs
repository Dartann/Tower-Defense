using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] private EnemyDataSO m_EnemyDataSO;

    public int GetHealth() => m_EnemyDataSO.health;
    public int GetSpeed() => m_EnemyDataSO.speed;

}

