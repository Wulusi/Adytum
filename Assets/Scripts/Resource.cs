using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit_health = 5;
        movement_speed = 0;
        damage = 0;
        detection_radius = 0;
        attack_range = 0;
        type = unit_type.RESOURCE;
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
