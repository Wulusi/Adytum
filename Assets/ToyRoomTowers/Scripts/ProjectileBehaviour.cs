using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject floatingTextPrefab;

    [SerializeField]
    private sObj_projectile_Params projectileParams;

    /// <summary>
    /// The layer of object this projectile is looking to hit, defaults to 30 which is the "Enemies" layer
    /// </summary>
    [SerializeField]
    [Range(0, 31)]
    private int enemyLayer;

    /// <summary>
    /// The amount of damage this projectile deals (defaults to 2)
    /// </summary>
    public float damageValue;

    /// <summary>
    /// The speed at which this projectile travels
    /// </summary>
    public float projectileSpeed;

    /// <summary>
    /// The amount of time in second that the projectile will be alive for (defaults to 1)
    /// </summary>
    public float projectileLifetime;

    /// <summary>
    /// If this is the first time that the projectile is spawned in the world, if it isn't do not activate the interface
    /// again
    /// </summary>
    public bool isFirstSpawned;

    /// <summary>
    /// The attached rigidbody component
    /// </summary>
    private Rigidbody rb;

    private Rigidbody2D rb2d;

    /// <summary>
    /// Task cancellation token for the projectile kill
    /// </summary>
    readonly CancellationTokenSource killCancel = new CancellationTokenSource();

    /// <summary>
    /// Variable that represents the projectile kill task, allows collision call to cancel the async expiration 
    /// </summary>
    Task t;

    private void Awake()
    {
        GetDataParams();

        if (null != rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        else
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
    }

    private void OnEnable()
    {
        GetDataParams();
        KillCurrentProjectile();
         //ignore this underline
    }

    //Data Getter through Scritable Objects
    private void GetDataParams()
    {
        enemyLayer = projectileParams._enemyLayer;
        damageValue = projectileParams._damageValue;
        projectileSpeed = projectileParams._projectileSpeed;
        projectileLifetime = projectileParams._projectileLifetime;

    }

    private async void KillCurrentProjectile()
    {
        try
        {
            await KillProjectile();
        } 
        catch (TaskCanceledException exception)
        {
            Debug.Log("Projectile terminated before reaching target, lifetime timer reached" + exception.ToString());
        }
    }

    /// <summary>
    /// Asynchronous function that is called on Awake, this method kills the projectile after its lifetime expires
    /// </summary>
    private async Task KillProjectile()
    {
        //Delay by the lifetime, and give the cancellation token needed to stop the task if the projectile hit its enemy
        t = Task.Delay((int)(projectileLifetime * 1000), killCancel.Token);

        //Call the delay task to start the expiration countdown
        await t;

        //Instead of destroy send projectile back into Object Pool
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Moves the projectile by its speed towards its forward direction
    /// </summary>
    private void MoveProjectile()
    {
        if(rb != null)
        rb.velocity = transform.forward * (projectileSpeed * Time.fixedDeltaTime);
        else
        rb2d.velocity = transform.right * (projectileSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(this.gameObject.name + " Hit " + other.gameObject.name);

        if (other.gameObject.layer == enemyLayer)
        {
            //Damage the enemy the projectile just hit
            //other.GetComponent<EnemyBehaviour>().LoseHP(damageValue);
            ShowDamage(damageValue.ToString(), other.transform.root.position);

            if (other.gameObject != null)
            {
                other.GetComponentInParent<Unit>().GetDamangeAndCheckHealth((int)damageValue);
            }

            //Tell the task cancellation token to cancel the delay task in KillProjectile because we're going to kill the projectile early
            killCancel.Cancel();

            //Instead of destroy send projectile back into Object Pool                   
            this.gameObject.SetActive(false);
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
}
