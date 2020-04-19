using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float baseForce = 5f;

    public GameObject[] deathEffects;
    public GameObject graphics;

    private Rigidbody2D m_rb;
    private Vector3 m_startingPosition;
    private Quaternion m_startingRotation;

    private Transform basket;
    private Vector3 basket_startingPosition;
    private Quaternion basket_startingRotation;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.isKinematic = true;


        m_startingPosition = transform.position;
        m_startingRotation = transform.rotation;

        basket = transform.GetChild(0).GetChild(1);
        basket_startingPosition = basket.transform.position;
        basket_startingRotation = basket.transform.rotation;

        GameManager.Instance.OnBeforePlayerDied.AddListener(EffectKilled);
        GameManager.Instance.OnPlayerDied.AddListener(Killed);
        GameManager.Instance.OnLevelRun.AddListener(Started);
        GameManager.Instance.OnLevelCompleted.AddListener(Completed);
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

    void Completed()
    {
        StartCoroutine(Work());

        IEnumerator Work()
        {
            yield return new WaitForSeconds(0.2f);
            m_rb.isKinematic = true;
            ResetPosition();
        }
    }

    void Started()
    {
        m_rb.isKinematic = false;
    }

    void Killed()
    {
        Debug.Log("Player killed");
        m_rb.isKinematic = true;
        graphics.SetActive(true);
        ResetPosition();
    }

    void EffectKilled()
    {
        Camera.main.GetComponent<CameraController>().Shake();
        graphics.SetActive(false);
        foreach (var effect in deathEffects)
        {
            var o = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(o, 5);
        }
    }

    void ResetPosition()
    {
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_rb.velocity = Vector3.zero;
        transform.position = m_startingPosition;
        transform.rotation = m_startingRotation;

        basket.transform.position = basket_startingPosition;
        basket.transform.rotation = basket_startingRotation;

        m_rb.constraints = RigidbodyConstraints2D.None;
    }
}