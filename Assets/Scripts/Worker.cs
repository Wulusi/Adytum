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
}

    // Update is called once per frame
    void Update()
    {
        current_target = FindTarget();
        MoveToTarget(current_target);
    }
}
