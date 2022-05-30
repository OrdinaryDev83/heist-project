using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public Transform[] spawnpoints;
    
    #region Stats
    private static int _quickMoney;
    public static int QuickMoney
    {
        set => _quickMoney = value;
        get => _quickMoney;
    }

    private static int _kills;
    public static int Kills
    {
        set => _kills = value;
        get => _kills;
    }

    private static int _hostageKilled;

    public static int HostageKilled
    {
        set => _hostageKilled = value;
        get => _hostageKilled;
    }

    #endregion

    public delegate void OnPlayerSpawnedDelegate(Transform player);
    public static OnPlayerSpawnedDelegate OnPlayerSpawned;


    private void Start()
    {
        int r = Random.Range(0, spawnpoints.Length);
        var player = Instantiate(playerPrefab, spawnpoints[r].position, Quaternion.identity, transform);
        OnPlayerSpawned?.Invoke(player.transform);
    }
    
    public static void AddQuickMoney(int amount)
    {
        QuickMoney += amount;
    }
}
