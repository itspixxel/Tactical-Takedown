using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{

    public Transform tilePrefab;
    public Transform wallPrefab;
    public Vector2 mapSize;

    [Range(1, 0)]
    public float tileSize;

    List<Coord> tileCoords;
    Queue<Coord> shuffledTileCoords;

    public int seed = 10;

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {

        tileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                tileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(FYshuffle(tileCoords.ToArray(), seed));

        string container = "Generated";
        if (transform.Find(container))
        {
            DestroyImmediate(transform.Find(container).gameObject);
        }

        Transform mapContainer = new GameObject(container).transform;
        mapContainer.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPos(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * tileSize;
                newTile.parent = mapContainer;
            }
        }


        int obstacleCount = 10;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPos(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(wallPrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity);
            newObstacle.parent = mapContainer;
        }

    }

    public static A[] FYshuffle<A>(A[] array, int seed) {
		System.Random rng = new System.Random (seed);

		for (int i =0; i < array.Length -1; i ++) {
			int randomIndex = rng.Next(i,array.Length);
			A tempItem = array[randomIndex];
			array[randomIndex] = array[i];
			array[i] = tempItem;
		}

		return array;
	}

    Vector3 CoordToPos(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }
}