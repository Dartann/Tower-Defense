using UnityEngine;

public class Shop : MonoBehaviour
{
    ShopİtemData shopData;

    int money;

    private void Start(){
        shopData = GetComponent<ShopİtemData>();
    }
    public void Buy(TowerDataSO buyedObjectSO){
        if (shopData.İsTowerUnlocked(buyedObjectSO.towerID))
        {
            Debug.Log("Tower already unlocked");
            return;
        }

        Debug.Log("Tower is unlocked");
        // check money 
        // tell UIManager new tower added
        shopData.AddUnlockedTower(buyedObjectSO);

    }
    private void İncreaseMoney(int moneyAmount) => money += moneyAmount;


    private void OnEnable() => EnemyHealth.Event_OnEnemyDie += İncreaseMoney;
    private void OnDisable() => EnemyHealth.Event_OnEnemyDie -= İncreaseMoney;

}

