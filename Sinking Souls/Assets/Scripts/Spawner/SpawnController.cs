using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public List<GameObject> spawnPoints;
    public List<SpawnerConfiguration> possibleConfigurations;

    private SpawnerConfiguration configuration;

    private void Start() {
        configuration = possibleConfigurations[Random.Range(0, possibleConfigurations.Count - 1)];
        Spawn();
    }

    public void Spawn() {
        foreach(GameObject entity in configuration.entities) {
            GameObject enemy = Instantiate(entity);
            int index = Random.Range(0, spawnPoints.Count - 1);
            Debug.Log(index);
            enemy.transform.position = spawnPoints[index].transform.position;
            spawnPoints.RemoveAt(index);
            enemy.GetComponent<AIController>().SetupAI();
        }
        spawnPoints.Clear();
    }

}
