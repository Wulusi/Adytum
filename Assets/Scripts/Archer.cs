using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{

    public HealthBar health_bar;
    private SpecialAbility special_ability;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit_health = 100;
        movement_speed = 3;
        damage = 2;
        detection_radius = 50f;
        attack_range = 4f;
        type = unit_type.SOLDIER;
        health_bar.SetMaxHealth(unit_health);
        ability_in_use = false;

        //Special Ability set up
        special_ability.ability_name = "Achilles Heel";
        special_ability.ability_cooldown = 5.0f;
        special_ability.ability_range = 100.0f;
        special_ability.ability_duration = 2.0f;
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
            float speed_percentage = 0.5f;


            // If the ability is off cooldown and the target is in range, then use the ability
            if (special_ability.ability_time_stamp <= Time.time &&
                current_target != null &&
                Vector2.Distance(current_target.GetComponentInParent<Transform>().position, this.transform.position) <= special_ability.ability_range &&
                !current_target.GetComponentInParent<Unit>().is_slow)
            {

                special_ability.ability_duration_time_stamp = Time.time;
                special_ability.ability_time_stamp = Time.time + special_ability.ability_cooldown;
                ability_in_use = true;
                special_ability.target_afflicted = true;
                current_target.GetComponentInParent<Unit>().is_slow = true;
            }

            // Check if ability duration is up
            if (special_ability.ability_duration_time_stamp + special_ability.ability_duration <= Time.time && current_target.GetComponentInParent<Unit>().is_slow)
            {
                current_target.GetComponentInParent<Unit>().is_slow = false;
            }

            ability_in_use = false;
        }
    }
}
