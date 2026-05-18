using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileData", menuName = "Codex/TileData")]
public class TileData : ScriptableObject
{
    public string tileName;
    public TileBase tileAsset;
    public bool isWalkable = true;
}