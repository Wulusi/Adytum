using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //Class variables
    public int unit_health;
    public float movement_speed;
    public int damage;
    public float detection_radius;
    public float attack_range;
    public enum unit_type { RESOURCE, ENEMY, SOLDIER, WORKER };
    public unit_type type;
    
    private float nearest_target_distance = Mathf.Infinity;
    private float distance;
    private GameObject target;
    //private Vector3 offset_vector = new Vector3(0.2f, -0.2f, 0);
    private float attack_cooldown = 2.0f;
    private float time_stamp = 0f;

    public virtual void MoveToTarget(GameObject target)
    {
        float step = movement_speed * Time.deltaTime;
        
        if (target != null)
        {
            distance = Vector2.Distance(target.GetComponentInParent<Transform>().position, this.transform.position);
            if (distance > attack_range)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, step);
            }
        }

    }

    public virtual void Attack(GameObject target)
    {
        if (target != null)
        {
            if (Vector2.Distance(target.GetComponentInParent<Transform>().position, this.transform.position) <= attack_range)
            {
                Debug.Log("Going to attack");
                if (time_stamp <= Time.time)
                {
                    target.GetComponentInParent<Unit>().unit_health -= damage;
                    Debug.Log("Deal Damage!");
                    //Debug.Log("Current Health: " + target.GetComponentInParent<Unit>().unit_health);
                    time_stamp = Time.time + attack_cooldown;
                    DestroyTarget(target);
                }
            }
        }
    }

    public virtual GameObject FindTarget(unit_type target_type)
    {
        Collider2D[] hit_colliders = Physics2D.OverlapCircleAll(this.transform.position, detection_radius);

            for (int i = 0; i < hit_colliders.Length; i++)
            {
                if (hit_colliders[i].gameObject.GetComponent<Unit>().type == target_type)
                {
                    distance = Vector2.Distance(hit_colliders[i].GetComponentInParent<Transform>().position, this.transform.position);
                    if (distance < nearest_target_distance)
                    {
                        nearest_target_distance = distance;
                        target = hit_colliders[i].gameObject;                      
                    }
                }
            }
       
        return target;
        

    }


    public virtual void DestroyTarget(GameObject target)
    {
        if (target.GetComponentInParent<Unit>().unit_health <= 0)
        {
            Destroy(target);
            nearest_target_distance = Mathf.Infinity;
        }
    }
}
