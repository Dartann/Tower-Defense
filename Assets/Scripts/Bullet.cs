using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    int damage;
    int speed;
    public void Instantiate(int bulletDamage, int bulletSpeed)
    {
        damage = bulletDamage;
        speed = bulletSpeed;

        MakeBulletMove();
    } 
    private void MakeBulletMove() => rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
    private void Awake() => rb = GetComponent<Rigidbody2D>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
