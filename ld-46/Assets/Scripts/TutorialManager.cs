using System.Collections;
using Doozy.Engine.UI;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform player;
    public TutorialStep[] steps;
    
    public UIView view;
    public Canvas canvas;

    public Level level;
    public bool canPlaceFan;
    
    public Vector3 canvasMiddle
    {
        get
        {
            var rect = canvas.GetComponent<RectTransform>();
            var middle = new Vector2(rect.sizeDelta.x / 2, rect.sizeDelta.y /2);
            return middle;
        }
    }

    private int m_currentIndex = 0;

    void Start()
    {
        DisplayStep(m_currentIndex);
        GameManager.Instance.OnLevelRun.AddListener(() =>
        {
            Event("StartPressed");
        });
    }
    
    void Update()
    {
        
    }

    void DisplayStep(int index)
    {
        StartCoroutine(Work());

        IEnumerator Work()
        {
            if (view.IsVisible) view.Hide();
            yield return new WaitForSeconds(0.5f);

            var step = steps[index];
            var pos = step.origin == null ? canvasMiddle : Camera.main.WorldToScreenPoint(step.origin.position);
            
            view.CustomStartAnchoredPosition = pos + step.offset;

            var tview = view.GetComponent<TutorialView>();
            tview.title.text = step.title;
            tview.content.text = step.content;
            tview.nextButton.SetActive(step.displayNextButton);
            
            view.Show();
        }
    }

    public void Event(string name)
    {
        if (m_currentIndex >= steps.Length) return; 
        if (steps[m_currentIndex].waitForEvent == name)
        {
            DisplayStep(++m_currentIndex);
        }
    }

    public void NextStep()
    {
        steps[m_currentIndex].onStepEnded?.Invoke();
        if (++m_currentIndex < steps.Length)
        {
            DisplayStep(m_currentIndex);
        }
        else
        {
            view.Hide();
        }
    }

    public void CanPlaceFan(bool value)
    {
        canPlaceFan = value;
    }
}
