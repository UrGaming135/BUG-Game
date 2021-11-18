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
    public bool isSpawning = false;

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

    // TESTING FIELDS
    [SerializeField]
    private int testEnemiesToSpawn = 10;
    [SerializeField]
    private int testEnemiesPerSpawn = 2;
    [SerializeField]
    private float testSecondsBetweenSpawn = 2;

    private Coroutine spawnCoroutine;

    private void Start()
    {
        spawnCoroutine = StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        //if (!hasPrintedGameOver && GameManager.instance.IsGameOver)
        //{
        //    hasPrintedGameOver = true;
        //    //print("GAME OVER!!");
        //}
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
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
