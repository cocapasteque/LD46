using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FanSelector : MonoBehaviour
{
    public bool selected = false;
    public Fan Fan;

    public Color selectedColor;
    public Color deselectedColor;
    public Level level;
    public SpriteRenderer graphics;

    public float currentAngle;

    private void Start()
    {
        currentAngle = 0f;
    }

    private void Update()
    {
        if (selected)
        {
            currentAngle = GameOverlay.Instance.RotationSlider.normalizedValue;
            float rad = (1f - GameOverlay.Instance.RotationSlider.normalizedValue) * 2f * Mathf.PI;
            float deg = (180 / Mathf.PI) * rad;
            transform.parent.rotation = Quaternion.Euler(0, 0, deg);
        }
    }

    public void Select()
    {
        selected = true;
        Fan.Select();
        graphics.color = selectedColor;
        GameOverlay.Instance.RotationSlider.normalizedValue = currentAngle;
        GameOverlay.Instance.SelectFan();
        //        tooltipView.Show();
    }

    public void Deselect()
    {
        selected = false;
        graphics.color = deselectedColor;
        GameOverlay.Instance.DeselectFan();
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

    public void Rotate2()
    {
        
    }

    public void Remove()
    {
        level.RemoveFan(this);
    }
}
