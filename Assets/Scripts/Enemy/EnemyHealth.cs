using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health;
    private int maxHealth;

    void Start(){
        maxHealth = GetComponent<EnemyData>().GetHealth();
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
            Destroy(gameObject);
    }

}
