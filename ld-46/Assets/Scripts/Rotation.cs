using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float RotSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward, RotSpeed * Time.deltaTime);
    }
}
