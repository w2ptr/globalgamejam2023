using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalGameJam2023;
using static GlobalGameJam2023.Powerup;

public class PowerupController : MonoBehaviour
{
    public int spawnPeriod = 10000;
    public float spawnHeight = 15.0f;
    public float spawnRadius = 10.0f;
    public List<GameObject> powerupPrefabs;

    private int time = 0;
    private List<PowerupType> choices;

    void Start()
    {
        choices = new List<PowerupType>() {
            PowerupType.AddWater,
            PowerupType.AddWater,
            PowerupType.AddWater,
            PowerupType.DestroyBranch,
            PowerupType.SplitBranch,
        };
    }

    void Update()
    {
        time++;

        if (time >= spawnPeriod)
        {
            float x = Random.Range(-spawnRadius, spawnRadius);
            float y = Random.Range(-spawnRadius, spawnRadius);
            Vector3 position = new Vector3(x, y, -spawnHeight);
            int choice = Random.Range(0, choices.Count);
            SpawnPowerup(position, choices[choice]);
            time %= spawnPeriod;
        }
    }

    private void SpawnPowerup(Vector3 position, PowerupType type)
    {
        Debug.Log("spawning");
        var prefab = powerupPrefabs[(int) type];
        var obj = Instantiate(prefab);
        obj.transform.position = position;
    }
}