using UnityEngine;

public class Shop : MonoBehaviour
{
    Shop›temData shopData;

    int money;

    private void Start(){
        shopData = GetComponent<Shop›temData>();
    }
    public void Buy(TowerDataSO buyedObjectSO){
        if (shopData.›sTowerUnlocked(buyedObjectSO.towerID))
        {
            Debug.Log("Tower already unlocked");
            return;
        }

        Debug.Log("Tower is unlocked");
        // check money 
        // tell UIManager new tower added
        shopData.AddUnlockedTower(buyedObjectSO);

    }
    private void ›ncreaseMoney(int moneyAmount) => money += moneyAmount;


    private void OnEnable() => EnemyHealth.Event_OnEnemyDie += ›ncreaseMoney;
    private void OnDisable() => EnemyHealth.Event_OnEnemyDie -= ›ncreaseMoney;

}

