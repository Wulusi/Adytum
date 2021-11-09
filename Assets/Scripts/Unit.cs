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
    public enum unit_type { RESOURCE, ENEMY, SOLDIER, WORKER};
    public unit_type type;
    
    private float nearest_target_distance = Mathf.Infinity;
    private float distance;
    private GameObject target;
    private Vector3 offset_vector = new Vector3(0.2f, -0.2f, 0);

    public virtual void MoveToTarget(GameObject target)
    {
        float step = movement_speed * Time.deltaTime;
        if (target != null)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position + offset_vector, step);
        }

    }

    public virtual void Attack(GameObject target)
    {

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
                        Debug.Log("Current target = " + target);
                    }
                }
            }
       
        return target;
        

    }
}
