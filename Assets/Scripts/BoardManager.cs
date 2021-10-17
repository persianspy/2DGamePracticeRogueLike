using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }

    }

    public int columns = 8;
    public int rows = 8;

    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform BoardHolder;
    private List<Vector3> GridPositions = new List<Vector3>();

    void InitialiseList()
    {
        GridPositions.Clear();

        for (int x=1; x < columns -1; x++)
        {
            for (int y = 1; y < rows -1; y++)
            {
                GridPositions.Add(new Vector3(x, y, 0f));
            }
        }

    }

    void BoardSetup()
    {
        BoardHolder = new GameObject("Board").transform;

        for( int x=-1; x < columns +1; x++)
        {
            for( int y=-1; y < rows +1; y++)
            {
                GameObject ToInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    ToInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
                }

                GameObject Instance = Instantiate(ToInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                Instance.transform.SetParent(BoardHolder);

            }
        }

    }

    Vector3 RandomPosition()
    {
        int RandomIndex = Random.Range(0, GridPositions.Count);
        Vector3 RandomPosition = GridPositions[RandomIndex];
        GridPositions.RemoveAt(RandomIndex);
        return RandomPosition;
    }


    void LayoutObjectAtRandom(GameObject[] TileArray, int Minimum, int Maximum)
    {
        int ObjectCount = Random.Range(Minimum, Maximum + 1);

        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 randomposition = RandomPosition();
            GameObject TileChoice = TileArray[Random.Range(0, TileArray.Length)];
            Instantiate(TileChoice, randomposition, Quaternion.identity);

        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(WallTiles, WallCount.minimum, WallCount.maximum);
        LayoutObjectAtRandom(FoodTiles, FoodCount.minimum, FoodCount.maximum);
        int EnemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(EnemyTiles, EnemyCount, EnemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity); 
    }


  
}
