using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform wallPrefab;
    public Vector2 levelSize;

    [Range(0, 1)]
    public float tileSize = 1;
    [Range(0, 1)]
    public float wallsAmount = 0.5f;

    List<Coords> tileCoords;
    Queue<Coords> shuffledTileCoords;

    public int seed = 0;
    Coords levelCenter;

    // Coord Struct
    public struct Coords
    {
        public int x;
        public int y;

        public Coords(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coords a, Coords b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Coords a, Coords b)
        {
            return !(a == b);
        }
    }

    void Start()
    {
        GenerateLevel();
    }

    // Generates the level
    public void GenerateLevel()
    {
        tileCoords = new List<Coords>();
        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                tileCoords.Add(new Coords(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coords>(FYshuffle(tileCoords.ToArray(), seed));
        levelCenter = new Coords((int)levelSize.x / 2, (int)levelSize.y / 2);

        string container = "Generated";
        if (transform.Find(container))
        {
            DestroyImmediate(transform.Find(container).gameObject);
        }

        Transform levelContainer = new GameObject(container).transform;
        levelContainer.parent = transform;

        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                Vector3 tilePosition = CoordsToPos(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * tileSize;
                newTile.parent = levelContainer;
            }
        }

        // Store the currently taken coords into a map
        bool[,] wallsMap = new bool[(int)levelSize.x, (int)levelSize.y];

        int wallsCount = (int)(levelSize.x * levelSize.y * wallsAmount);
        int currentWallsCount = 0;

        for (int i = 0; i < wallsCount; i++)
        {
            Coords randomCoord = GetRandomCoord();
            wallsMap[randomCoord.x, randomCoord.y] = true;
            currentWallsCount++;

            if (randomCoord != levelCenter && IsFullyAccessible(wallsMap, currentWallsCount))
            {
                Vector3 obstaclePosition = CoordsToPos(randomCoord.x, randomCoord.y);

                Transform newObstacle = Instantiate(wallPrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                newObstacle.parent = levelContainer;
            }
            else
            {
                // Don't store the new tile coords if it's not valid
                wallsMap[randomCoord.x, randomCoord.y] = false;
                currentWallsCount--;
            }
        }
    }

    // Fisher-yates shuffle algorithm
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

    // Checks if the map is fully accessible
    private bool IsFullyAccessible(bool[,] wallsMap, int currentWallsAmount)
    {
        bool[,] visitedTilesMap = new bool[wallsMap.GetLength(0), wallsMap.GetLength(1)];
        Queue<Coords> queue = new Queue<Coords>();
        queue.Enqueue(levelCenter);
        visitedTilesMap[levelCenter.x, levelCenter.y] = true;

        int accessibleTileCount = 1;

        // DON'T TOUCH | IT'S HIDEOUS BUT IT WORKS
        while (queue.Count > 0)
        {
            Coords tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourTileX = tile.x + x;
                    int neighbourTileY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourTileX >= 0 && neighbourTileX < wallsMap.GetLength(0) && neighbourTileY >= 0 && neighbourTileY < wallsMap.GetLength(1))
                        {
                            if (!visitedTilesMap[neighbourTileX, neighbourTileY] && !wallsMap[neighbourTileX, neighbourTileY])
                            {
                                visitedTilesMap[neighbourTileX, neighbourTileY] = true;
                                queue.Enqueue(new Coords(neighbourTileX, neighbourTileY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(levelSize.x * levelSize.y - currentWallsAmount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    // Get a random coord out of the shuffled tiles
    public Coords GetRandomCoord()
    {
        Coords randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    // Convert 2D coords to 3D
    Vector3 CoordsToPos(int x, int y)
    {
        return new Vector3(-levelSize.x / 2 + 0.5f + x, 0, -levelSize.y / 2 + 0.5f + y);
    }
}