using UnityEngine;
using UnityEngine.Tilemaps;

namespace Codex.Map
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Scene References")]
        [Tooltip("타일이 그려질 Isometric Z as Y 타일맵 (Base_Layer)")]
        public Tilemap targetTilemap;

        [Header("Tile Data (ScriptableObjects)")]
        [Tooltip("기본 사무실 바닥 데이터")]
        public TileData officeData;
        [Tooltip("데이터 오류가 발생한 글리치 구역 데이터")]
        public TileData glitchData;

        [Header("Generation Settings")]
        public int width = 50;
        public int height = 50;

        [Range(0.01f, 0.5f)]
        public float noiseScale = 0.15f;

        [Range(0, 1)]
        public float threshold = 0.6f;

        [Header("Seed System")]
        public bool useRandomSeed = true;
        public int seed;

        [ContextMenu("Generate Map")]
        public void Generate()
        {
            if (targetTilemap == null || officeData == null || glitchData == null)
            {
                Debug.LogError("MapGenerator: 필수 데이터가 연결되지 않았습니다!");
                return;
            }

            targetTilemap.ClearAllTiles();

            if (useRandomSeed) seed = Random.Range(0, 100000);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xCoord = (float)x * noiseScale + seed;
                    float yCoord = (float)y * noiseScale + seed;
                    float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                    Vector3Int tilePos = new Vector3Int(x, y, 0);

                    if (noiseValue > threshold)
                    {
                        targetTilemap.SetTile(tilePos, glitchData.GetRandomTile());
                    }
                    else
                    {
                        targetTilemap.SetTile(tilePos, officeData.GetRandomTile());
                    }
                }
            }

            Debug.Log($"<color=cyan>The Broken Codex:</color> 맵 생성 완료! (Seed: {seed})");
        }
    }
}
