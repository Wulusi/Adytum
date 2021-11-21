using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public GameObject current_target;

    // Start is called before the first frame update
    void Start()
    {
        unit_health = 10;
        movement_speed = 5;
        damage = 5;
        detection_radius = 50f;
        attack_range = 1f;
        type = unit_type.ENEMY;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_target == null)
        {
            current_target = FindTarget(unit_type.SOLDIER);
        }
        MoveToTarget(current_target);
        Attack(current_target);
    }
}
