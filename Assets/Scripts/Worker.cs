using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{

    public HealthBar health_bar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit_health = 10;
        movement_speed = 5;
        damage = 1;
        detection_radius = 50f;
        attack_range = 1f;
        type = unit_type.WORKER;
        health_bar.SetMaxHealth(unit_health);

        statemachine = new Statemachine();
        //start the statemachine
        statemachine.ChangeState(new FindTargetState(this));
        gameManager.onPhaseChange += OnPhaseChanged;
    }

    // Update is called once per frame
    void Update()
    {
        statemachine.Update();
        health_bar.SetHealth(unit_health);
    }

    protected override void OnPhaseChanged(phase phase)
    {
        if (phase == phase.ATTACK)
        {
            statemachine.ChangeState(new ReturnState(this));
        }

        if (phase == phase.GATHER)
        {
            statemachine.ChangeState(new FindTargetState(this));
        }
    }

    public void OnDestroy()
    {
        gameManager.onPhaseChange -= OnPhaseChanged;
    }
}
public class MoveToTargetState : IState
{
    Unit thisUnit;
    public MoveToTargetState(Unit unitOwner)
    {
        thisUnit = unitOwner;
    }
    public void Enter()
    {
        Debug.Log(thisUnit.name + "has entered " + thisUnit.currentState);

        if (thisUnit.current_target == null)
        {
            thisUnit.statemachine.ChangeState(new FindTargetState(this.thisUnit));
        }
    }

    public void Execute()
    {
        thisUnit.MoveToTarget(thisUnit.current_target);

        if (this.thisUnit.canAttack())
        {
            thisUnit.statemachine.ChangeState(new AttackState(this.thisUnit));
        }
    }

    public void Exit()
    {

    }
}

public class FindTargetState : IState
{
    Unit thisUnit;
    public FindTargetState(Unit unitOwner)
    {
        thisUnit = unitOwner;
    }
    public void Enter()
    {
        thisUnit.currentState = this;

        Debug.Log(thisUnit.name + "has entered " + thisUnit.currentState);

        if (thisUnit.current_target == null)
        {
            thisUnit.current_target = thisUnit.FindTarget(unit_type.RESOURCE);
        }
    }

    public void Execute()
    {
        if (thisUnit.current_target == null)
        {
            Enter();
        }
        else
        {
            thisUnit.statemachine.ChangeState(new MoveToTargetState(thisUnit));
            Debug.Log("Changing to move state");
        }
    }

    public void Exit()
    {

    }
}
public class AttackState : IState
{
    Unit thisUnit;

    public AttackState(Unit unitOwner)
    {
        thisUnit = unitOwner;
    }
    public void Enter()
    {
        thisUnit.currentState = this;

        Debug.Log(thisUnit.name + "has entered " + thisUnit.currentState);
    }

    public void Execute()
    {
        if (thisUnit.current_target != null)
            thisUnit.Attack(thisUnit.current_target);
        else
            this.thisUnit.statemachine.ChangeState(new FindTargetState(thisUnit));
    }

    public void Exit()
    {
        thisUnit.current_target = null;
    }
}

public class ReturnState : IState
{
    Unit thisUnit;

    public ReturnState(Unit unitOwner)
    {
        thisUnit = unitOwner;
    }
    public void Enter()
    {
        thisUnit.currentState = this;
        //FInd target to base
        Debug.Log("Finding target to base");

        Debug.Log(thisUnit.name + "has entered " + thisUnit.currentState);
    }

    public void Execute()
    {
        //Return back to base when ready
        Debug.Log("Executing command to go back to base");
    }

    public void Exit()
    {
        //callback to execute when state is completed'
        Debug.Log("Base back complete doing call back");
    }
}

