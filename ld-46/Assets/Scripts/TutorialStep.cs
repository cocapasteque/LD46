using System;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TutorialStep
{
    public string title;
    
    [TextArea]
    public string content;

    public Transform origin;
    public Vector3 offset;

    public bool displayNextButton = true;
    [HideIf("displayNextButton")] public string waitForEvent;

    public UnityEvent onStepEnded;
}