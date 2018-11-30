using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AICreatorTool : EditorWindow {

    public class ActionField {
        public bool selected;
        public string action;

        public ActionField(string action) {
            selected = false;
            this.action = action;
        }
    };

    public class DecisionField {
        public bool selected;
        public string decision;

        public DecisionField(string decision) {
            selected = false;
            this.decision = decision;
        }
    };

    public string aiName;
    public Dictionary<string, DecisionField> decisions;
    public Dictionary<string, ActionField> actions;
    public int numberStates;

    private GUISkin skin;

    [MenuItem("Window/AI Creator")]
    public static void ShowWindow() {
        var window = GetWindow<AICreatorTool>("AI Creator");
        window.minSize = new Vector2(420, 350);
        window.maxSize = new Vector2(420, 350);
    }

    private void OnEnable() {
        aiName = "";
        decisions = new Dictionary<string, DecisionField>();
        actions = new Dictionary<string, ActionField>();

        decisions["Animation"]  = new DecisionField("AnimationDecision");
        decisions["Longer"]     = new DecisionField("DistanceLongerDecision");
        decisions["Shorter"]    = new DecisionField("DistanceShorterDecision");
        decisions["Hit"]        = new DecisionField("HitDecision");
        decisions["InSight"]    = new DecisionField("InSightDecision");
        decisions["Look"]       = new DecisionField("LookDecision");
        decisions["Time"]       = new DecisionField("TimeDecision");

        actions["Attack"]       = new ActionField("AttackAction");
        actions["Chase"]        = new ActionField("ChaseAction");
        actions["Idle"]         = new ActionField("IdleAction");
        actions["Hit"]          = new ActionField("HitAction");
        actions["Look"]         = new ActionField("LookAction");
        actions["Spell"]        = new ActionField("SpellAction");
        actions["Teleport"]     = new ActionField("TeleportAction");
        actions["Turn"]         = new ActionField("TurnAction");

        skin = Resources.Load<GUISkin>("GUIStylesheet");
    }

    private void OnGUI () {

        GUILayout.Label("AI Creator", skin.GetStyle("Header"));

        GUILayout.BeginHorizontal();
        GUILayout.Label("AI Name");
        aiName = GUILayout.TextField(aiName, 30);
        GUILayout.EndHorizontal();

        #region Decisions
        GUILayout.Label("Decisions", skin.GetStyle("SubHeader"));

        List<string> keys = new List<string>(decisions.Keys);
        int i = 1;
        foreach (string key in keys) {
            if(i == 1) GUILayout.BeginHorizontal();

            decisions[key].selected = EditorGUILayout.ToggleLeft(key, decisions[key].selected, GUILayout.Width(100f));

            if (i == 4) {
                GUILayout.EndHorizontal();
                i = 0;
            }

            i++;
        }
        if (keys.Count % 4 != 0) GUILayout.EndHorizontal();
        #endregion

        #region Actions
        GUILayout.Label("Actions", skin.GetStyle("SubHeader"));

        keys = new List<string>(actions.Keys);
        i = 1;
        foreach (string key in keys) {
            if (i == 1) GUILayout.BeginHorizontal();

            actions[key].selected = EditorGUILayout.ToggleLeft(key, actions[key].selected, GUILayout.Width(100f));

            if (i == 4) {
                GUILayout.EndHorizontal();
                i = 0;
            }

            i++;
        }
        if(keys.Count % 4 != 0) GUILayout.EndHorizontal();
        #endregion

        GUILayout.Label("States", skin.GetStyle("SubHeader"));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Nº States");
        numberStates = EditorGUILayout.IntField(numberStates);
        GUILayout.EndHorizontal();

        GUILayout.Space(10.0f);

        if (aiName == "" || aiName == null || aiName == " ") {
            EditorGUILayout.HelpBox("Enemy name cannot be null.", MessageType.Error);
        }
        else {

            string rootPath = "Assets/Scriptable Objects/PlugableAI";
            string enemyPath = rootPath + "/" + aiName;

            if (AssetDatabase.IsValidFolder(enemyPath)) {
                EditorGUILayout.HelpBox("Seems that this enemy already exists.", MessageType.Warning);
                GUILayout.Space(10.0f);
            }
            
            if(GUILayout.Button("Create AI")) {
                AssetDatabase.CreateFolder(rootPath, aiName);
                AssetDatabase.CreateFolder(enemyPath, "Actions");
                AssetDatabase.CreateFolder(enemyPath, "Decisions");
                AssetDatabase.CreateFolder(enemyPath, "States");

                foreach (var action in actions)
                {
                    if (action.Value.selected)
                    {
                        string fileName = enemyPath + "/Actions/" + aiName.Replace(" ", "") + "_" + action.Key + "Action" + ".asset";
                        AssetDatabase.CreateAsset(CreateInstance(action.Value.action), fileName);
                    }
                }

                foreach (var decision in decisions)
                {
                    if (decision.Value.selected)
                    {
                        string fileName = enemyPath + "/Decisions/" + aiName.Replace(" ", "") + "_" + decision.Key + "Decision" + ".asset";
                        AssetDatabase.CreateAsset(CreateInstance(decision.Value.decision), fileName);
                    }
                }

                for (i = 0; i < numberStates; i++)
                {
                    string fileName = enemyPath + "/States/" + aiName.Replace(" ", "") + "_" + i.ToString() + "State" + ".asset";
                    AssetDatabase.CreateAsset(CreateInstance("State"), fileName);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();


            }
        }
    }
}
