using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrap : MonoBehaviour
{
    public enum TrapType { Slow, Damage, Blockage }
    public TrapType trapType;

    public int slowTimer;
    public int trapDamage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D Object)
    {
        //If the target is a unit or something that is intended to interact with the trap
        //Then the trigger should activate the trap
        if (Object.GetComponent<Unit>() != null)
        {
            ActivateTrap(Object.gameObject);
            //ActivateTrap(Object);
        }
    }

    private void ActivateTrap(GameObject Target)
    {
        //Only interact with the target if it is an enemy unity type
        if (Target.GetComponent<Unit>().type == Unit.unit_type.ENEMY)
        {
            //Based on the trap apply different logic to the target
            switch (trapType)
            {
                case TrapType.Blockage:

                    Rigidbody2D TargetRB = Target.GetComponent<Rigidbody2D>();
                    if (TargetRB != null)
                    {
                        //Save the target's current speed
                        Vector2 TargetSpd = TargetRB.velocity;
                        //Stop the target
                        TargetRB.velocity = Vector2.zero;
                        //Wait for X seconds based onthe slow Timer
                        StartCoroutine(CountDown(slowTimer));
                        //Then put the target back to its current speed
                        TargetRB.velocity = TargetSpd;
                    }
                    break;

                case TrapType.Damage:
                    if (Target.GetComponent<Unit>().unit_health > 0)
                    {
                        //Code to deal damage to the target unit's health
                        //For now use hard coded one
                        //TO DO: Should only deal damage to target once
                        Target.GetComponent<Unit>().unit_health -= trapDamage;
                    }
                    break;

                case TrapType.Slow:

                    Rigidbody2D TargetRB2 = Target.GetComponent<Rigidbody2D>();
                    if (TargetRB2 != null)
                    {
                        //Save the target's current speed
                        Vector2 TargetSpd = TargetRB2.velocity;
                        //Slow the target by 66% or another number we see fit
                        TargetRB2.velocity = TargetSpd * 0.3f;
                        //Wait for X seconds based onthe slow Timer
                        StartCoroutine(CountDown(slowTimer));
                        //Then put the target back to its current speed
                        TargetRB2.velocity = TargetSpd;
                    }
                    break;
            }
        }
    }

    private IEnumerator CountDown(float duration)
    {
        float totalTime = 0;
        while (totalTime <= duration)
        {
            //This is for if we want an UI element to showcase how long the units are slowed for
            //countdownImage.fillAmount = totalTime / duration;
            totalTime += Time.deltaTime;
            yield return null;
        }
    }

}
