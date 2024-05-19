using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCometSpawner : MonoBehaviour
{
    public Transform cometTarget;
    public Transform spawnTarget;
    public List<GameObject> cometPrefabs;
    
    public float timeToSpawn = 5;
    public float acceleration = 0.1f;

    public float cometHeight;
    public float cometSpawnRadius;

    private void Start()
    {
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        
        Vector3 cometSpawnPosition = cometTarget.position + Vector3.up * cometHeight;
        Vector3 unitSphereRandom = Random.insideUnitSphere;
        unitSphereRandom.y = Mathf.Abs(unitSphereRandom.y);
        cometSpawnPosition += unitSphereRandom * cometSpawnRadius;
        
        EnemyComet comet = Instantiate(cometPrefabs[Random.Range(0, cometPrefabs.Count)], cometSpawnPosition, 
            Quaternion.identity).GetComponent<EnemyComet>();
        comet.SpawnComet(cometTarget.position, spawnTarget.position);
        
        Invoke("Start", timeToSpawn - timeToSpawn * acceleration);
    }
}
