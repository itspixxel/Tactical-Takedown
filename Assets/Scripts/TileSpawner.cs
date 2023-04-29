using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;

public class TileSpawner : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform wallPrefab;
    public Transform navMeshFloor;
    public Transform navMeshBounds;
    public Vector2 maxLevelSize;

    public Level[] levels;
    public int levelIndex;

    public float tileSize;

    [Range(0, 1)]
    public float tileScale = 1;

    private List<Coords> tileCoords;
    private Queue<Coords> shuffledTileCoords;

    private Level currentLevel;

    // Coord Struct
    [System.Serializable]
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

    [System.Serializable]
    public class Level
    {
        public int seed;
        public float minwallHeight;
        public float maxwallHeight;

        public Coords levelSize;
        public Coords levelCenter { get { return new Coords(levelSize.x / 2, levelSize.y / 2); } }

        public Color gradientTop;
        public Color gradientBottom;

        [Range(0, 1)]
        public float wallsAmount;
    }

    void Start()
    {
        GenerateLevel();
    }

    // Generates the level
    public void GenerateLevel()
    {
        currentLevel = levels[levelIndex];
        System.Random rng = new System.Random(currentLevel.seed);

        tileCoords = new List<Coords>();
        for (int x = 0; x < currentLevel.levelSize.x; x++)
        {
            for (int y = 0; y < currentLevel.levelSize.y; y++)
            {
                tileCoords.Add(new Coords(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coords>(FYshuffle(tileCoords.ToArray(), currentLevel.seed));

        string container = "Generated";
        if (transform.Find(container))
        {
            DestroyImmediate(transform.Find(container).gameObject);
        }

        Transform levelContainer = new GameObject(container).transform;
        levelContainer.parent = transform;

        for (int x = 0; x < currentLevel.levelSize.x; x++)
        {
            for (int y = 0; y < currentLevel.levelSize.y; y++)
            {
                Vector3 tilePosition = CoordsToPos(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * tileScale * tileSize;
                newTile.parent = levelContainer;
            }
        }

        // Store the currently taken coords into a map
        bool[,] wallsMap = new bool[(int)currentLevel.levelSize.x, (int)currentLevel.levelSize.y];

        int wallsCount = (int)(currentLevel.levelSize.x * currentLevel.levelSize.y * currentLevel.wallsAmount);
        int currentWallsCount = 0;

        for (int i = 0; i < wallsCount; i++)
        {
            Coords randomCoord = GetRandomCoord();
            wallsMap[randomCoord.x, randomCoord.y] = true;
            currentWallsCount++;

            if (randomCoord != currentLevel.levelCenter && IsFullyAccessible(wallsMap, currentWallsCount))
            {
                float wallHeight = Mathf.Lerp(currentLevel.minwallHeight, currentLevel.maxwallHeight, (float)rng.NextDouble());
                Vector3 wallPosition = CoordsToPos(randomCoord.x, randomCoord.y);

                Transform newWall = Instantiate(wallPrefab, wallPosition + Vector3.up * wallHeight / 2, Quaternion.identity);
                newWall.parent = levelContainer;
                newWall.localScale = new Vector3((tileScale) * tileSize, wallHeight, (tileScale) * tileSize);
            }
            else
            {
                // Don't store the new tile coords if it's not valid
                wallsMap[randomCoord.x, randomCoord.y] = false;
                currentWallsCount--;
            }
        }

        Transform leftBound = Instantiate(navMeshBounds, Vector3.left * (currentLevel.levelSize.x + maxLevelSize.x) / 4 * tileSize, Quaternion.identity);
        leftBound.parent = levelContainer;
        leftBound.localScale = new Vector3((maxLevelSize.x - currentLevel.levelSize.x) / 2, 1, currentLevel.levelSize.y) * tileSize;

        Transform rightBound = Instantiate(navMeshBounds, Vector3.right * (currentLevel.levelSize.x + maxLevelSize.x) / 4 * tileSize, Quaternion.identity);
        rightBound.parent = levelContainer;
        rightBound.localScale = new Vector3((maxLevelSize.x - currentLevel.levelSize.x) / 2, 1, currentLevel.levelSize.y) * tileSize;

        Transform topBound = Instantiate(navMeshBounds, Vector3.forward * (currentLevel.levelSize.x + maxLevelSize.x) / 4 * tileSize, Quaternion.identity);
        topBound.parent = levelContainer;
        topBound.localScale = new Vector3(maxLevelSize.x, 1, (maxLevelSize.x - currentLevel.levelSize.x) / 2) * tileSize;

        Transform bottomBound = Instantiate(navMeshBounds, Vector3.back * (currentLevel.levelSize.x + maxLevelSize.x) / 4 * tileSize, Quaternion.identity);
        bottomBound.parent = levelContainer;
        bottomBound.localScale = new Vector3(maxLevelSize.x, 1, (maxLevelSize.x - currentLevel.levelSize.x) / 2) * tileSize;


        navMeshFloor.localScale = new Vector3(maxLevelSize.x, maxLevelSize.y) * tileSize;
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

    // Checking each neighbor individually (SLOWER)
    //private bool IsFullyAccessible(bool[,] wallsMap, int currentWallsAmount)
    //{
    //    bool[,] visitedTilesMap = new bool[wallsMap.GetLength(0), wallsMap.GetLength(1)];
    //    Queue<Coords> queue = new Queue<Coords>();
    //    queue.Enqueue(currentLevel.levelCenter);
    //    visitedTilesMap[currentLevel.levelCenter.x, currentLevel.levelCenter.y] = true;

    //    int accessibleTileCount = 1;

    //    // DON'T TOUCH | IT'S HIDEOUS BUT IT WORKS
    //    while (queue.Count > 0)
    //    {
    //        Coords tile = queue.Dequeue();

    //        for (int x = -1; x <= 1; x++)
    //        {
    //            for (int y = -1; y <= 1; y++)
    //            {
    //                int neighbourTileX = tile.x + x;
    //                int neighbourTileY = tile.y + y;
    //                if (x == 0 || y == 0)
    //                {
    //                    if (neighbourTileX >= 0 && neighbourTileX < wallsMap.GetLength(0) && neighbourTileY >= 0 && neighbourTileY < wallsMap.GetLength(1))
    //                    {
    //                        if (!visitedTilesMap[neighbourTileX, neighbourTileY] && !wallsMap[neighbourTileX, neighbourTileY])
    //                        {
    //                            visitedTilesMap[neighbourTileX, neighbourTileY] = true;
    //                            queue.Enqueue(new Coords(neighbourTileX, neighbourTileY));
    //                            accessibleTileCount++;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    int targetAccessibleTileCount = (int)(currentLevel.levelSize.x * currentLevel.levelSize.y - currentWallsAmount);
    //    return targetAccessibleTileCount == accessibleTileCount;
    //}

    // Flood fill method (FASTER)
    private bool IsFullyAccessible(bool[,] wallsMap, int currentWallsAmount)
    {
        bool[,] visitedTilesMap = new bool[wallsMap.GetLength(0), wallsMap.GetLength(1)];

        int accessibleTileCount = FloodFill(wallsMap, visitedTilesMap, currentLevel.levelCenter.x, currentLevel.levelCenter.y);

        int targetAccessibleTileCount = (int)(currentLevel.levelSize.x * currentLevel.levelSize.y - currentWallsAmount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private int FloodFill(bool[,] wallsMap, bool[,] visitedTilesMap, int x, int y)
    {
        if (x < 0 || x >= wallsMap.GetLength(0) || y < 0 || y >= wallsMap.GetLength(1) || visitedTilesMap[x, y] || wallsMap[x, y])
        {
            return 0;
        }

        visitedTilesMap[x, y] = true;

        int count = 1;

        count += FloodFill(wallsMap, visitedTilesMap, x + 1, y);
        count += FloodFill(wallsMap, visitedTilesMap, x - 1, y);
        count += FloodFill(wallsMap, visitedTilesMap, x, y + 1);
        count += FloodFill(wallsMap, visitedTilesMap, x, y - 1);

        return count;
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
        return new Vector3(-currentLevel.levelSize.x / 2 + 0.5f + x, 0, -currentLevel.levelSize.y / 2 + 0.5f + y) * tileSize;
    }
}