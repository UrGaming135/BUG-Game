using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject selectedMob;


    [Tooltip("Number of enemeies spawned at a time.")]
    public float enemeiesPerTick = 1;

    [SerializeField]
    private string[] pathsToPrefabs;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private float spawnRadius = 10f;
    [SerializeField]
    private float timeBetweenSpawns = 2f;
    [SerializeField]
    private float enemiesToSpawn = 2f;
    [SerializeField]
    private float startSpawnRadius = 50f;

    private Coroutine spawnCoroutine;
    private GameObject player;
    private bool isSpawning = false;

    private void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            var playerDistance = Vector3.Distance(player.transform.position, transform.position);
            if (playerDistance <= startSpawnRadius && !isSpawning)
            {
                StartSpawning();
            } else if (playerDistance > startSpawnRadius && isSpawning)
            {
                StopSpawning();
            }
        }
    }

    private bool GetNavMeshPoint(out Vector3 point)
    {
        for (int i = 0; i < 30; i++)
        {
            var randomPoint = Random.insideUnitSphere * spawnRadius + transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
        }
        point = Vector3.zero;
        return false;
    }

    private void StartSpawning()
    {
        isSpawning = true;
        spawnCoroutine = StartCoroutine(Spawn());
    }

    private void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        isSpawning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, startSpawnRadius);
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                // Place enemy on the map
                if (GetNavMeshPoint(out Vector3 point))
                {
                    Instantiate(selectedMob, point, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
