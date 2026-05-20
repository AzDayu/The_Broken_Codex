using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Scene References")]
    [Tooltip("타일이 그려질 Isometric Z as Y 타일맵 (Base_Layer)")]
    public Tilemap targetTilemap;

    [Header("Tile Data (ScriptableObjects)")]
    public TileData officeData;
    public TileData glitchData;

    [Header("Generation Settings")]
    public int width = 50;
    public int height = 50;

    [Range(0.01f, 0.5f)] public float noiseScale = 0.15f;
    [Range(0, 1)] public float threshold = 0.6f;

    [Header("Seed System")]
    public bool useRandomSeed = true;
    public int seed;

    private void Start()
    {
        Generate();
    }

    [ContextMenu("Generate Map")]
    public void Generate()
    {
        if (targetTilemap == null || officeData == null || glitchData == null)
        {
            Debug.LogError("MapGenerator: 필수 데이터가 연결되지 않았습니다!");
            return;
        }

        targetTilemap.ClearAllTiles();

        Dictionary<Vector2Int, TileType> tempMapData = new Dictionary<Vector2Int, TileType>();

        if (useRandomSeed) seed = Random.Range(0, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x * noiseScale + seed;
                float yCoord = (float)y * noiseScale + seed;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Vector2Int gridPos = new Vector2Int(x, y);

                if (noiseValue > threshold)
                {
                    targetTilemap.SetTile(tilePos, glitchData.GetRandomTile());
                    tempMapData[gridPos] = glitchData.tileType;
                }
                else
                {
                    targetTilemap.SetTile(tilePos, officeData.GetRandomTile());
                    tempMapData[gridPos] = officeData.tileType;
                }
            }
        }

        if (MapManager.Instance != null)
        {
            MapManager.Instance.SetMapData(tempMapData, targetTilemap);
            Debug.Log("<color=cyan>[MapGenerator]</color> MapManager에게 데이터 배달 완료!");
        }
        else
        {
            Debug.LogError("MapGenerator: 씬에 MapManager 오브젝트를 찾을 수 없습니다!");
        }
    }
}