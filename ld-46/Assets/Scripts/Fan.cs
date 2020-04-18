using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Fan : MonoBehaviour
{
    public bool selected = false;

    public Color selectedColor;
    public Color deselectedColor;
    
    public Level level;
    
    private void Start()
    {
//        tooltipView.GetComponentInParent<UICanvas>().CanvasName = Guid.NewGuid().ToString();
//        GetComponent<PointEffector2D>().forceMagnitude = forceSlider.value * 50;
    }

    public void Select()
    {
        selected = true;
        GetComponentInChildren<SpriteRenderer>().color = selectedColor;
//        tooltipView.Show();
    }

    public void Deselect()
    {
        selected = false;
        GetComponentInChildren<SpriteRenderer>().color = deselectedColor;
//        tooltipView.Hide();
    }

    public void SliderValueChanged(float val)
    {
        GetComponent<PointEffector2D>().forceMagnitude = val * 50;
    }

    public void Remove()
    {
        level.RemoveFan(this);
    }

    public void Move(Vector2 position)
    {
        transform.position = position;
    }

    public void Rotate(Vector2 position)
    {
        // Get Angle in Radians
        float rad = Mathf.Atan2(position.y - transform.position.y, position.x - transform.position.x);
        // Get Angle in Degrees
        float deg = (180 / Mathf.PI) * rad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, deg);
    }
}
