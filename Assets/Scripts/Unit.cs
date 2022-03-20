using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum unit_type { RESOURCE, ENEMY, SOLDIER, WORKER, BUILDING };
public class Unit : MonoBehaviour
{
    //Class variables
    public int unit_health;
    public float movement_speed;
    public int damage;
    public float detection_radius;
    public float attack_range;
    public unit_type type;
    public bool is_slow = false;

    private float nearest_target_distance = Mathf.Infinity;
    private float distance;

    [SerializeField]
    protected GameObject target;
    //private Vector3 offset_vector = new Vector3(0.2f, -0.2f, 0);
    private float attack_cooldown = 2.0f;
    private float time_stamp = 0f;

    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] protected GameObject selectionCursor;

    protected GameManager gameManager;
    protected bool ability_in_use = false;

    public Statemachine statemachine;
    public GameObject current_target;

    public IState currentState;
    public float savedSpeeed;

    protected bool isSelected = false;

    protected Vector2 destinationCoordinates;
    public struct SpecialAbility
    {
        public string ability_name;
        public float ability_cooldown;
        public float ability_range;
        public float ability_duration;
        public bool target_afflicted;
        public float ability_time_stamp;
        public float ability_duration_time_stamp;
    }

    protected virtual void Start()
    {
        gameManager = GameHub.GameManager;

        if (selectionCursor != null)
        {
            selectionCursor.SetActive(false);
        }
        //statemachine.ChangeState(initialState);
    }

    public virtual void MoveToTarget(GameObject target)
    {
        float step = movement_speed * Time.deltaTime;

        if (target != null && !ability_in_use)
        {
            distance = Vector2.Distance(target.GetComponentInParent<Transform>().position, this.transform.position);
            if (distance > attack_range)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, step);
            }
        }
    }

    public bool canAttack()
    {
        if (target != null)
        {
            return Vector2.Distance(target.GetComponentInParent<Transform>().position, this.transform.position) <= attack_range;

        }
        else
        {
            return false;
        }
    }


    public virtual void MoveToPosition(Vector3 targetPosition)
    {
        float step = movement_speed * Time.deltaTime;

        if (targetPosition != null)
        {
            destinationCoordinates = targetPosition;
            distance = Vector2.Distance(targetPosition, this.transform.position);
            if (distance > 0.5f)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, step);
            }
        }
    }

    public bool hasReachedDestination()
    {
        distance = Vector2.Distance(destinationCoordinates, this.transform.position);
        if (distance > 0.5f)
        {
            return false;
        } else
        {
            return true;
        }
    }
    public virtual void Attack(GameObject target)
    {
        if (target != null && !ability_in_use)
        {
            if (Vector2.Distance(target.GetComponentInParent<Transform>().position, this.transform.position) <= attack_range)
            {
                if (time_stamp <= Time.time)
                {
                    ShowDamage(damage.ToString(), target.GetComponentInParent<Transform>().position);
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
            Unit targetUnit = hit_colliders[i].gameObject.GetComponent<Unit>();

            if (targetUnit != null && targetUnit.type == target_type)
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

    public void GetDamangeAndCheckHealth(int damageToTake)
    {
        unit_health -= damageToTake;
        if (this.unit_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void ShowDamage(string text, Vector2 position)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    protected virtual void OnPhaseChanged(phase phase)
    {
        //Behaviour to change based on phases
    }

    protected virtual void OnStateChanged()
    {
        //Behaviour to change based on state change for the unit such as cursor commands
    }

    public virtual void setSelectionCursor(bool _bool)
    {
        if (selectionCursor != null)
        {
            selectionCursor.SetActive(_bool);
        }

        if (_bool)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }
    }
    
    public bool isThisUnitSelected()
    {
        return isSelected;
    }
}
