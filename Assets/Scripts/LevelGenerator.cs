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
    [SerializeField] private EnemyWaveManager spawnManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject gridPrefab2;

    [SerializeField] private GameObject pathPointPrefab;

    [Header("Level Data")]
    [SerializeField] private LevelPathernSO []SO_Level_Patherns;

    private LevelPathernSO.LevelData _currentLevelData;
    private int _currentLevelWidth;
    private int _currentLevelHeight;
    private int _currentLevel;

    private List<Transform> currentLevelPathPoint = new();

    private Dictionary<Tuple<int, int>, GameObject> Paths = new();


    void Awake()
    {
        _currentLevel = 1;
        GetcurrentLevelData();
        GenerateLevel();
    }

    private void GetcurrentLevelData()
    {
        foreach (var levelData in SO_Level_Patherns)
        {
            if (_currentLevel == levelData.levels.level)
            {
                _currentLevelHeight = levelData.levels.levelHeight;
                _currentLevelWidth = levelData.levels.levelWidth;
                _currentLevelData = levelData.levels;
                break;
            }
        }
        
    }

    private void GenerateLevel()
    {
        for (int x = 0; x <= _currentLevelWidth; x++)
        {
            for (int y = 0; y <= _currentLevelHeight; y++)
            {
                GameObject clone = Instantiate(GetGridPrefab(new Vector2(x, y)), new Vector3(x, y, 0), Quaternion.identity);

                clone.transform.SetParent(gridCategoryParrent);

                Paths.Add(new Tuple<int, int>(x, y), clone);
            }
        }
        GenerateRoad();
    }

    private void GenerateRoad()
    {
        foreach (var pathData in _currentLevelData.gridPath)
        {

            int totalPath = CalculateRoadLength(pathData, pathData.isMovementOnX);

            Vector2 StartPosition = new(pathData.pathStartPosition.x, pathData.pathStartPosition.y);

            for (int currentPathNumber = 0; currentPathNumber <= totalPath; currentPathNumber++)
            {
                GameObject gridObject;

                if (pathData.isMovementOnX)
                {
                    var newXLocation = IsRoadGoingReverseRotation(pathData, pathData.isMovementOnX) ? StartPosition.x - currentPathNumber : StartPosition.x + currentPathNumber;
                    Paths.TryGetValue(new Tuple<int, int>((int)newXLocation, (int)StartPosition.y), out gridObject);
                }
                else
                {
                    var newYLocation = IsRoadGoingReverseRotation(pathData, pathData.isMovementOnX) ? StartPosition.y - currentPathNumber : StartPosition.y + currentPathNumber;
                    Paths.TryGetValue(new Tuple<int, int>((int)StartPosition.x, (int)newYLocation), out gridObject);
                }
                Destroy(gridObject);
            }
            CreateAIPathPoint(pathData.pathEndPosition);

        }
        // DUZELTTTTTTTTTT
        spawnManager.GetCurrentLevelÝnformation(SO_Level_Patherns[0].GetLevelStartPosition(_currentLevel), currentLevelPathPoint);
    }

    private void CreateAIPathPoint(Vector2 pathSpawnPosition)
    {
        var clonePathPoint = Instantiate(pathPointPrefab, pathSpawnPosition, Quaternion.identity).transform;
        currentLevelPathPoint.Add(clonePathPoint);
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
            return 0 > data.pathEndPosition.x - data.pathStartPosition.x;

        return 0 > data.pathEndPosition.y - data.pathStartPosition.y;
    }

    private GameObject GetGridPrefab(Vector2 gridPosition)
    {
        if ((gridPosition.x % 2 == 0 && gridPosition.y % 2 != 0) || (gridPosition.x % 2 != 0 && gridPosition.y % 2 == 0))
                return gridPrefab;
        
         return gridPrefab2;
    }

    public List<Transform> GetPathList() => currentLevelPathPoint;


}
