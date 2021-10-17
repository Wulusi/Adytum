using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    public Transform[] destinationMarkers;
    public List<GameObject> enemiesTypes = new List<GameObject>();

    public float spawnTimer, spawnCount, maxSpawnCount;

    // Start is called before the first frame update
    void Start()
    {
        //TO DO: Multiton for all manager level elements
        objectPooler = ObjectPooler.Instance;
        StartCoroutine(Countdown(spawnTimer));
    }

    private IEnumerator Countdown(float duration)
    {
        float totalTime = 0;
        while (totalTime <= duration)
        {
            //countdownImage.fillAmount = totalTime / duration;
            totalTime += Time.deltaTime;
            yield return null;
        }
        spawnEnemy(enemiesTypes[Random.Range(0, enemiesTypes.Count)]);
    }

    private void spawnEnemy(GameObject enemy)
    {
        if (spawnCount < maxSpawnCount)
        {
            Analytics.CustomEvent("enemy spanwed", new Dictionary<string, object>
            {
                {"enemy name", enemy.name },
                {"time_elasped", Time.timeSinceLevelLoad }
            }
            );

            GameObject spawned = objectPooler.SpawnFromPool(enemy.name, this.transform.position, Quaternion.identity);
            //spawned.GetComponent<EnemyBehaviour>().DestinationMarker = destinationMarkers[0];
            spawnCount++;
            StartCoroutine(Countdown(spawnTimer));
        }
        else
        {
            Debug.Log("At max number of spawn!");
        }
    }
}
