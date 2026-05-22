using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Prefabs")]
    public GameObject fragmentPrefab;
    public GameObject monsterPrefab;

    [Header("Spawn Settings")]
    public int fragmentCount = 5;
    public int monsterCount = 3;

    public void SpawnObjects(Dictionary<Vector2Int, TileType> mapData, Tilemap tilemap)
    {
        List<Vector2Int> spawnableTiles = new List<Vector2Int>();

        foreach (var kvp in mapData)
        {
            if (kvp.Value == TileType.Office || kvp.Value == TileType.Glitch)
            {
                spawnableTiles.Add(kvp.Key);
            }
        }

        SpawnRandomly(spawnableTiles, fragmentPrefab, fragmentCount, tilemap);
        SpawnRandomly(spawnableTiles, monsterPrefab, monsterCount, tilemap);
    }

    private void SpawnRandomly(List<Vector2Int> availableTiles, GameObject prefab, int count, Tilemap tilemap)
    {
        for (int i = 0; i < count; i++)
        {
            if (availableTiles.Count == 0) break;

            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector2Int gridPos = availableTiles[randomIndex];

            Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(gridPos.x, gridPos.y, 0));

            Instantiate(prefab, worldPos, Quaternion.identity);

            availableTiles.RemoveAt(randomIndex);
        }
    }

    public void ClearExistingObjects()
    {
        GameObject[] fragments = GameObject.FindGameObjectsWithTag("Fragment");
        foreach (GameObject f in fragments)
        {
            Destroy(f);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            Destroy(e);
        }

        Debug.Log("<color=yellow>[SpawnManager]</color> 이전 맵의 오브젝트들을 청소했습니다.");
    }
}
