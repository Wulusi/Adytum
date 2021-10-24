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

    private GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void MoveToTarget(GameObject target)
    {
        float step = movement_speed * Time.deltaTime;
        this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, step);
    }

    public virtual void Attack(GameObject target)
    {

    }

    public virtual GameObject FindTarget()
    {
        Collider2D[] hit_colliders = Physics2D.OverlapCircleAll(this.transform.position, detection_radius);
        float nearest_target_distance = Mathf.Infinity;
        float distance;

        for (int i = 0; i < hit_colliders.Length; i++)
        {
            distance = Vector2.Distance(hit_colliders[i].GetComponentInParent<Transform>().position, this.transform.position);
            if (distance < nearest_target_distance)
            {
                nearest_target_distance = distance;
                target = hit_colliders[i].gameObject;
                Debug.Log("Current target = " + target);
            }
        }

        return target;
    }
}
