using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    [Tooltip("Object that should contain all the possible spawnpoints.")]
    public GameObject spawnHolder;

    [Tooltip("List of all the possible room configurations.")]
    public List<SpawnerConfiguration> possibleConfigurations;

    [HideInInspector]
    public bool alreadySpawned = false;

    private SpawnerConfiguration configuration;
    private List<GameObject> spawnPoints;

    private void Start() {
        spawnPoints = new List<GameObject>();
        configuration = possibleConfigurations[Random.Range(0, possibleConfigurations.Count - 1)];

        for(int i = 0; i < spawnHolder.transform.childCount; i++) {
            spawnPoints.Add(spawnHolder.transform.GetChild(i).gameObject);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (GameController.instance.debugMode)
            foreach (GameObject spawnPoint in spawnPoints)
                Gizmos.DrawCube(spawnPoint.transform.position, new Vector3(1,1,1));
    }

    public void Spawn(GameObject _player) {
        if (!alreadySpawned) {
            foreach (GameObject entity in configuration.entities) {
                GameObject enemy = Instantiate(entity);
                int index = Random.Range(0, spawnPoints.Count - 1);
                enemy.transform.position = spawnPoints[index].transform.position;
                spawnPoints.RemoveAt(index);
                enemy.GetComponent<AIController>().SetupAI();
            }

            spawnPoints.Clear();
            alreadySpawned = true;
        }
    }

}
