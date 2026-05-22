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

    [Header("Maze Settings")]
    public int width = 31;
    public int height = 31;

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        if (floorTilemap == null || wallTilemap == null || officeData == null || wallData == null)
        {
            Debug.LogError("MapGenerator: 필수 데이터나 타일맵이 연결되지 않았습니다!");
            return;
        }

        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        Dictionary<Vector2Int, TileType> tempMapData = new Dictionary<Vector2Int, TileType>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tempMapData[new Vector2Int(x, y)] = TileType.Wall;
            }
        }

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int startPos = new Vector2Int(1, 1);
        tempMapData[startPos] = TileType.Office;
        stack.Push(startPos);

        while (stack.Count > 0)
        {
            Vector2Int currentPos = stack.Peek();
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos, tempMapData);

            if (unvisitedNeighbors.Count > 0)
            {
                Vector2Int chosenPos = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];

                Vector2Int wallToBreak = currentPos + (chosenPos - currentPos) / 2;
                tempMapData[wallToBreak] = TileType.Office;
                tempMapData[chosenPos] = TileType.Office;

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
        }

        if (MapManager.Instance != null)
        {
            MapManager.Instance.SetMapData(tempMapData, floorTilemap);
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos, Dictionary<Vector2Int, TileType> map)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { new Vector2Int(0, 2), new Vector2Int(0, -2), new Vector2Int(-2, 0), new Vector2Int(2, 0) };

        foreach (var dir in directions)
        {
            Vector2Int nPos = pos + dir;
            if (nPos.x > 0 && nPos.x < width - 1 && nPos.y > 0 && nPos.y < height - 1)
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