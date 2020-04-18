using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour
{
    public string levelName;
    public int availableFans = 5;

    public List<Fan> fans = new List<Fan>();

    private float currentPressTime = 0f;
    private Fan grabbedFan;

    public bool isTutorial = false;
    private TutorialManager tManager;

    private void Start()
    {
        if (isTutorial)
        {
            tManager = FindObjectOfType<TutorialManager>();
            tManager.level = this;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);

            // check if we clicked a scene object (fan/player/sawblade) 
            if (hit)
            {
                SceneObjectClicked(hit);
                if (isTutorial) tManager.Event("FanClicked");
            }
            // check if we want to place a fan.
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                PlaceFan(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (isTutorial) tManager.Event("FanPlaced");
            }
        }

        // If we hold left click (move)
        if (Input.GetMouseButton(0))
        {
            // If we already grabbed a fan
            if (grabbedFan != null)
            {
                grabbedFan.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (isTutorial) tManager.Event("FanMoved");
            }
            else
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);
                // check if we clicked a scene object (fan/player/sawblade) 
                if (hit)
                {
                    if (currentPressTime >= GameManager.Instance.pressThreshold)
                    {
                        SceneObjectPressed(hit);
                    }

                    currentPressTime += Time.deltaTime;
                }
            }
        }

        // If we hold right click (rotate)
        if (Input.GetMouseButton(1))
        {
            var selectedFan = fans.SingleOrDefault(x => x.selected);
            if (selectedFan != null)
            {
                if (currentPressTime >= GameManager.Instance.pressThreshold)
                {
                    selectedFan.Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if(isTutorial) tManager.Event("FanRotated");
                }

                currentPressTime += Time.deltaTime;
            }
        }
        
        // Reset press time
        if (Input.GetMouseButtonUp(0))
        {
            currentPressTime = 0;
            grabbedFan = null;
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

    private void SceneObjectPressed(RaycastHit2D hit)
    {
        var fan = hit.transform.GetComponent<Fan>();
        if (fan != null)
        {
            if (!fan.selected) fan.Select();
            grabbedFan = fan;
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
        fan.level = this;

        return fan;
    }

    public void RemoveFan(Fan fan)
    {
        fans.Remove(fan);
        Destroy(fan.gameObject);
    }
}