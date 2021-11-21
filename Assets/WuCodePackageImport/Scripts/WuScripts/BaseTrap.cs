using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrap : MonoBehaviour
{
    public enum TrapType { Slow, Damage, Blockage }
    public TrapType trapType;

    public float trapRadius;
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
            Debug.Log("Trap Activated!");
            ActivateTrap(Object.gameObject);
            //ActivateTrap(Object);
        }
    }

    private void ActivateTrap(GameObject Target)
    {
        //Only interact with the target if it is an enemy unity type
        Unit TargetUnit = Target.GetComponent<Unit>();
        if (TargetUnit.type == Unit.unit_type.ENEMY)
        {
            //Based on the trap apply different logic to the target
            switch (trapType)
            {
                case TrapType.Blockage:

                    //Rigidbody2D TargetRB = Target.GetComponent<Rigidbody2D>();
                    if (TargetUnit != null)
                    {
                        //Save the target's current speed
                        float TargetSpd = TargetUnit.movement_speed;
                        //Stop the target
                        TargetUnit.movement_speed = 0;
                        //Wait for X seconds based onthe slow Timer
                        //Then put the target back to its current speed
                        StartCoroutine(CountDown(slowTimer, TargetUnit, TargetSpd));
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

                    if (TargetUnit != null)
                    {
                        //Save the target's current speed
                        float TargetSpd = TargetUnit.movement_speed;
                        //Slow the target up to 30% of its original speed
                        TargetUnit.movement_speed = 0.3f * TargetUnit.movement_speed;
                        //Wait for X seconds based onthe slow Timer
                        //Then put the target back to its current speed
                        StartCoroutine(CountDown(slowTimer, TargetUnit, TargetSpd));
                    }
                    break;
            }
        }
    }

    private IEnumerator CountDown(float duration, Unit TargetUnit, float speed)
    {
        float totalTime = 0;
        while (totalTime <= duration)
        {
            //This is for if we want an UI element to showcase how long the units are slowed for
            //countdownImage.fillAmount = totalTime / duration;
            totalTime += Time.deltaTime;
            yield return null;
        }
        //Reset the target unit's speed
        TargetUnit.movement_speed = speed;
    }

    private void OnDrawGizmos()
    {
        float radius = this.gameObject.GetComponent<CircleCollider2D>().radius;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius * 1.3f);
    }

    private void OnValidate()
    {
        if (this.gameObject.GetComponent<CircleCollider2D>() != null)
        {
            this.gameObject.GetComponent<CircleCollider2D>().radius = trapRadius;
        }
    }
}
