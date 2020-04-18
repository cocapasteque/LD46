using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float baseForce = 5f;

    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyBaseForce();
    }

    void ApplyBaseForce()
    {
        m_rb.AddForce(Vector2.up * baseForce);
    }
}
