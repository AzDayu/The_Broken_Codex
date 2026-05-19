using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileData", menuName = "Codex/TileData")]
public class TileData : ScriptableObject
{
    public string tileName;
    public List<TileBase> variations;
    public bool isWalkable = true;

    public TileBase GetRandomTile()
    {
        if (variations == null || variations.Count == 0) return null;

        int randomIndex = Random.Range(0, variations.Count);
        return variations[randomIndex];
    }
}