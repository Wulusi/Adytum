using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public HealthBar health_bar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit_health = 100;
        movement_speed = 5;
        damage = 5;
        detection_radius = 50f;
        attack_range = 1f;
        type = unit_type.ENEMY;
        health_bar.SetMaxHealth(unit_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_target == null)
        {
            current_target = FindTarget(unit_type.SOLDIER);
            if (current_target == null)
            {
                FindTarget(unit_type.BUILDING);
            }
        }

        if (is_slow)
        {
            movement_speed = 2.5f;
        } else
        {
            movement_speed = 5f;
        }

        MoveToTarget(current_target);
        Attack(current_target);
        health_bar.SetHealth(unit_health);
        Debug.Log(movement_speed);
    }
}
