using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float baseForce = 5f;

    private Rigidbody2D m_rb;
    private Vector3 m_startingPosition;
    
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.isKinematic = true;

        m_startingPosition = transform.position;
        
        GameManager.Instance.OnPlayerDied.AddListener(Killed);
        GameManager.Instance.OnLevelRun.AddListener(Started);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyBaseForce();
    }
    void ApplyBaseForce()
    {
        if (GameManager.Instance.State != GameState.Running) return;
        m_rb.AddForce(Vector2.up * baseForce);
    }

    void Started()
    {
        m_rb.isKinematic = false;
    }
    void Killed()
    {
        Debug.Log("Player killed");
        m_rb.isKinematic = true;
        m_rb.velocity = Vector3.zero;
        transform.position = m_startingPosition;
    }
}
