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
    [SerializeField] protected TowerDataSO towerDataSO;
    public float WorkSpeed { get; protected set; }

    protected bool isAlreadyWorking = false;

    protected virtual void Awake() => SetStatsFromSO();
    protected abstract void SetStatsFromSO();
    public TowerDataSO GetTowerData() => towerDataSO;
}

