using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallFader : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("기둥 벽이 그려진 타일맵")]
    public Tilemap wallTilemap;

    [Tooltip("투명해질 반경 (기본 1.5)")]
    public float checkRadius = 1.0f;

    [Tooltip("가려질 때 벽의 투명도 (0: 완전투명 ~ 1: 불투명)")]
    public float fadeAlpha = 0.3f;

    private List<Vector3Int> fadedTiles = new List<Vector3Int>();

    void Update()
    {
        if (wallTilemap == null) return;

        foreach (var pos in fadedTiles)
        {
            wallTilemap.SetTileFlags(pos, TileFlags.None);
            wallTilemap.SetColor(pos, Color.white);
        }
        fadedTiles.Clear();

        Vector3Int centerCell = wallTilemap.WorldToCell(transform.position);

        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                Vector3Int cellPos = centerCell + new Vector3Int(x, y, 0);

                if (wallTilemap.HasTile(cellPos))
                {
                    Vector3 cellWorldPos = wallTilemap.GetCellCenterWorld(cellPos);

                    if (cellWorldPos.y < transform.position.y && Vector2.Distance(transform.position, cellWorldPos) < checkRadius)
                    {
                        wallTilemap.SetTileFlags(cellPos, TileFlags.None);
                        wallTilemap.SetColor(cellPos, new Color(1, 1, 1, fadeAlpha));

                        fadedTiles.Add(cellPos);
                    }
                }
            }
        }
    }
}
