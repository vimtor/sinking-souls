using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public List<GameObject> spawnPoints;
    public List<SpawnerConfiguration> possibleConfigurations;
    public bool alreadySpawned = false;

    public SpawnerConfiguration configuration;

    private void Start() {
        configuration = possibleConfigurations[Random.Range(0, possibleConfigurations.Count - 1)];
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (GameController.instance.debugMode)
            foreach (GameObject spawnPoint in spawnPoints)
                Gizmos.DrawCube(spawnPoint.transform.position, new Vector3(1,1,1));
    }

    public void Spawn(GameObject _player) {
        if (!alreadySpawned) {
            Debug.Log(configuration);
            foreach (GameObject entity in configuration.entities) {
                GameObject enemy = Instantiate(entity);
                int index = Random.Range(0, spawnPoints.Count - 1); Debug.Log("4");
                enemy.transform.position = spawnPoints[index].transform.position;
                spawnPoints.RemoveAt(index);
                enemy.GetComponent<AIController>().SetupAI(_player);
            }
            spawnPoints.Clear();
            alreadySpawned = true;
        }
    }

}
