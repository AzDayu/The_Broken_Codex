using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;

    [Header("Tile Data (ScriptableObjects)")]
    public TileData officeData;
    public TileData glitchData;
    public TileData wallData;

    [Header("Advanced Maze Settings")]
    [Tooltip("미로의 방(칸) 개수 (예: 10x10 미로)")]
    public int mazeColumns = 10;
    public int mazeRows = 10;

    [Tooltip("길의 두께 (추천: 2)")]
    public int pathWidth = 2;
    [Tooltip("벽의 두께 (추천: 1)")]
    public int wallWidth = 1;

    private int mapWidth;
    private int mapHeight;

    [Range(0f, 1f)]
    public float glitchChance = 0.15f;

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        if (floorTilemap == null || wallTilemap == null || officeData == null || wallData == null) return;

        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        Dictionary<Vector2Int, TileType> tempMapData = new Dictionary<Vector2Int, TileType>();

        mapWidth = wallWidth + mazeColumns * (pathWidth + wallWidth);
        mapHeight = wallWidth + mazeRows * (pathWidth + wallWidth);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                tempMapData[new Vector2Int(x, y)] = TileType.Wall;
            }
        }

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        int step = pathWidth + wallWidth;

        Vector2Int startPos = new Vector2Int(wallWidth, wallWidth);
        CarveArea(startPos, startPos, tempMapData);
        stack.Push(startPos);

        while (stack.Count > 0)
        {
            Vector2Int currentPos = stack.Peek();
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos, tempMapData, step);

            if (unvisitedNeighbors.Count > 0)
            {
                Vector2Int chosenPos = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];

                CarveArea(currentPos, chosenPos, tempMapData);

                stack.Push(chosenPos);
            }
            else
            {
                stack.Pop();
            }
        }

        foreach (var kvp in tempMapData)
        {
            Vector3Int tilePos = new Vector3Int(kvp.Key.x, kvp.Key.y, 0);

            if (kvp.Value == TileType.Wall)
            {
                floorTilemap.SetTile(tilePos, officeData.GetRandomTile());
                wallTilemap.SetTile(tilePos, wallData.GetRandomTile());
            }
            else if (kvp.Value == TileType.Office)
            {
                floorTilemap.SetTile(tilePos, officeData.GetRandomTile());
            }
            else if (kvp.Value == TileType.Glitch)
            {
                floorTilemap.SetTile(tilePos, glitchData.GetRandomTile());
            }
        }

        if (MapManager.Instance != null)
        {
            MapManager.Instance.SetMapData(tempMapData, floorTilemap);
        }
    }

    private void CarveArea(Vector2Int pos1, Vector2Int pos2, Dictionary<Vector2Int, TileType> map)
    {
        int minX = Mathf.Min(pos1.x, pos2.x);
        int maxX = Mathf.Max(pos1.x, pos2.x) + pathWidth - 1;
        int minY = Mathf.Min(pos1.y, pos2.y);
        int maxY = Mathf.Max(pos1.y, pos2.y) + pathWidth - 1;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (Random.value < glitchChance)
                {
                    map[new Vector2Int(x, y)] = TileType.Glitch;
                }
                else
                {
                    map[new Vector2Int(x, y)] = TileType.Office;
                }
            }
        }
    }




    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos, Dictionary<Vector2Int, TileType> map, int step)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { new Vector2Int(0, step), new Vector2Int(0, -step), new Vector2Int(-step, 0), new Vector2Int(step, 0) };

        foreach (var dir in directions)
        {
            Vector2Int nPos = pos + dir;
            if (nPos.x >= wallWidth && nPos.x < mapWidth - wallWidth &&
                nPos.y >= wallWidth && nPos.y < mapHeight - wallWidth)
            {
                if (map[nPos] == TileType.Wall)
                {
                    neighbors.Add(nPos);
                }
            }
        }
        return neighbors;
    }
}