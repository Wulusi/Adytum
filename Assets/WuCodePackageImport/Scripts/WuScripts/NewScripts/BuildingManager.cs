using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour, IphaseChangeable
{
    [SerializeField]
    private Transform[] MineLocations;
    // Start is called before the first frame update
    void Start()
    {
        GameHub.GameManager.onPhaseChange += phaseChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void phaseChanged(phase phaseChangeTo)
    {
        string debug = " {0} is within phase {1} ";

        if (phaseChangeTo == phase.ATTACK)
        {
            Debug.LogFormat(debug, this.gameObject.name, phaseChangeTo);
        }
        else
        {
            Debug.LogFormat(debug, this.gameObject.name, phaseChangeTo);
        }
    }

    private void OnDestroy()
    {
        if (GameHub.GameManager != null)
        {
            GameHub.GameManager.onPhaseChange -= phaseChanged;
        }
    }
}
