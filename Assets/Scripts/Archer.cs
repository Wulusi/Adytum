using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{

    public HealthBar health_bar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit_health = 10;
        movement_speed = 3;
        damage = 2;
        detection_radius = 50f;
        attack_range = 4f;
        type = unit_type.SOLDIER;
        health_bar.SetMaxHealth(unit_health);

    }

    // Update is called once per frame
    void Update()
    {

        if (current_target == null)
        {
            current_target = FindTarget(unit_type.ENEMY);
        }
        MoveToTarget(current_target);
        Attack(current_target);
        health_bar.SetHealth(unit_health);

    }
}
