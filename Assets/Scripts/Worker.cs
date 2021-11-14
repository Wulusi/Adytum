using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    public GameObject current_target;

    // Start is called before the first frame update
    void Start()
    {
    unit_health = 10;
    movement_speed = 5;
    damage = 1;
    detection_radius = 50f;
    attack_range = 1;
    type = unit_type.WORKER;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_target == null)
        {
            current_target = FindTarget(unit_type.RESOURCE);
        }
        MoveToTarget(current_target);
        Attack(current_target);
    }
}
