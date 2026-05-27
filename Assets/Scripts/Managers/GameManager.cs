using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    public MapGenerator mapGenerator;
    public SpawnManager spawnManager;
    public CinemachineCamera cinemachineCamera;
    public GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        if (spawnManager != null)
        {
            spawnManager.ClearExistingObjects();
        }

        Debug.Log("1. 맵 생성을 시작합니다.");
        mapGenerator.GenerateMap();

        if (MapManager.Instance != null)
        {
            Dictionary<Vector2Int, TileType> currentMapData = MapManager.Instance.GetMapData();
            Tilemap currentTilemap = MapManager.Instance.targetTilemap;

            Debug.Log("2. 파편 및 몬스터 스폰을 시작합니다.");
            spawnManager.SpawnObjects(currentMapData, currentTilemap);

            if (player != null && currentTilemap != null)
            {
                int startPosOffset = mapGenerator.wallWidth;
                Vector3 startWorldPos = currentTilemap.GetCellCenterWorld(new Vector3Int(startPosOffset, startPosOffset, 0));

                player.transform.position = startWorldPos;
                cinemachineCamera.Follow = player.transform;
                Debug.Log("3. 플레이어를 시작 위치(1, 1)로 재배치했습니다.");
            }
        }
        else
        {
            Debug.LogError("GameManager: MapManager를 찾을 수 없어 초기화에 실패했습니다.");
            return;
        }

        Debug.Log("스테이지 세팅 완료! 새로운 구역이 시작되었습니다.");


    }

    public void GoToNextStage()
    {
        Debug.Log("<color=cyan><b>[GameManager] 다음 스테이지로 이동합니다!</b></color>");
        InitGame();
    }
}