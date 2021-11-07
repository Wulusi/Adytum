using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        unit_health = 10;
        movement_speed = 3;
        damage = 2;
        detection_radius = 50f;
        attack_range = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
