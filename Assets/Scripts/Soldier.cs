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
        unit_health = 100;
        movement_speed = 5;
        damage = 5;
        detection_radius = 50f;
        attack_range = 1f;
        type = unit_type.SOLDIER;
        health_bar.SetMaxHealth(unit_health);

        //Special Ability set up
        special_ability.ability_name = "Charge";
        special_ability.ability_cooldown = 5.0f;
        special_ability.ability_range = 25.0f;
        special_ability.ability_duration = 0.0f;
        special_ability.target_afflicted = false;
        special_ability.ability_time_stamp = 0.0f;
        special_ability.ability_duration_time_stamp = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_target == null)
        {
            current_target = FindTarget(unit_type.ENEMY);
        }
        ExecuteSpecialAbility();
        MoveToTarget(current_target);
        Attack(current_target);
        health_bar.SetHealth(unit_health);
    }

    void ExecuteSpecialAbility()
    {
        if (current_target != null)
        {


            if (special_ability.ability_time_stamp <= Time.time && Vector2.Distance(target.transform.position, this.transform.position) <= special_ability.ability_range)
            {
                ability_in_use = true;
                movement_speed = 20.0f;
                special_ability.ability_time_stamp = Time.time + special_ability.ability_cooldown;
            }

            if (Vector2.Distance(current_target.GetComponentInParent<Transform>().position, this.transform.position) <= attack_range){
                movement_speed = 5f;
                ability_in_use = false;
            }

            if (ability_in_use)
            {
                float step = movement_speed * Time.deltaTime;
                this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, step);
            }
        }
    }
}
