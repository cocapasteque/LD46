using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRot : MonoBehaviour
{
    public bool FreezeX;
    public bool FreezeY;
    public bool FreezeZ;

    private Vector3 FreezeVector;

    private void Start()
    {
        FreezeVector = new Vector3(FreezeX ? 0f : 1f, FreezeY ? 0f : 1f, FreezeZ ? 0f : 1f);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.Scale(transform.rotation.eulerAngles, FreezeVector));    
    }
}
