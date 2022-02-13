using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject GameEndScreen;

    [SerializeField]
    private List<Task> _myTasks = new List<Task>();

    [SerializeField]
    private List<GameLevelPhase> gamePhases = new List<GameLevelPhase>();

    [SerializeField]
    private TextMeshProUGUI phaseName, timer, roundIndicator;

    [SerializeField]
    private int currentRound = 0;
    [SerializeField]
    private int maxRounds = 0;

    [SerializeField]
    private GameObject townCenter;

    void Start()
    {
        BeginCountDown();
    }

    void Update()
    {

    }
    public void AddPhase(GameLevelPhase phaseToAdd)
    {
        if (timer != null)
        {
            phaseToAdd.setTimer(timer);
        }
        gamePhases.Add(phaseToAdd);
    }

    public void RemovePhase(GameLevelPhase phaseToRemove)
    {
        gamePhases.Remove(phaseToRemove);
    }

    public GameObject getTownCenter()
    {
        if(townCenter != null)
        {
            return townCenter;
        } else
        {
            Debug.LogError("You fogot to assign a towncenter to your level manager script", townCenter);
            return null;
        }
    }

    async void BeginCountDown()
    {
        currentRound++;

        roundIndicator.SetText("Round: {0}", currentRound);

        GameEndScreen.SetActive(false);

        for (int i = 0; i < gamePhases.Count; i++)
        {
            GameLevelPhase phaseActive = gamePhases[i];

            phaseName.text = phaseActive.phaseName.ToString() + " " + phaseActive.thisPhase;
            phaseActive.setTimer(timer);

            GameHub.GameManager.PhaseChange(phaseActive.thisPhase);

            _myTasks.Add(phaseActive.WaitForTimerToEnd());
            await gamePhases[i].WaitForTimerToEnd();
        }

        await Task.WhenAll(_myTasks);

        if (currentRound == maxRounds)
        {
            GameEndScreen.SetActive(true);
        }
        else
        {
            BeginCountDown();
        }
    }
}
