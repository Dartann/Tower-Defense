using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseFighterTower : BaseTowerScript
{
    [Header("Object Data")]
    [SerializeField] protected GameObject towerBulletPrefab;

    [Header("AI Settings")]
    protected LayerMask targetMask;

    [Header("Tower Parts")]
    [SerializeField] protected Transform[] bulletSpawnPosition;
    protected Transform towerTop;

    protected Collider2D target;
    protected Collider2D[] targetsInViewRadius;

    public int Damage { get; protected set; }
    public int BulletSpeed { get; protected set; }
    public int Range { get; protected set; }

    protected override void Awake()
    {
        targetMask = LayerMask.GetMask("Enemy");
        towerTop = transform.Find("Towertop");
        base.Awake();
        
    }
    protected void Update() => GetTarget();
    protected void GetTarget()
    {
        FindTarget();

        if (!target)
            SetTarget();
        else
        {
            CheckTargetIsInRange();
            TurnHeadToEnemy();
        }

    } 
    // if tower looking for multiple enemy override this
    protected virtual void SetTarget()
    {
        if (targetsInViewRadius.Length > 0)
            target = targetsInViewRadius[0];
    }
    protected void FindTarget() => targetsInViewRadius = Physics2D.OverlapBoxAll(transform.position, new Vector2(Range, Range), 0f, targetMask);
    protected void CheckTargetIsInRange()
    {
        if (targetsInViewRadius.Contains(target))
            return;
        target = null;
    }
    protected IEnumerator AttackEnemy()
    {
        if (isAlreadyWorking)
            yield break;

        isAlreadyWorking = true;

        FireBullet();

        yield return new WaitForSeconds(WorkSpeed);

        isAlreadyWorking = false;
    }

    //if tower attack different override this
    protected virtual void FireBullet()
    {
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
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector2(Range, Range));
    }

    protected override void SetStatsFromSO()
    {
        var tower = towerDataSO as FighterTowerSO;

        Damage = tower.Damage;
        WorkSpeed = tower.FireRate;
        Range = tower.Range;

    }
}
