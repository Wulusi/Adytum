using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    public HealthBar health_bar;
    private SpecialAbility special_ability;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit_health = 10;
        movement_speed = 5;
        damage = 5;
        detection_radius = 50f;
        attack_range = 1f;
        type = unit_type.SOLDIER;
        health_bar.SetMaxHealth(unit_health);

        //Special Ability set up
        special_ability.ability_name = "Charge";
        special_ability.ability_cooldown = 25.0f;
        special_ability.ability_range = 10.0f;
        special_ability.ability_duration = 0.0f;
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

    void ExecuteSpecialAbility()
    {

    }
}
