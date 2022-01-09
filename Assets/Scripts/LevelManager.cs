using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public enum phase {GATHER, ATTACK};

    public GameObject phaseIndicator;

    [SerializeField]
    private List<Task> _myTasks = new List<Task>();

    [SerializeField]
    private List<GameLevelPhase> gamePhases = new List<GameLevelPhase>(); 
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddPhase(GameLevelPhase phaseToAdd)
    {
        gamePhases.Add(phaseToAdd);
    }

    public void RemovePhase(GameLevelPhase phaseToRemove)
    {
        gamePhases.Remove(phaseToRemove);
    }

    async void BeginCountDown()
    {
        phaseIndicator.SetActive(false);

        for (int i = 0; i < gamePhases.Count; i++)
        {
            _myTasks.Add(gamePhases[i].WaitForTimerToEnd());
        }

        await Task.WhenAll(_myTasks);

        phaseIndicator.SetActive(true);
    }
}
