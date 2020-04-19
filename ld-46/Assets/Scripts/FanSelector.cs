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

    public List<Sprite> FanSprites;

    public float fanChangeTime;
    public float currentAngle;
    private int currentSpriteIndex;
    private float currentSpriteTime;
    private void Start()
    {
        currentSpriteIndex = 0;
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
        if (Fan.currentForce > 0)
        {
            if (currentSpriteTime > fanChangeTime / Fan.currentForce)
            {
                currentSpriteIndex = (currentSpriteIndex + 1) % FanSprites.Count;
                graphics.sprite = FanSprites[currentSpriteIndex];
                currentSpriteTime = 0f;
            }
            else
            {
                currentSpriteTime += Time.deltaTime;
            }
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

    public void Remove()
    {
        level.RemoveFan(this);
    }
}
