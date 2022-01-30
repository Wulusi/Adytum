using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    public void Enter();
    public void Execute();
    public void Exit();
}
public class Statemachine : MonoBehaviour
{
    IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        newState.Enter();
    }

    public bool IsInState<T> (IState state) where T: IState
    {
        return state is T;
    }
    public void Update()
    {
        if (currentState != null)
        {
            Debug.Log("Executing State!");
            currentState.Execute();
        }
    }
}
