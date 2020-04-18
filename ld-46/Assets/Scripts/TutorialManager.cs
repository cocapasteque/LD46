using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TutorialManager : MonoBehaviour
{
    public Transform player;
    public TutorialStep[] steps;
    
    public UIView view;
    public Canvas canvas;

    public Level level;
    
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
        if (steps[m_currentIndex].waitForEvent == name)
        {
            DisplayStep(++m_currentIndex);
        }
    }

    public void NextStep()
    {
        steps[m_currentIndex].onStepEnded?.Invoke();
        DisplayStep(++m_currentIndex);
    }
}
