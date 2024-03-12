using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public abstract class BaseTowerScript : MonoBehaviour
{
    [Header("Object Data")]
    [SerializeField] protected TowerDataSO towerDataSO;
    [SerializeField] protected GameObject towerBulletPrefab;

    [Header("AI Settings")]
    protected LayerMask targetMask;

    [Header("Tower Parts")]
    [SerializeField] protected Transform []bulletSpawnPosition;
    protected Transform towerTop;
    
    protected Transform target;

    public int Damage { get; protected set; }
    public int Range { get; protected set; }
    public float FireRate { get; protected set; }

    protected bool isAlreadyAttacking = false;

    protected void Awake(){
        targetMask = LayerMask.GetMask("Enemy");
        towerTop = transform.Find("Towertop");

        Damage = towerDataSO.damage;
        Range = towerDataSO.range;
        FireRate = towerDataSO.fireRate;
    }

    protected void Update() => CheckTarget();
    protected void CheckTarget()
    {
        if (target)
        {
            TurnHeadToEnemy();
            CheckTargetIsInRange();
            return;
        }
        FindTarget();
    }
    protected virtual void FindTarget(){

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, Range, targetMask);

        if (targetsInViewRadius.Length > 0)
            target = targetsInViewRadius[0].transform;

    } // if tower looking for multiple enemy override this

    protected void CheckTargetIsInRange(){
        if(Vector3.Distance(transform.position, target.position) > Range){      
            target = null;
            Debug.Log("Dýþarýda");
            return;
        }
        StartCoroutine(AttackEnemy());
    }
    protected IEnumerator AttackEnemy()
    {
        if (isAlreadyAttacking)
            yield break;

        isAlreadyAttacking = true;

        FireBullet();

        yield return new WaitForSeconds(FireRate);

        isAlreadyAttacking = false;
    }
    
    //if tower attack different override this
    protected virtual void FireBullet() => Instantiate(towerBulletPrefab, bulletSpawnPosition[0].position, Quaternion.Euler(bulletSpawnPosition[0].transform.rotation.eulerAngles));
    protected void TurnHeadToEnemy()
    {
        if (target)
        {
            var dirToTarget = (target.position - transform.position).normalized;
            var angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            towerTop.eulerAngles = new Vector3(0, 0, angle);
        }
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    public TowerDataSO GetTowerData() => towerDataSO;
}

