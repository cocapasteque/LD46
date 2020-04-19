using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour
{
    public string levelName;
    public int availableFans = 5;
    public float baseScore;
    public float baseTime;
    public float fanHighscoreValue;
    public float timeHighscoreValue;
    public int tries;

    public List<FanSelector> fans = new List<FanSelector>();

    private float currentPressTime = 0f;

    private FanSelector selectedFan;
    private FanSelector grabbedFan;

    public bool isTutorial = false;
    private TutorialManager tManager;
    private Camera mainCamera;
    private Vector2 dragOffset;


    private void Start()
    {
        mainCamera = Camera.main;
        if (isTutorial)
        {
            tManager = FindObjectOfType<TutorialManager>();
            tManager.level = this;
        }

        tries = PlayerPrefs.HasKey("Tries_" + levelName) ? PlayerPrefs.GetInt("Tries_" + levelName) : 1;
    }

    public void ClearTries()
    {
        PlayerPrefs.SetInt("Tries_" + levelName, 1);
        tries = PlayerPrefs.HasKey("Tries_" + levelName) ? PlayerPrefs.GetInt("Tries_" + levelName) : 1;
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Running)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);

                // check if we clicked a scene object (fan/player/saw blade) 
                if (hit)
                {
                    SceneObjectClicked(hit);
                    if (isTutorial) tManager.Event("FanClicked");
                }
                // check if we want to place a fan.
                else if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (isTutorial && !tManager.canPlaceFan) return;

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
                    grabbedFan.Move((Vector2) mainCamera.ScreenToWorldPoint(Input.mousePosition) + dragOffset);
                    if (isTutorial) tManager.Event("FanMoved");
                }
                else
                {
                    var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);
                    // check if we clicked a scene object (fan/player/saw blade) 
                    if (hit)
                    {
                        if (currentPressTime >= GameManager.Instance.pressThreshold)
                        {
                            SceneObjectPressed(hit, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        }

                        currentPressTime += Time.deltaTime;
                    }
                }
            }

            // Reset press time
            if (Input.GetMouseButtonUp(0))
            {
                currentPressTime = 0;
                grabbedFan = null;
            }
        }
    }

    public void DeselectAllFans()
    {
        fans.ForEach(f => { f.Deselect(); });
        selectedFan = null;
    }

    private void SceneObjectClicked(RaycastHit2D hit)
    {
        Debug.Log($"Over Scene Object {hit.collider.name}");
        var fan = hit.transform.GetComponent<FanSelector>();
        if (fan != null)
        {
            if (fan.selected)
            {
                fan.Deselect();
                selectedFan = null;
            }
            else
            {
                DeselectAllFans();
                fan.Select();
                selectedFan = fan;
            }
        }
    }

    private void SceneObjectPressed(RaycastHit2D hit, Vector2 hitPosition)
    {
        var fan = hit.transform.GetComponent<FanSelector>();
        if (fan != null)
        {
            if (!fan.selected) fan.Select();
            dragOffset = (Vector2) fan.transform.parent.position - hitPosition;
            grabbedFan = fan;
        }
    }

    private FanSelector PlaceFan(Vector2 pos)
    {
        if (availableFans <= fans.Count) return null;

        DeselectAllFans();

        var obj = Instantiate(GameManager.Instance.fanPrefab, pos, Quaternion.identity);
        var fan = obj.GetComponentInChildren<FanSelector>();
        fans.Add(fan);
        if (!isTutorial) fan.Select();
        fan.level = this;
        selectedFan = fan;
        UpdateFanCount();
        return fan;
    }

    public void RemoveCurrentFan()
    {
        if (selectedFan != null)
        {
            RemoveFan(selectedFan);
        }
    }

    public void RemoveAllFans()
    {
        foreach (var fan in fans)
        {
            Destroy(fan.transform.parent.gameObject);
        }

        fans = new List<FanSelector>();
    }

    public void RemoveFan(FanSelector fan)
    {
        fans.Remove(fan);
        Destroy(fan.transform.parent.gameObject);
        UpdateFanCount();
    }

    public void AddTry()
    {
        tries++;
        PlayerPrefs.SetInt("Tries_" + levelName, tries);
        GameOverlay.Instance.UpdateTries();
    }

    private void UpdateFanCount()
    {
        GameOverlay.Instance.Fans.text = fans.Count() + "/" + availableFans;
    }
}