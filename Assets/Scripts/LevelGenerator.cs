using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [Header("Hierarcy_Categorys")]
    [SerializeField] private Transform gridCategoryParrent;
    [SerializeField] private Transform pathCategoryParrent;

    [Header("Managers")]
    [SerializeField] private EnemySpawnManager enemySpawnManager; // eventen çek
    [SerializeField] private LevelDataHolder levelDataHolder; // eventen çek

    [Header("Prefabs")]
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject pathPointPrefab;

    private LevelPathernSO _currentLevelData;
    [SerializeField] private int _currentLevel;

    private List<Transform> currentLevelAIPathPoints = new();
    private Dictionary<Vector3, GameObject> currentLevelGrids = new();

    Color32 color = new Color32(47, 188, 57, 255);

    void Start()
    {
        enemySpawnManager = GetComponent<EnemySpawnManager>();
        levelDataHolder = GetComponent<LevelDataHolder>();

        _currentLevel = 2;

        GetcurrentLevelData();
        GenerateLevel();
    }

    private void GetcurrentLevelData() => _currentLevelData = levelDataHolder.GetCurrentLevelData(_currentLevel);    

    private void GenerateLevel()
    {
        for (int x = 0; x <= _currentLevelData.levels.levelWidth; x++)
        {
            for (int y = 0; y <= _currentLevelData.levels.levelHeight; y++)
            {
                GameObject cloneGrid = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);

                ChangeGridColorBasedOnPosition(new Vector2(x, y), cloneGrid);

                cloneGrid.transform.SetParent(gridCategoryParrent);

                currentLevelGrids.Add(new Vector3(x, y), cloneGrid);
            }
        }
        GenerateRoad();
    }

    private void GenerateRoad()
    {
        foreach (var pathData in _currentLevelData.levels.gridPath)
        {
            int RoadLength = CalculateRoadLength(pathData, pathData.isMovementOnX);

            Vector2 StartPosition = new(pathData.pathStartPosition.x, pathData.pathStartPosition.y);

            for (int currentPathNumber = 0; currentPathNumber <= RoadLength; currentPathNumber++)
            {
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
        enemySpawnManager.SetCurrentLevelÝnformation(_currentLevelData.GetLevelStartPosition(_currentLevel), currentLevelAIPathPoints);
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

        cloneGrid.GetComponent<SpriteRenderer>().color = color;
    }

    public List<Transform> GetPathList() => currentLevelAIPathPoints;


}
