using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    GridObject currentGridTileObject;
    BaseTowerScript currentBuyedTowerObject;
    BaseTowerScript currentGridTowerUpgradeVersion;

    bool isInBuildingMode = true;

    //can be null if outside grid
    private void PlacementManager_UpdateCurrentGridObject(GridObject gridobject) => currentGridTileObject = gridobject;
    private void PlacementManager_UpdateCurrentBuyedTower(BaseTowerScript buyedTower) => currentBuyedTowerObject = buyedTower;

    //can be null if upgrade not exist
    private void PlacementManager_UpdateCurrentGridTowerUpgrade(BaseTowerScript gridTowerUpgradeVersion) => currentGridTowerUpgradeVersion = gridTowerUpgradeVersion;
    private void Awake()
    {
        GridObject.Event_UpdateCurrentGridobject += PlacementManager_UpdateCurrentGridObject;

        TowerFactory.Event_UpdateCurrentBuyedTower += PlacementManager_UpdateCurrentBuyedTower;
        TowerFactory.Event_CurrentGridTowerUpgradeVersion += PlacementManager_UpdateCurrentGridTowerUpgrade;

        UIManager.Event_ÝnBuildMode += DisableOrEnableBuildMode;

    }
    private void Update()
    {
        Testputmethod();
    }
    private void Testputmethod()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanBuild())
            BuildOnCurrentGridTile();
        
    }
    private void BuildOnCurrentGridTile()
    {
        BaseTowerScript towerType = GetTowerType();

        if (towerType != null)
        {
            var NewTower = Instantiate(towerType, currentGridTileObject.transform.position, Quaternion.identity);
            currentGridTileObject.SetTowerObjectOnGrid(NewTower);           
        }

    }
    private BaseTowerScript GetTowerType()
    {
        BaseTowerScript towerType;
        if (!currentGridTileObject.IsThereTowerOnGrid)
            towerType = currentBuyedTowerObject;
        else if (currentGridTowerUpgradeVersion)
            towerType = currentGridTowerUpgradeVersion;
        else
            towerType = null;
        return towerType;
    }

    private bool CanBuild() => isInBuildingMode && currentGridTileObject && currentBuyedTowerObject;
    private void DisableOrEnableBuildMode(bool mode) => isInBuildingMode = mode;

}
