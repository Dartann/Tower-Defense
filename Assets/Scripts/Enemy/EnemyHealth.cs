using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static Action<int> Event_OnEnemyDie;

    EnemyData enemyData;

    private int health;
    private int maxHealth;


    void Start(){
        enemyData = GetComponent<EnemyData>();
        maxHealth = enemyData.Enemy_DataSO.health;
        health = maxHealth;
    }

    public void TakeDamage(int damage){

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log($"Mevcut can {health}");
        CheckHealth();
    }

    void CheckHealth(){

        if(health == 0)
        {
            Event_OnEnemyDie.Invoke(enemyData.Enemy_DataSO.moneyReward);
            Destroy(gameObject);
        }
    }

}
