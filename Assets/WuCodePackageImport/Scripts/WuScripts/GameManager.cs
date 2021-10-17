using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    //Current temporary game manager
    public float CurrentHealth, MaxHealth;

    [SerializeField]
    private Text Health;

    [SerializeField]
    private GameObject GameOver;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private float finalSpawnCount;

    // Start is called before the first frame update
    void Start()
    {
        GameOver.SetActive(false);
        CurrentHealth = MaxHealth;
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            UpdateUI();
            CheckGameState();
            yield return null;
        }
    }

    private void CheckGameState()
    {
        if (CurrentHealth <= 0)
        {
            GameOver.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        Health.text = CurrentHealth.ToString();
    }

    public void ReloadLevel()
    {
        finalSpawnCount = enemySpawner.GetComponent<EnemySpawner>().spawnCount;

        Analytics.CustomEvent("enemy spanwed", new Dictionary<string, object>
            {
                {"total_gameplay_time", Time.timeSinceLevelLoad },
                {"total_enemy_spawned", finalSpawnCount }
            }
            );

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
