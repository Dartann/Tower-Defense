using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [Header("Hierarcy_Categorys")]
    [SerializeField] private Transform gridCategoryParrent;
    [SerializeField] private Transform pathCategoryParrent;

    [Header("Managers")]
    [SerializeField] private EnemySpawnManager enemySpawnManager; // eventen �ek
    [SerializeField] private LevelDataHolder levelDataHolder; // eventen �ek

    [Header("Prefabs")]
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject pathPointPrefab;

    [Header("GridColor")]
    [SerializeField] private Color32 gridColor;

    [SerializeField] private LevelPathernSO _currentLevelData;
    [SerializeField] private int _currentLevel;

    private List<Transform> currentLevelAIPathPoints = new();
    private Dictionary<Vector3, GameObject> currentLevelGrids = new();

    void Start()
    {
        levelDataHolder = GetComponent<LevelDataHolder>();

        _currentLevel = 2;

        GetcurrentLevelData();
        StartCoroutine(GenerateLevel());
    }

    private void GetcurrentLevelData() => _currentLevelData = levelDataHolder.GetCurrentLevelData(_currentLevel);    

    private IEnumerator GenerateLevel()
    {
        for (int x = 0; x <= _currentLevelData.levels.levelWidth; x++)
        {
            for (int y = 0; y <= _currentLevelData.levels.levelHeight; y++)
            {
                yield return new WaitForSeconds(0.01f);

                GameObject cloneGrid = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);

                ChangeGridColorBasedOnPosition(new Vector2(x, y), cloneGrid);

                cloneGrid.transform.SetParent(gridCategoryParrent);

                currentLevelGrids.Add(new Vector3(x, y), cloneGrid);
            }
        }
        StartCoroutine(GenerateRoad());
    }

    private IEnumerator GenerateRoad()
    {
        foreach (var pathData in _currentLevelData.levels.gridPath)
        {
            int RoadLength = CalculateRoadLength(pathData, pathData.isMovementOnX);

            Vector2 StartPosition = new(pathData.pathStartPosition.x, pathData.pathStartPosition.y);

            for (int currentPathNumber = 0; currentPathNumber <= RoadLength; currentPathNumber++)
            {
                yield return new WaitForSeconds(0.03f);

                GameObject gridObject;

                if (pathData.isMovementOnX)
                {
                    int newXLocation = IsRoadGoingReverseRotation(pathData, pathData.isMovementOnX) ? (int)StartPosition.x - currentPathNumber : (int)StartPosition.x + currentPathNumber;
                    currentLevelGrids.TryGetValue(new Vector2(newXLocation, StartPosition.y), out gridObject);
                }
                else
                {
                    int newYLocation = IsRoadGoingReverseRotation(pathData, pathData.isMovementOnX) ? (int)StartPosition.y - currentPathNumber : (int)StartPosition.y + currentPathNumber;
                    currentLevelGrids.TryGetValue(new Vector2(StartPosition.x, newYLocation), out gridObject);
                }
                Destroy(gridObject);
            }
            CreateAIPathPoint(pathData.pathEndPosition);

        }
        enemySpawnManager.SetCurrentLevel�nformation(_currentLevelData.GetLevelStartPosition(_currentLevel), currentLevelAIPathPoints);
    }

    private void CreateAIPathPoint(Vector2 pathSpawnPosition)
    {
        var clonePathPoint = Instantiate(pathPointPrefab, pathSpawnPosition, Quaternion.identity).transform;
        currentLevelAIPathPoints.Add(clonePathPoint);
        clonePathPoint.transform.SetParent(pathCategoryParrent);
    }

    private int CalculateRoadLength(LevelPathernSO.LevelPathInformation data, bool isOnXaxis)
    {
        if (isOnXaxis)
            return (int)MathF.Abs(data.pathStartPosition.x - data.pathEndPosition.x);

        return (int)MathF.Abs(data.pathStartPosition.y - data.pathEndPosition.y);

    }
    private bool IsRoadGoingReverseRotation(LevelPathernSO.LevelPathInformation data, bool isOnXaxis)
    {
        if (isOnXaxis)
            return 0 >= data.pathEndPosition.x - data.pathStartPosition.x;

        return 0 >= data.pathEndPosition.y - data.pathStartPosition.y;
    }

    private void ChangeGridColorBasedOnPosition(Vector2 gridPosition, GameObject cloneGrid)
    {
        // use orginal prefab color
        if ((gridPosition.x % 2 == 0 && gridPosition.y % 2 != 0) || (gridPosition.x % 2 != 0 && gridPosition.y % 2 == 0))
              return;

        cloneGrid.GetComponent<SpriteRenderer>().color = gridColor;
    }
 
    public List<Transform> GetPathList() => currentLevelAIPathPoints;

}
