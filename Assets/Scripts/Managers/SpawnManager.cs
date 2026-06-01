using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [Header("Data Setup")]
    public TextAsset monsterDataJson;
    private Dictionary<string, MonsterData> monsterDB = new Dictionary<string, MonsterData>();

    [Header("Prefabs")]
    public GameObject shardPrefab;
    public GameObject bossPrefab;
    public GameObject[] normalMonsters;

    [Header("Spawn Settings")]
    public int shardCount = 5;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Awake()
    {
        if (monsterDataJson != null)
        {
            MonsterDatabase db = JsonUtility.FromJson<MonsterDatabase>(monsterDataJson.text);
            foreach (MonsterData data in db.monsters)
            {
                monsterDB.Add(data.id, data);
            }
            Debug.Log($"총 {monsterDB.Count}개의 몬스터 데이터를 성공적으로 로드했습니다.");
        }
        else
        {
            Debug.LogError("MonsterData JSON 파일이 연결되지 않았습니다! 인스펙터를 확인해 주세요.");
        }
    }

    public void SpawnObjects(Dictionary<Vector2Int, TileType> mapData, Tilemap targetTilemap)
    {
        SpawnRandomObjects(shardPrefab, shardCount, mapData, targetTilemap);
        SpawnRandomMonsters(shardCount, mapData, targetTilemap);
        SpawnBoss(targetTilemap);
    }

    private void SpawnRandomObjects(GameObject prefab, int count, Dictionary<Vector2Int, TileType> mapData, Tilemap tilemap)
    {
        List<Vector2Int> walkableTiles = GetWalkableTiles(mapData);

        for (int i = 0; i < count; i++)
        {
            if (walkableTiles.Count == 0) break;

            int randomIndex = Random.Range(0, walkableTiles.Count);
            Vector2Int spawnGridPos = walkableTiles[randomIndex];
            walkableTiles.RemoveAt(randomIndex);

            Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(spawnGridPos.x, spawnGridPos.y, 0));
            GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity, transform);
            spawnedObjects.Add(obj);
        }
    }

    private void SpawnRandomMonsters(int count, Dictionary<Vector2Int, TileType> mapData, Tilemap tilemap)
    {
        List<Vector2Int> walkableTiles = GetWalkableTiles(mapData);

        for (int i = 0; i < count; i++)
        {
            if (walkableTiles.Count == 0) break;

            int randomIndex = Random.Range(0, walkableTiles.Count);
            Vector2Int spawnGridPos = walkableTiles[randomIndex];
            walkableTiles.RemoveAt(randomIndex);

            Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(spawnGridPos.x, spawnGridPos.y, 0));

            GameObject randomMonsterPrefab = normalMonsters[Random.Range(0, normalMonsters.Length)];
            GameObject obj = Instantiate(randomMonsterPrefab, worldPos, Quaternion.identity, transform);
            spawnedObjects.Add(obj);

            InjectDataToMonster(obj);
        }
    }

    private void SpawnBoss(Tilemap tilemap)
    {
        if (bossPrefab == null || MapManager.Instance == null) return;

        Vector2Int centerGridPos = MapManager.Instance._mapCenter;
        Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(centerGridPos.x, centerGridPos.y, 0));

        GameObject boss = Instantiate(bossPrefab, worldPos, Quaternion.identity, transform);
        spawnedObjects.Add(boss);

        InjectDataToMonster(boss);
        Debug.Log("보스 스폰 완료 및 데이터 주입 완료: " + centerGridPos);
    }

    private void InjectDataToMonster(GameObject monsterObj)
    {
        Monster monster = monsterObj.GetComponent<Monster>();
        if (monster != null)
        {
            if (monsterDB.ContainsKey(monster.monsterID))
            {
                monster.Initialize(monsterDB[monster.monsterID]);
            }
            else
            {
                Debug.LogWarning($"{monsterObj.name}의 ID({monster.monsterID})와 일치하는 JSON 데이터가 없습니다!");
            }
        }
    }

    private List<Vector2Int> GetWalkableTiles(Dictionary<Vector2Int, TileType> mapData)
    {
        List<Vector2Int> walkables = new List<Vector2Int>();
        foreach (var kvp in mapData)
        {
            if (kvp.Value == TileType.Office || kvp.Value == TileType.Glitch || kvp.Value == TileType.Restored)
            {
                walkables.Add(kvp.Key);
            }
        }
        return walkables;
    }

    public void ClearExistingObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }
}