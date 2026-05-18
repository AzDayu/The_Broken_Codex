using UnityEngine;
using UnityEngine.Tilemaps;

namespace Codex.Map
{
    /// <summary>
    /// The Broken Codex: 파편화된 세계 생성을 담당하는 핵심 클래스
    /// </summary>
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
        public float noiseScale = 0.15f; // 노이즈 밀도: 낮을수록 덩어리가 커집니다.

        [Range(0, 1)]
        public float threshold = 0.6f;   // 글리치 비율: 높을수록 글리치 타일이 적게 생성됩니다.

        [Header("Seed System")]
        public bool useRandomSeed = true;
        public int seed;

        /// <summary>
        /// 인스펙터의 컴포넌트 메뉴(점 세 개)에서 'Generate Map'을 클릭하면 즉시 실행됩니다.
        /// </summary>
        [ContextMenu("Generate Map")]
        public void Generate()
        {
            if (targetTilemap == null || officeData == null || glitchData == null)
            {
                Debug.LogError("MapGenerator: 필수 데이터가 연결되지 않았습니다!");
                return;
            }

            // 1. 기존 맵 데이터 초기화
            targetTilemap.ClearAllTiles();

            // 2. 시드 설정
            if (useRandomSeed) seed = Random.Range(0, 100000);

            // 3. 맵 루프 실행
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // 펄린 노이즈 계산 (시드를 더해 매번 다른 맵 생성)
                    float xCoord = (float)x * noiseScale + seed;
                    float yCoord = (float)y * noiseScale + seed;
                    float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                    // Isometric Z as Y 환경에서 바닥은 동일 평면(z=0)에 배치
                    Vector3Int tilePos = new Vector3Int(x, y, 0);

                    // 4. 노이즈 임계값(Threshold)에 따른 타일 결정
                    if (noiseValue > threshold)
                    {
                        // 글리치 구역 배치
                        targetTilemap.SetTile(tilePos, glitchData.tileAsset);
                    }
                    else
                    {
                        // 일반 사무실 구역 배치
                        targetTilemap.SetTile(tilePos, officeData.tileAsset);
                    }
                }
            }

            Debug.Log($"<color=cyan>The Broken Codex:</color> 맵 생성 완료! (Seed: {seed})");
        }
    }
}
