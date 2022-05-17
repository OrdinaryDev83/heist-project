using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;
using System;

public class ObjectiveManager : MonoBehaviour {

    public Objective[] objs;

    public Objective escapeObj;

    public Objective[] failObjs;

    private HealthHandlerPlayer _playerHealth;

    public static ObjectiveManager I = null;
    private void Awake() {
        I = this;
        PlayersManager.OnPlayerSpawned += OnPlayerSpawned;
    }

    void OnPlayerSpawned(Transform p)
    {
        _playerHealth = p.GetComponent<HealthHandlerPlayer>();
    }

    public GameObject deathPanel;

    private void Start() {
        deathPanel.SetActive(false);
    }

    public float objectiveUpdateRate = 1f;

    float _cooldown = 0f;
    private void Update() {
        if (deathPanel.activeSelf) return;

        if(_cooldown < 1f) {
            _cooldown += Time.deltaTime * objectiveUpdateRate;
        } else {
            UpdateObjectives();
            _cooldown = 0f;
        }
    }

    public int actualObj;
    void UpdateObjectives() {
        if (actualObj < objs.Length && EvaluateObjRecursively(objs[actualObj], actualObj, objs)) {
            Debug.Log("Completed " + objs[actualObj].label);
            actualObj++;
        }else if (actualObj >= objs.Length && EvaluateObj(escapeObj)) {
            OnWinLevel();
        }

        if (_playerHealth.Health <= 0 || (failObjs.Length > 0 && EvaluateObjRecursively(failObjs[0], 0, failObjs))) {
            OnLoseLevel();
        }
    }

    public Objective GetCurrentObj() {
        if (actualObj < objs.Length) {
            return objs[actualObj];
        } else {
            return escapeObj;
        }
    }

    bool EvaluateObjRecursively(Objective obj, int actual, Objective[] collection) {
        var result = EvaluateObj(obj);
        if (!result && obj.nextOverrides) {
            var next = actual + 1;
            return next < collection.Length && EvaluateObj(collection[next]);
        }
        return result;
    }

    Objective _lastAccomplished;
    bool EvaluateObj(Objective obj) {
        foreach (var condition in obj.conditions) {
            bool total = true;

            foreach (var item in condition.properties) {
                Type targetType = condition.target.GetType();

                var fields = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                bool found = false;
                foreach (var field in fields) {
                    if (item.name == field.Name) {
                        total = total && Equal(item.type, item.value, field.GetValue(condition.target).ToString(), item.comparison);
                        found = true;
                        break;
                    }
                }
                if (found) {
                    continue;
                }
                foreach (var prop in properties) {
                    if (item.name == prop.Name) {
                        if (item.name == prop.Name) {
                            total = total && Equal(item.type, item.value, prop.GetValue(condition.target).ToString(), item.comparison);
                            found = true;
                            break;
                        }
                    }
                }
                if (!found) {
                    total = false;
                    break;
                }
            }
            if (total) {
                _lastAccomplished = obj;
                return true;
            }
        }
        return false;
    }

    bool Equal(VarProperty.Type type, string a, string b, VarProperty.Comparison comp) {
        switch (type) {
            case VarProperty.Type.Bool:
                var u = bool.Parse(a);
                var v = bool.Parse(b);

                switch (comp) {
                    case VarProperty.Comparison.Equal:
                        return u == v;
                    case VarProperty.Comparison.Different:
                        return u != v;
                    default:
                        return false;
                }
            case VarProperty.Type.Int:
                var u1 = int.Parse(a);
                var v1 = int.Parse(b);

                switch (comp) {
                    case VarProperty.Comparison.Equal:
                        return u1 == v1;
                    case VarProperty.Comparison.Different:
                        return u1 != v1;
                    case VarProperty.Comparison.Greater:
                        return u1 <= v1;
                    case VarProperty.Comparison.Smaller:
                        return u1 >= v1;
                    case VarProperty.Comparison.StrictlyGreater:
                        return u1 < v1;
                    case VarProperty.Comparison.StrictlySmaller:
                        return u1 > v1;
                    default:
                        return false;
                }
            case VarProperty.Type.Float:
                var u2 = float.Parse(a);
                var v2 = float.Parse(b);

                switch (comp) {
                    case VarProperty.Comparison.Equal:
                        return u2 == v2;
                    case VarProperty.Comparison.Different:
                        return u2 != v2;
                    case VarProperty.Comparison.Greater:
                        return u2 <= v2;
                    case VarProperty.Comparison.Smaller:
                        return u2 >= v2;
                    case VarProperty.Comparison.StrictlyGreater:
                        return u2 < v2;
                    case VarProperty.Comparison.StrictlySmaller:
                        return u2 > v2;
                    default:
                        return false;
                }
            case VarProperty.Type.String:
                switch (comp) {
                    case VarProperty.Comparison.Equal:
                        return a == b;
                    case VarProperty.Comparison.Different:
                        return a != b;
                    default:
                        return false;
                }
            case VarProperty.Type.Reference:
                switch (comp) {
                    case VarProperty.Comparison.Null:
                        return b == "null";
                    case VarProperty.Comparison.NotNull:
                        return b != "null";
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    public void OnWinLevel() {
        Debug.LogWarning("Won!");
    }

    void OnLoseLevel() {
        Debug.LogWarning("Lost!");
        PlayerUI.I.SetFailText = _lastAccomplished.label;
        deathPanel.SetActive(true);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

[System.Serializable]
public struct Objective {
    public string label;
    
    [Header("If at least one conditions true => the objective is compelted")]
    public Condition[] conditions;
    public bool nextOverrides;
}

[System.Serializable]
public struct Condition {
    public MonoBehaviour target;
    public VarProperty[] properties;
}

[System.Serializable]
public struct VarProperty {
    public string name;
    public string value;
    public Type type;
    public Comparison comparison;

    public enum Type {
        Bool,
        Int,
        Float,
        String,
        Reference
    }

    public enum Comparison {
        Equal,
        Different,
        Greater,
        Smaller,
        StrictlyGreater,
        StrictlySmaller,
        Null,
        NotNull
    }
}