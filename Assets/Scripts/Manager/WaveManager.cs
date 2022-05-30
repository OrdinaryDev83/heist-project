using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager I = null;

    [SerializeField]
    private float pauseTime = 30f;
    [SerializeField]
    private float assaultTime = 300f;

    [SerializeField]
    private int wave;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public Difficulty difficulty;

    [SerializeField]
    private Transform[] spawnPoints;
    public Transform[] GetSpawnPoints
    {
        get
        {
            return spawnPoints;
        }
    }

    [SerializeField]
    private int killsRemaining;

    [SerializeField]
    private AnimationCurve spawnCountCurve;

    [SerializeField]
    private GameObject[] enemiesPrefab;

    private Transform _player;

    private void Awake()
    {
        I = this;
        wave = 1;
        killsRemaining = -1;
        PlayersManager.OnPlayerSpawned += OnPlayerSpawned;
        _respawnQueue = new Queue<float>();
    }

    void OnPlayerSpawned(Transform pl)
    {
        _player = pl;
    }

    public enum AssaultState
    {
        Pause,
        Running
    }
    [SerializeField]
    private AssaultState state = AssaultState.Pause;
    public AssaultState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }

    [SerializeField]
    float cooldown = 0f;

    // FIXME FAIRE EN SORTE QUE CA SPAWN AU FUR ET A MESURE PAS D'UN COUP MERCi
    // + POOLING

    Queue<float> _respawnQueue; // pour avoir un nombre presque constant d'ennemis sur la carte

    float _respawnCooldown = -1f;
    private void Update()
    {
        if (_respawnQueue.Count != 0)
        {
            if (_respawnCooldown <= 0f)
            {
                float timePassed = Time.time - _respawnQueue.Dequeue();
                _respawnCooldown = Mathf.Clamp(enemyRespawnCooldown - timePassed, 0f, enemyRespawnCooldown);
                SpawnEnemy(0, true);
            }
            else
            {
                _respawnCooldown -= Time.deltaTime;
            }
        }

        switch (state)
        {
            case AssaultState.Pause:
                {
                    PlayerUI.I.SetAssaultState(state, cooldown);
                    if (cooldown > 0f)
                        cooldown -= Time.deltaTime;
                    else if (cooldown < 0f)
                    {
                        cooldown = assaultTime;
                        SpawnNewWave();
                        State = AssaultState.Running;
                    }
                    break;
                }
            case AssaultState.Running:
                {
                    PlayerUI.I.SetAssaultState(state, cooldown);
                    if (cooldown > 0f)
                        cooldown -= Time.deltaTime;
                    if (cooldown < 0f) // FIXME proportional number of fleeing cops
                    {
                        killsRemaining = -1;
                        cooldown = pauseTime;
                        State = AssaultState.Pause;
                        _respawnQueue.Clear();
                        _respawnCooldown = 0f;
                    }
                    break;
                }
        }
    }

    [SerializeField]
    private float enemyRespawnCooldown;
    void OneEnemyDied()
    {
        if (State == AssaultState.Running)
        {
            killsRemaining -= 1;
            PlayersManager.Kills++;
            _respawnQueue.Enqueue(Time.time);
        }
    }

    public void SpawnNewWave()
    {
        int n = Mathf.CeilToInt(spawnCountCurve.Evaluate((float)wave));
        for (int i = 0; i < n; i++)
        {
            SpawnEnemy(0);
        }
    }

    public void SpawnEnemy(int id, bool forceAssault = false)
    {
        int r = Random.Range(0, spawnPoints.Length);

        var e = Instantiate(enemiesPrefab[id], spawnPoints[r].position, Quaternion.identity, transform);
        ControllerPolice controllerPolice = e.GetComponent<ControllerPolice>();
        if (forceAssault)
            controllerPolice.type = ControllerPolice.Type.Attacking;
        controllerPolice.GetComponent<HealthHandler>().OnDeathEvent += OneEnemyDied;
        killsRemaining++;
    }
}
