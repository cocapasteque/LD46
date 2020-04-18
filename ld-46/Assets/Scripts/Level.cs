using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour
{
    public string levelName;
    public int availableFans = 5;

    public List<Fan> fans = new List<Fan>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);

            // check if we clicked a scene object (fan/player/sawblade) 
            if (hit)
            {
                SceneObjectClicked(hit);
            }
            // check if we want to place a fan.
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                PlaceFan(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }


    private void DeselectAllFans()
    {
        fans.ForEach(f => { f.Deselect(); });
    }

    private void SceneObjectClicked(RaycastHit2D hit)
    {
        Debug.Log($"Over Scene Object {hit.collider.name}");
        var fan = hit.transform.GetComponent<Fan>();
        if (fan != null)
        {
            if (fan.selected)
            {
                fan.Deselect();
            }
            else
            {
                DeselectAllFans();
                fan.Select();
            }
        }
    }

    private Fan PlaceFan(Vector2 pos)
    {
        if (availableFans <= fans.Count) return null;
        
        DeselectAllFans();

        var obj = Instantiate(GameManager.Instance.fanPrefab, pos, Quaternion.identity);
        var fan = obj.GetComponent<Fan>();
        fans.Add(fan);
        fan.Select();

        return fan;
    }
}