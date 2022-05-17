using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public Transform[] spawnpoints;

    private static int _quickMoney;

    public static int QuickMoney
    {
        set => _quickMoney = value;
        get => _quickMoney;
    }

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
