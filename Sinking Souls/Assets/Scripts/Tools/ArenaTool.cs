using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArenaTool : EditorWindow {

    public GameObject enemy;
    public SpawnerConfiguration configuration;
    public GameObject spawnHolder;


    private int selected;
    private string[] options = new string[] { "Configuration", "Enemies" };
    private List<GameObject> spawnPoints;

    [MenuItem("Window/Arena Controller")]
    public static void ShowWindow() {
        GetWindow<ArenaTool>("Arena");
    }

    private void OnGUI() {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spawn Points");
        spawnHolder = (GameObject)EditorGUILayout.ObjectField(spawnHolder, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spawn Mode");
        selected = EditorGUILayout.Popup(selected, options);
        GUILayout.EndHorizontal();

        switch (selected) {
            case 0:
                configuration = (SpawnerConfiguration)EditorGUILayout.ObjectField(configuration, typeof(SpawnerConfiguration), false);
                break;

            case 1:
                enemy = (GameObject)EditorGUILayout.ObjectField(enemy, typeof(GameObject), false);
                break;
        }


        if (GUILayout.Button("Spawn")) {
            switch (selected) {
                case 0:
                    for (int i = 0; i < spawnHolder.transform.childCount; i++) {
                        spawnPoints.Add(spawnHolder.transform.GetChild(i).gameObject);
                    }

                    foreach (GameObject entity in configuration.entities) {
                        GameObject enemy = Instantiate(entity);
                        int index = Random.Range(0, spawnPoints.Count - 1);
                        enemy.transform.position = spawnPoints[index].transform.position;
                        enemy.GetComponent<AIController>().SetupAI();
                    }
                    break;

                case 1:
                    GameObject instantiated = Instantiate(enemy);
                    instantiated.transform.position = spawnHolder.transform.GetChild(0).position;
                    instantiated.GetComponent<AIController>().SetupAI();
                    break;
            }
        }
    }

}
