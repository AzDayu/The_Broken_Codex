using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [HideInInspector]
    public Tilemap _targetTilemap;

    private Dictionary<Vector2Int, TileType> _tileDataDict = new Dictionary<Vector2Int, TileType>();

    public int _debugOfficeTileCount;
    public int _debugGlitchTileCount;

    public Vector2Int _mapCenter;


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

    public void SetMapData(Dictionary<Vector2Int, TileType> generatedData, Tilemap tilemap , Vector2Int mapCenter)
    {
        _tileDataDict = new Dictionary<Vector2Int, TileType>(generatedData);
        _targetTilemap = tilemap;
        _mapCenter = mapCenter;
    }

    public Dictionary<Vector2Int, TileType> GetMapData()
    {
        return _tileDataDict;
    }

    public TileType GetTileUnderPosition(Vector3 worldPosition)
    {
        if (_targetTilemap == null) return TileType.None;

        Vector3Int cellPosition = _targetTilemap.WorldToCell(worldPosition);
        Vector2Int gridPos = new Vector2Int(cellPosition.x, cellPosition.y);

        if (_tileDataDict.TryGetValue(gridPos, out TileType type))
        {
            return type;
        }
        return TileType.None;
    }
}
