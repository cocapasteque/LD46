using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Fan : MonoBehaviour
{
    public float force;
    private BoxCollider2D coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
//        tooltipView.GetComponentInParent<UICanvas>().CanvasName = Guid.NewGuid().ToString();
//        GetComponent<PointEffector2D>().forceMagnitude = forceSlider.value * 50;
    }   

    public void SliderValueChanged(float val)
    {
        GetComponent<PointEffector2D>().forceMagnitude = val * 50;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {           
            float r = (collision as CircleCollider2D).radius;
            Vector2 localCollPos = transform.InverseTransformPoint(collision.transform.position);
            float d = Mathf.Abs(localCollPos.x);
            float b = coll.size.x / 2f;
            //Magic math
            float overlap = Mathf.Clamp01((b + r - d) / (2f * r));
            collision.attachedRigidbody.AddForce(transform.up * force * overlap * (1f / Vector2.Distance(collision.transform.position, transform.position)));
        }
    }
}
