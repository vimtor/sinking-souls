using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArenaTool : EditorWindow {

    GUISkin skin;

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

    private void OnEnable() {
        skin = Resources.Load<GUISkin>("GUIStylesheet");
    }

    private void OnGUI() {

        GUILayout.Label("Arena Controller", skin.GetStyle("Header"));
        GUILayout.Label("Select spawnmode and spawnpoints and press spawn to make it appear. \n" +
                        "If needed press the clear button to remove all enemies from the scene.", skin.GetStyle("Text"));

        GUILayout.Label("Configure", skin.GetStyle("SubHeader"));

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
                GUILayout.BeginHorizontal();
                GUILayout.Label("Configuration");
                configuration = (SpawnerConfiguration)EditorGUILayout.ObjectField(configuration, typeof(SpawnerConfiguration), false);
                GUILayout.EndHorizontal();

                GUILayout.Space(10.0f);

                if (configuration == null) {
                    EditorGUILayout.HelpBox("Configuration cannot be null.", MessageType.Error);
                }
                else {
                    if (GUILayout.Button("Spawn")) {
                        for (int i = 0; i < spawnHolder.transform.childCount; i++) {
                            spawnPoints.Add(spawnHolder.transform.GetChild(i).gameObject);
                        }

                        foreach (GameObject entity in configuration.entities) {
                            GameObject enemy = Instantiate(entity);
                            int index = Random.Range(0, spawnPoints.Count - 1);
                            enemy.transform.position = spawnPoints[index].transform.position;
                            enemy.GetComponent<AIController>().SetupAI();
                        }
                    }
                }

                break;

            case 1:
                GUILayout.BeginHorizontal();
                GUILayout.Label("Enemy Prefab");
                enemy = (GameObject)EditorGUILayout.ObjectField(enemy, typeof(GameObject), false);
                GUILayout.EndHorizontal();

                GUILayout.Space(10.0f);

                if (enemy == null) {
                    EditorGUILayout.HelpBox("Enemy prefab cannot be null.", MessageType.Error);
                }
                else {
                    if (!enemy.CompareTag("Enemy")) {
                        EditorGUILayout.HelpBox(enemy.name + " doesn't seems to be an enemy. Check the tag to make sure.", MessageType.Warning);
                        GUILayout.Space(10.0f);
                    }

                    if (GUILayout.Button("Spawn")) {
                        GameObject instantiated = Instantiate(enemy);
                        instantiated.transform.position = spawnHolder.transform.GetChild(0).position;
                        instantiated.GetComponent<AIController>().SetupAI();
                    }
                }

                break;
        }

        if (GUILayout.Button("Clean Arena")) {
            var spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject spawnedEnemy in spawnedEnemies) {
                DestroyImmediate(spawnedEnemy);
            }
        }
    }

}
