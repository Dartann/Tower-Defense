using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerScript : MonoBehaviour
{
    [Header("Object Data")]
    [SerializeField] protected TowerDataSO m_DataSO;
    [SerializeField] protected GameObject bulletPrefab;

    [Header("AI Settings")]
    [SerializeField] protected LayerMask targetMask;

    [Header("Tower Parts")]
    [SerializeField] GameObject towerTop;
    [SerializeField] Transform bulletSpawnPosition;
    
    Transform target;

    public int Damage { protected set; get; }
    public int Range { protected set; get; }
    public float FireRate { protected set; get; }

    bool isAlreadyAttacking = false;

    private void Awake(){
        Damage = m_DataSO.damage;
        Range = m_DataSO.range;
        FireRate = m_DataSO.fireRate;
    }

    private void Update() => FindTarget();
    void FindTarget()
    {
        if (target)
        {
            TurnHeadToEnemy();
            CheckTargetIsInRange();
            return;
        }
        SetTarget();
    }
    protected virtual void SetTarget(){

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, Range, targetMask);

        if (targetsInViewRadius.Length > 0)
            target = targetsInViewRadius[0].transform;

    }
    protected void TurnHeadToEnemy()
    {
        if (target)
        {
            var dirToTarget = (target.position - transform.position).normalized;
            var angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            towerTop.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
    protected void CheckTargetIsInRange()
    {
        if(Vector3.Distance(transform.position, target.position) > Range){      
            target = null;
            return;
        }
        StartCoroutine(AttackEnemy());
}

    protected virtual IEnumerator AttackEnemy()
    {
        if (isAlreadyAttacking)
            yield break;

        isAlreadyAttacking = true;

        Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.Euler(bulletSpawnPosition.transform.rotation.eulerAngles));

        yield return new WaitForSeconds(FireRate);

        isAlreadyAttacking = false;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}

