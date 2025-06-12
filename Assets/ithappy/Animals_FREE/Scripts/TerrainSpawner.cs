using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    [Header("Prefab à faire apparaître")]
    [SerializeField] private GameObject prefabToSpawn;

    [Header("Nombre d’instances")]
    [SerializeField] private int spawnCount = 10;

    [Header("Zone de spawn (centrée sur le terrain)")]
    [SerializeField] private float margin = 5f; // évite de spawner trop près du bord

    private Terrain terrain;

    private void Start()
    {
        terrain = Terrain.activeTerrain;

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnOnTerrain();
        }
    }

    private void SpawnOnTerrain()
    {
        Vector3 terrainPos = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        float x = Random.Range(margin, terrainSize.x - margin);
        float z = Random.Range(margin, terrainSize.z - margin);
        float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

        Vector3 spawnPoint = new Vector3(x + terrainPos.x, y, z + terrainPos.z);
        Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
    }
}
