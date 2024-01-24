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

    [Header("Level Options")]
    [SerializeField] private int weight;
    [SerializeField] private int height;
    private int[,] gridArrays;

    private Dictionary<Tuple<int, int>, GameObject> Paths = new Dictionary<Tuple<int, int>, GameObject>();
    private Dictionary<Tuple<int, int>, GameObject> Corners = new Dictionary<Tuple<int, int>, GameObject>();

    void Start()
    {
        gridArrays = new int[weight, height];

        GenerateLevel();
    }
    private void GenerateLevel()
    {
        for (int x = 0; x < gridArrays.GetLength(0); x++)
        {
            for (int y = 0; y < gridArrays.GetLength(1); y++)
            {
                GameObject clone = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);

                if (x == 0 || x == weight - 1 || y == 0 || y == height - 1)
                {
                    Corners.Add(new Tuple<int, int>(x, y), clone);
                    continue;
                }

                Paths.Add(new Tuple<int, int>(x, y), clone);
            }
        }
        GenerateManuelRoad(new List<Vector2> { new Vector2(5, 0), new Vector2(5, 1), new Vector2(5, 2), new Vector2(5, 3), new Vector2 (5,4), 
            new Vector2(5, 5), new Vector2(6, 5), new Vector2(7, 6), new Vector2(7,7), new Vector2(7,8), new Vector2(8,8) });    

    }
    private void GenerateManuelRoad(List<Vector2> MapPoints)
    {
        for(int x = 0; x < MapPoints.Count; x++)
        {
            if(x == 0 || x == MapPoints.Count) // baþlangýç ve bitiþ noktasýný ayrý alýyoruz
            {
                Corners.TryGetValue(new Tuple<int, int>((int)MapPoints[x].x,(int)MapPoints[x].y),out GameObject gridobject);
                Destroy(gridobject);
                continue;
            }

            Paths.TryGetValue(new Tuple<int, int>((int)MapPoints[x].x, (int)MapPoints[x].y), out GameObject grid);
            Destroy(grid);

        }
    }
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

}
