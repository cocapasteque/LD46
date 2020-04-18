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
            if (hit)
            {
                Debug.Log($"Over Scene Object {hit.collider.name}");
                if (hit.transform.GetComponent<Fan>() != null)
                {
                    hit.transform.GetComponent<Fan>().Select();
                }
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                DeselectAllFans();
                var fan = PlaceFan(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                fan.Select();
            }
        }
    }

    void DeselectAllFans()
    {
        fans.ForEach(f => { f.Deselect(); });
    }

    public Fan PlaceFan(Vector2 pos)
    {
        var obj = Instantiate(GameManager.Instance.fanPrefab, pos, Quaternion.identity);
        var fan = obj.GetComponent<Fan>();
        fans.Add(fan);
        return fan;
    }
}