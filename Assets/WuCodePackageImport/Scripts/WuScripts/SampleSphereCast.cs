using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSphereCast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Vector3 p1 = transform.position;

        float sphereSize = GetComponent<Transform>().transform.localScale.x;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (Physics.SphereCast(p1, sphereSize, transform.position, out hit, 10))
        {
            Debug.Log("I found: " + hit.collider.name);
        }
    }
}
