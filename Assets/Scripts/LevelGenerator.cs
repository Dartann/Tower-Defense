using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject pathPointPrefab;

    [Header("Level Options")]
    [SerializeField] private LevelPathernSO SO_Level_Patherns;

    private LevelPathernSO.LevelData _currentLevelData;
    private int _currentLevelWidth;
    private int _currentLevelHeight;
    [SerializeField] private int _currentLevel = 1;
    
    private int[,] _gridArrays;

    private Dictionary<Tuple<int, int>, GameObject> Paths = new Dictionary<Tuple<int, int>, GameObject>();

    void Start()
    {
        GetcurrentLevelData();
        GenerateLevel();
    }

    private void GetcurrentLevelData()
    {
        foreach (var levelData in SO_Level_Patherns.levels)
        {
            if (_currentLevel == levelData.level)
            {
                _currentLevelHeight = levelData.levelHeight;
                _currentLevelWidth = levelData.levelWidth;
                _gridArrays = new int[levelData.levelWidth, levelData.levelHeight];
                _currentLevelData = levelData;
            }
        }
    }

    private void GenerateLevel()
    {
        for (int x = 0; x < _gridArrays.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArrays.GetLength(1); y++)
            {
                GameObject clone = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);

                if ((x == 0 && y == 0) || (x == 0 && y == _currentLevelHeight - 1) || (x == _currentLevelWidth - 1 && y == 0) || x == _currentLevelWidth - 1 && y == _currentLevelHeight - 1)    
                    continue;
                
                Paths.Add(new Tuple<int, int>(x, y), clone);
            }
        }
        GenerateRoad();    

    }
    private void GenerateRoad()
    {
        foreach (var Data in _currentLevelData.gridPath)
        {
            if (Data.spawnPathNode)
                Instantiate(pathPointPrefab, new Vector2(Data.roadPosition.x, Data.roadPosition.y), quaternion.identity);

            Paths.TryGetValue(new Tuple<int, int>((int)Data.roadPosition.x, (int)Data.roadPosition.y), out GameObject gridobject);
            Destroy(gridobject);
        }       
    }
    /*
    private void ChoiceRandomStartAndEndPosition()
    {
        int randomNumber = UnityEngine.Random.Range(0, 2); // 0 gelirse x üzerinden, 1 gelirse y üzerinden baþlat.
        bool isOnX = isOnXRotation(randomNumber);
        Debug.Log("Gelen sayý:" + randomNumber);

        int startPoint = randomNumber == 0 ? UnityEngine.Random.Range(minInclusive: 1, weight - 2) : UnityEngine.Random.Range(1, height - 2);
        int endPoint = randomNumber == 0 ? UnityEngine.Random.Range(1, weight - 2) : UnityEngine.Random.Range(1, height - 2);
        Debug.Log("Baþlangýç numarasý:" + startPoint + ", bitiþ numarasý: " + endPoint);

        GameObject startPosition = randomNumber == 0 ? Corners[new Tuple<int, int>(startPoint, 0)] : Corners[new Tuple<int, int>(0, startPoint)];
        GameObject endPosition = randomNumber == 0 ? Corners[new Tuple<int, int>(endPoint, height - 1)] : Corners[new Tuple<int, int>(weight - 1, endPoint)];

        //GenerateRandomRoad(startPosition.gameObject.transform.position, endPosition.gameObject.transform.position, isOnX);
        Destroy(startPosition);
        Destroy(endPosition);    
        
    }
    private void GenerateRandomRoad(Vector2 startPosition, Vector2 endPosition, bool isOnXRotation)
    {
        // YARIM KALDI

        float numberNeedGoOnX;
        float numberNeedGoOnY;
        if (isOnXRotation)
        {
            if (startPosition.x == endPosition.x)
            {
                numberNeedGoOnX = 0;
                return;
            }
            bool xUporDown = startPosition.x > endPosition.x ? true : false;
            numberNeedGoOnX = xUporDown ? startPosition.x - endPosition.x : endPosition.x - startPosition.x;     
        }
        else
        {
            if (startPosition.y == endPosition.y)
            {
                numberNeedGoOnY = 0;
                return;
            }
            bool yUporDown = startPosition.y > endPosition.y ? true : false;
            numberNeedGoOnY = yUporDown ? startPosition.y - endPosition.y : endPosition.y - startPosition.y;
        }

        Vector2 currentPosition = startPosition;


    }
    private bool isOnXRotation(int number) => number == 0;
    */
}
