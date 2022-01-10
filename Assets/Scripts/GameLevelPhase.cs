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
    public string phaseName => _phaseName;

    [SerializeField]
    private float duration;

    [SerializeField]
    private TextMeshProUGUI timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTimer(TextMeshProUGUI timerToSet) 
    {
        timer = timerToSet;
    }

    public async Task WaitForTimerToEnd()
    {
        var end = Time.time + duration;
        //Debug.Log("Timer Started!!");
        while (Time.time < end)
        {
            //Put the timer here on the screen and the phase name
            if (timer != null)
            {
                float timeRemaining = (end - Time.time);
                //Debug.Log("Timer Left: " + timeRemaining);
                //string currentText = timer.text;
                timer.SetText("Time Remaining: {0:2}", timeRemaining);
            }
            await Task.Yield();
        }
    }
}
