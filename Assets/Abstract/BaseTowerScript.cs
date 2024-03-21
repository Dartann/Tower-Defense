using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    protected Collider2D target;
    Collider2D[] targetsInViewRadius;

    public int Damage { get; protected set; }
    public int Range { get; protected set; }
    public float FireRate { get; protected set; }
    public int BulletSpeed { get; protected set; }

    protected bool isAlreadyAttacking = false;

    protected void Awake()
    {
        targetMask = LayerMask.GetMask("Enemy");
        towerTop = transform.Find("Towertop");
        SetStatsFromSO();
    }
    protected void Update() => GetTarget();
    protected void GetTarget()
    {
        FindTarget();
        SetTarget();

        if (target)
        {
            CheckTargetIsInRange();
            TurnHeadToEnemy();
            return;
        }
    }
    protected void FindTarget() => targetsInViewRadius = Physics2D.OverlapBoxAll(transform.position, new Vector2(Range, Range), 0f, targetMask);

    // if tower looking for multiple enemy override this
    protected virtual void SetTarget()
    {
        if (targetsInViewRadius.Length > 0 && !target)
            target = targetsInViewRadius[0];
    }
    protected void CheckTargetIsInRange(){
        if (targetsInViewRadius.Contains(target))
            return;
        target = null;      
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
    protected virtual void FireBullet(){
        Instantiate(towerBulletPrefab, bulletSpawnPosition[0].position, Quaternion.Euler(bulletSpawnPosition[0].transform.rotation.eulerAngles))
            .GetComponent<Bullet>().Instantiate(Damage, BulletSpeed);          
        }
    protected void TurnHeadToEnemy()
    {
        if (target)
        {
            var dirToTarget = (target.transform.position - transform.position).normalized;
            var angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            towerTop.eulerAngles = new Vector3(0, 0, angle);
            StartCoroutine(AttackEnemy());
        }
    }
    protected void SetStatsFromSO()
    {
        Damage = towerDataSO.damage;
        Range = towerDataSO.range;
        FireRate = towerDataSO.fireRate;
        BulletSpeed = towerDataSO.bulletSpeed;
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector2(Range, Range));
    }

    public TowerDataSO GetTowerData() => towerDataSO;
}

