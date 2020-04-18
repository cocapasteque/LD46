using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : MonoBehaviour
{
    public float force = 5f;
    public bool selected = false;

    public Color selectedColor;
    public Color deselectedColor;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {

    }

    public void Select()
    {
        selected = true;
        GetComponentInChildren<SpriteRenderer>().color = selectedColor;
    }

    public void Deselect()
    {
        selected = false;
        GetComponentInChildren<SpriteRenderer>().color = deselectedColor;
    }
}
