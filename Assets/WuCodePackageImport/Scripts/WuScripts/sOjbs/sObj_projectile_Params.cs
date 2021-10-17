using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sObj_projectile_Params : ScriptableObject
{
    /// <summary>
    /// The layer of object this projectile is looking to hit, defaults to 30 which is the "Enemies" layer
    /// </summary>
    [SerializeField]
    [Range(0, 31)]
    private int enemyLayer;
    public int _enemyLayer => enemyLayer;

    /// <summary>
    /// The amount of damage this projectile deals (defaults to 2)
    /// </summary>
    [SerializeField]
    [Range(0, 50)]
    private float damageValue;
    public float _damageValue => damageValue;

    /// <summary>
    /// The speed at which this projectile travels
    /// </summary>
    [SerializeField]
    [Range(0, 5000)]
    private float projectileSpeed;
    public float _projectileSpeed => projectileSpeed;

    /// <summary>
    /// The amount of time in second that the projectile will be alive for (defaults to 1)
    /// </summary>
    [SerializeField]
    [Range(0, 50)]
    private float projectileLifetime;
    public float _projectileLifetime => projectileLifetime;

    [SerializeField]
    private GameObject projectile;
    public GameObject _projectile => projectile;
}
