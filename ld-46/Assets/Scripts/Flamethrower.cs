using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public Transform target;
    public Transform rotationOrigin;

    public float fireRate;
    
    // Update is called once per frame
    void Update()
    {
        LookAtTarget();
    }

    void LookAtTarget()
    {
        float rad = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x);
        // Get Angle in Degrees
        float deg = (180 / Mathf.PI) * rad;
        // Rotate Object
        rotationOrigin.rotation = Quaternion.Euler(0, 0, deg);
    }
}
