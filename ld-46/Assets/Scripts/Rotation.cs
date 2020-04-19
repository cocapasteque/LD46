using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float RotSpeed;

    void Update()
    {
        transform.Rotate(transform.forward, RotSpeed * Time.deltaTime);
    }
}
