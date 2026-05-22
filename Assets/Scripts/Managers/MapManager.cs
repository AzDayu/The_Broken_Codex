using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [HideInInspector]
    public Tilemap targetTilemap;

    private Dictionary<Vector2Int, TileType> tileDataDict = new Dictionary<Vector2Int, TileType>();

    public int debugOfficeTileCount;
    public int debugGlitchTileCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetMapData(Dictionary<Vector2Int, TileType> generatedData, Tilemap tilemap)
    {
        tileDataDict = new Dictionary<Vector2Int, TileType>(generatedData);
        targetTilemap = tilemap;
        //debugOfficeTileCount = tileDataDict.

        //Debug.Log($"<color=green>[MapManager]</color> 성공적으로 {debugOfficeTileCount}개의 타일 데이터를 전달받아 저장했습니다.");
    }

    public Dictionary<Vector2Int, TileType> GetMapData()
    {
        return tileDataDict;
    }

    public TileType GetTileUnderPosition(Vector3 worldPosition)
    {
        if (targetTilemap == null) return TileType.None;

        Vector3Int cellPosition = targetTilemap.WorldToCell(worldPosition);
        Vector2Int gridPos = new Vector2Int(cellPosition.x, cellPosition.y);

        if (tileDataDict.TryGetValue(gridPos, out TileType type))
        {
            return type;
        }
        return TileType.None;
    }
}
