using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// Fire method to run when firing the turret
/// </summary>
public class UnityCustomEvent : UnityEngine.Events.UnityEvent
{

}

public abstract class TowerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the data container for tower parameters
    /// </summary>
    public sObj_tower_Params towerParams;

    /// <summary>
    /// The current state of the tower at runtime
    /// </summary>
    public enum TurretState { searching, firingtarget };
    public TurretState turretState;

    /// <summary>
    /// A reference to the current target of which the tower is focused on 
    /// </summary>
    public GameObject CurrentTarget;

    /// <summary>
    /// A reference a list of targets within range of the tower
    /// </summary>
    public List<GameObject> targets = new List<GameObject>();

    /// <summary>
    /// Transform of the tip of barrel which the turret is pointing towards
    /// </summary>
    public Transform barrel;

    /// <summary>
    /// A reference to a custom method signature to call once a target is determined, set to fire
    /// </summary>
    public UnityCustomEvent fireAtTarget;

    /// <summary>
    /// The current rotation speed of the turret, determines how fast target rotates towards the enemy
    /// Set using towerParams sObj
    /// </summary>
    public float rotationSpeed;
    /// <summary>
    /// The fire rate of the turret, how fast can the turret fire at the target
    /// Set using towerParams sObj
    /// </summary>
    public float fireRate;
    /// <summary>
    /// Reference to the projectile of which the tower is firing
    /// Set using towerParams sObj
    /// </summary>
    [SerializeField]
    private GameObject projectilePrefab;

    //Private member varibales for helper functions
    private ObjectPooler objectPooler;
    private Vector3 targetDir;
    private bool routineActive;

    public virtual void Awake()
    {
        //All current parameter data for tower from data container
        //*Note* method does not run in update loop, may need to adjust this for upgradable towers
        GetData();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        //Continous coroutine that runs the base logic of the tower
        StartCoroutine(ActivateTurret());

        //Reference to the ObjectPooler so Tower can spawn necessary instance of prefabs such as bullets
        objectPooler = ObjectPooler.Instance;
    }

    //Main coroutine loop, runs in while true continously instead of update
    public IEnumerator ActivateTurret()
    {
        while (true)
        {
            //Cycles through state of the turret to determine what is should currently do
            TurretStates();
            //Constantly check to see if turret has a target in range and if it is a valid target
            CheckTarget();
            yield return null;
        }
    }

    //Data getting function to get all needed parameters for turret
    private void GetData()
    {
        rotationSpeed = towerParams._rotationSpeed;
        fireRate = towerParams._fireRate;
        projectilePrefab = towerParams._projectile;
    }

    //Main turret behaviour states, can be overriden in inherited members to perform customizable function
    public virtual void TurretStates()
    {
        switch (turretState)
        {
            //Searching state
            case TurretState.searching:

                if (targets.Count != 0)
                {
                    LookAtTarget();

                    if (CurrentTarget)
                    {
                        //Debug.Log("Target Acquired");
                        Vector3 Dir = (CurrentTarget.transform.position - barrel.transform.position);
                        float angle = Vector3.Angle(Dir, barrel.transform.forward);

                        //Debug.Log("angle is " + angle);
                        if (angle <= 5)
                        {
                            turretState = TurretState.firingtarget;
                        }
                    }
                }
                else
                {
                    //Debug.Log("Searching for target");
                    CurrentTarget = null;
                    SearchTarget();
                }
                break;
            case TurretState.firingtarget:

                //Debug.Log("Firing Target");
                FireAtTarget();
                //StartCoroutine(Countdown(1f));
                break;
        }
    }

    public void CheckTarget()
    {
        if (CurrentTarget == null)
        {
            targets.Remove(CurrentTarget);

            if (targets.Count > 0)
            {
                CurrentTarget = targets[0];
            }
        }
        else if (!CurrentTarget.activeSelf)
        {
            targets.Remove(CurrentTarget);
            turretState = TurretState.searching;
        }
    }

    public virtual void LookAtTarget()
    {
        CheckTarget();
        if (CurrentTarget && targets.Count > 0)
        {
            Vector3 targetDir = CurrentTarget.transform.position - barrel.transform.position;

            CurrentTarget = targets[0];
            Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0f);
            barrel.rotation = Quaternion.LookRotation(newDir.normalized);
        }
        else
        {
            turretState = TurretState.searching;
        }
    }

    public virtual void SearchTarget()
    {
        Vector3 newDir = Vector3.RotateTowards(barrel.forward, targetDir, Mathf.Lerp(0, rotationSpeed, Time.deltaTime), 0.0f);

        float angle = Vector3.Angle(barrel.forward, new Vector3(targetDir.x, 0, targetDir.z));

        if (angle < 0.1f || targetDir == Vector3.zero)
        {
            targetDir = (Vector3)Random.insideUnitCircle - barrel.forward;
        }
        barrel.rotation = Quaternion.LookRotation(new Vector3(newDir.normalized.x, 0, newDir.normalized.z));
    }

    public virtual void FireAtTarget()
    {
        //Rotate target at the same time when firing at the target as well
        LookAtTarget();

        //Invoke custom event with a custom duration delay that counts up
        CoolDown(fireRate, fireAtTarget);
    }

    public void TestFire()
    {
        CheckTarget();
        if (CurrentTarget)
        {
            objectPooler.SpawnFromPool(projectilePrefab.name, barrel.transform.position, barrel.transform.rotation);
        }
    }

    public void CoolDown(float duration, UnityCustomEvent eventToInvoke)
    {
        StartCoroutine(Countdown(duration, eventToInvoke));
    }

    public IEnumerator Countdown(float duration, UnityCustomEvent eventToInvoke)
    {
        if (!routineActive)
        {
            routineActive = true;
            float totalTime = 0;
            while (totalTime <= duration)
            {
                totalTime += Time.deltaTime;
                yield return null;
            }
            routineActive = false;
            eventToInvoke?.Invoke();
        }
    }
}
