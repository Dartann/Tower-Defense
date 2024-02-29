using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] TowerDataSO m_DataSO;

    [SerializeField] LayerMask targetMask;
    
    Transform target;

    public int Damage { protected set; get; }
    public int Range { protected set; get; }
    public float FireRate { protected set; get; }


    bool isAlreadyAttacking = false;
    private void Awake()
    {
        Damage = m_DataSO.damage;
        Range = m_DataSO.range;
        FireRate = m_DataSO.fireRate;
    }
    private void Update() => FindTarget();
    void FindTarget()
    {
        if (target)
        {
            CheckTargetIsInRange();
            return;
        }
        SetTarget();
    }
    void SetTarget(){

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, Range, targetMask);

        if (targetsInViewRadius.Length > 0)
            target = targetsInViewRadius[0].transform;
            
        // düþmanýn posizyonu hesapla ve ona doðru dön
        // mermi düþmana kitli olsun(?) ýskalama ihtimali var.

    }
    void CheckTargetIsInRange()
    {
        if(Vector3.Distance(transform.position, target.position) > Range){      
            target = null;
            return;
        }
        StartCoroutine(AttackEnemy());
}

    IEnumerator AttackEnemy()
    {
        if (isAlreadyAttacking)
            yield break;

        isAlreadyAttacking = true;

        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

        enemyHealth.TakeDamage(Damage);

        yield return new WaitForSeconds(FireRate);

        isAlreadyAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}

