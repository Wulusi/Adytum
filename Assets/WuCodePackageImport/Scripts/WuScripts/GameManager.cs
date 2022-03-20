using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System;

public enum phase { GATHER, ATTACK };
public interface IphaseChangeable
{
    void phaseChanged(phase phaseChangeTo);
}
public class GameManager : MonoBehaviour
{
    public phase currentPhase;
    //Current temporary game manager
    public float CurrentHealth, MaxHealth;

    [SerializeField]
    private Text Health;

    [SerializeField]
    private GameObject GameOver;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private float finalSpawnCount;

    [SerializeField]
    private LevelManager levelManager;
    public LevelManager getLevelManager => levelManager;

    public event Action<phase> onPhaseChange;

    public event Action onStateChanged;

    [SerializeField]
    private Vector3 selectedCursorPosition;
    public Vector3 mouseSelectedPosition => selectedCursorPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (GameOver != null)
        {
            GameOver.SetActive(false);
        }
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
    public void PhaseChange(phase phaseToChange)
    {
        currentPhase = phaseToChange;

        onPhaseChange?.Invoke(phaseToChange);
    }
    private void CheckGameState()
    {
        if (CurrentHealth <= 0 && GameOver != null)
        {
            GameOver.SetActive(true);
        }
    }
    private void UpdateUI()
    {
        if (Health != null)
        {
            Health.text = CurrentHealth.ToString();
        }
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

    public void setMousePositionVector(Vector3 selectedPosition)
    {
        selectedCursorPosition = selectedPosition;
        onStateChanged?.Invoke();
    }
}
