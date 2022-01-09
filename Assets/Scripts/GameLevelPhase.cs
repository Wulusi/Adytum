using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLevelPhase : MonoBehaviour
{
    [SerializeField]
    private string _phaseName;

    [SerializeField]
    private float duration;

    [SerializeField]
    private TextMeshPro timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task WaitForTimerToEnd()
    {
        var end = Time.time + duration;
        while(Time.time < end)
        {
            //Put the timer here on the screen and the phase name
            if (timer != null)
            {
                timer.text = (end - Time.time).ToString();
            }
            await Task.Yield();
        }
    }
}
