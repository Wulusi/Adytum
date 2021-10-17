using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sObj_tower_Params : ScriptableObject
{
    /// <summary>
    /// Rotational speed of tower's turret, how fast does it rotate to face the enemy
    /// </summary>
    [SerializeField]
    [Range(0, 50)]
    private float rotationSpeed;
    public float _rotationSpeed => rotationSpeed;

    /// <summary>
    /// The firing speed of the tower, how fast the tower fires at the target
    /// </summary>
    [SerializeField]
    [Range(0, 50)]
    private float fireRate;
    public float _fireRate => fireRate;

    /// <summary>
    /// The firing speed of the tower, how fast the tower fires at the target
    /// </summary>
    [SerializeField]
    [Range(0, 50)]
    private float detectionRadius;
    public float _detectionRadius => detectionRadius;

    /// <summary>
    /// Reference to the projectile of which the tower is supposed to fire
    /// </summary>
    [SerializeField]
    private GameObject projectile;
    public GameObject _projectile => projectile;
}
