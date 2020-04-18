using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSelector : MonoBehaviour
{
    public bool selected = false;

    public Color selectedColor;
    public Color deselectedColor;
    public Level level;
    public SpriteRenderer graphics;

    public void Select()
    {
        selected = true;
        graphics.color = selectedColor;
        //        tooltipView.Show();
    }

    public void Deselect()
    {
        selected = false;
        graphics.color = deselectedColor;
        //        tooltipView.Hide();
    }

    public void Move(Vector2 position)
    {
        transform.parent.position = position;
    }

    public void Rotate(Vector2 position)
    {
        // Get Angle in Radians
        float rad = Mathf.Atan2(position.y - transform.parent.position.y, position.x - transform.parent.position.x);
        // Get Angle in Degrees
        float deg = (180 / Mathf.PI) * rad;
        // Rotate Object
        transform.parent.rotation = Quaternion.Euler(0, 0, deg);
    }

    public void Remove()
    {
        level.RemoveFan(this);
    }
}
