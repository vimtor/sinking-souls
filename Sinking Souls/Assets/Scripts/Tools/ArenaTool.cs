using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArenaTool : EditorWindow {

    public GameObject enemy;
    public SpawnerConfiguration configuration;
    public GameObject spawnHolder;
    public GameObject room;

    private int selected;
    private string[] options = new string[] { "Configuration", "Enemies" };
    private List<GameObject> spawnPoints;
    private GUISkin skin;

    [MenuItem("Window/Arena Controller")]
    public static void ShowWindow() {
        var window = GetWindow<ArenaTool>("Arena");
        window.minSize = new Vector2(420, 370);
        window.maxSize = new Vector2(420, 370);
    }

    private void OnEnable() {
        skin = Resources.Load<GUISkin>("GUIStylesheet");
        spawnPoints = new List<GameObject>();
    }

    private void OnGUI() {

        GUILayout.Label("Arena Controller", skin.GetStyle("Header"));
        GUILayout.Label("Select spawnmode and spawnpoints and press spawn to make it appear. \n" +
                        "If needed press the clear button to remove all enemies from the scene.", skin.GetStyle("Text"));

        GUILayout.Label("Configure", skin.GetStyle("SubHeader"));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spawn Points");
        EditorGUI.BeginChangeCheck();
        spawnHolder = (GameObject)EditorGUILayout.ObjectField(spawnHolder, typeof(GameObject), true);
        if (EditorGUI.EndChangeCheck()) {
            spawnPoints.Clear();

            for (int i = 0; i < spawnHolder.transform.childCount; i++) {
                spawnPoints.Add(spawnHolder.transform.GetChild(i).gameObject);
            }
        }
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
                    if (GUILayout.Button("Spawn")) SpawnConfiguration();
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

                    if (GUILayout.Button("Spawn")) SpawnEnemy();
                }

                break;
        }

        if (GUILayout.Button("Clean Arena")) CleanArena();

        
        GUILayout.Label("Select Room", skin.GetStyle("SubHeader"));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Room Prefab");
        room = (GameObject)EditorGUILayout.ObjectField(room, typeof(GameObject), false);
        GUILayout.EndHorizontal();

        GUILayout.Space(10.0f);

        if (room == null) {
            EditorGUILayout.HelpBox("Room prefab cannot be null.", MessageType.Error);
        }
        else {
            if (!room.CompareTag("Room")) {
                EditorGUILayout.HelpBox(room.name + " doesn't seems to be a room. Check the tag to make sure.", MessageType.Error);
            }
            else {
                if (GUILayout.Button("Change Room")) {
                    GameObject currentRoom = GameObject.Find("Arena").transform.GetChild(0).gameObject;
                    GameObject newRoom = Instantiate(room);
                    newRoom.transform.position = currentRoom.transform.position;
                    newRoom.transform.SetParent(GameObject.Find("Arena").transform);
                    newRoom.transform.Find("NavMesh").gameObject.SetActive(true);
                    DestroyImmediate(currentRoom);
                }
            }
        }
    }

    private void SpawnEnemy() {
        GameObject instantiated = Instantiate(enemy);
        instantiated.transform.position = spawnHolder.transform.GetChild(0).position;
        instantiated.GetComponent<AIController>().SetupAI();
    }

    private void SpawnConfiguration() {
        foreach (GameObject entity in configuration.entities) {
            GameObject enemy = Instantiate(entity);
            int index = Random.Range(0, spawnPoints.Count - 1);
            enemy.transform.position = spawnPoints[index].transform.position;
            enemy.GetComponent<AIController>().SetupAI();
        }
    }

    private void CleanArena() {
        var spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject spawnedEnemy in spawnedEnemies) {
            DestroyImmediate(spawnedEnemy);
        }
    }

}