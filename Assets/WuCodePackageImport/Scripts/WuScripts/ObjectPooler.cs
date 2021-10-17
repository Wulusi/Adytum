using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton Pattern
    //Create a singleton pattern for easier access
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    //This is a list of the class Pool named pools, can create lists of classes, gameObjects etc etc.
    public List<Pool> pools;

    //This is a dictionary taking in string and gameObject named as poolDictionary
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    [SerializeField]
    private Transform targetParent;

    // Use this for initialization
    void Start()
    {

        //Initialize a dictionary/List taking in a string tag and a queue of gameObjects
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //For each element in the list named pools given a name as sausage 
        foreach (Pool pool in pools)
        {
            //Initialize the Queue of gameObjects as objectPool, create a queue of objects
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //create each one of the gameObjects that is supposed to be in each list of the dictionary
            for (int i = 0; i < pool.size; i++)
            {
                //instantiate the gameObject within the pool class as gObj
                GameObject gObj = Instantiate(pool.prefab);

                gObj.transform.SetParent(targetParent);

                //Set this gameObject active to false
                gObj.SetActive(false);

                //Put this gameObject into the queue of pools
                objectPool.Enqueue(gObj);
            }

            //Add this created Queue to the poolDictionary
            poolDictionary.Add(pool.prefab.name, objectPool);
        }
    }

    //Taking gObj from our pool and spawning into the world
    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rotation)
    {
        //Checks to see if the PoolDictionary contains a queue/ObjectPool with the appropriate Tag
        //If this tag does not exist throw an error
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " does not exist");

            return null;
        }

        //Create a reference to gameObject that needs to be taken out of the queue
        //the gameObject is taken out of from a Queue/Pool with tag within the poolDictionary
        GameObject gObjToSpawn = poolDictionary[tag].Dequeue();

        if (gObjToSpawn)
        {
            gObjToSpawn.SetActive(true);
            gObjToSpawn.transform.SetParent(targetParent);
            gObjToSpawn.transform.position = pos;
            gObjToSpawn.transform.rotation = rotation;
        }


        //Obtain reference to the component that contains interface save it as a variable and look at if it is null
        //if the not activate the method derived from the interface within the gameObject that derives this interface
        PooledObjInterface pooledObj = gObjToSpawn.GetComponent<PooledObjInterface>();

        if (pooledObj != null)
        {
            pooledObj.OnPooledObjSpawn();
        }

        poolDictionary[tag].Enqueue(gObjToSpawn);

        //Return the GameObject to be Spawned as a GameObject
        return gObjToSpawn;
    }

}

