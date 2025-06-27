using Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject boss;
    public Transform bossSpawnLocation;

    private bool _hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasSpawned)
        {
            Instantiate(boss, bossSpawnLocation.position, bossSpawnLocation.rotation);

            _hasSpawned = true;
        }
    }
}
