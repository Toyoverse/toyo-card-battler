using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    private List<Button> _buttons;
    public UnityEvent[] events;
    [HideInInspector] public UIDocument uiDoc;

    private void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        var root = uiDoc.rootVisualElement;
        _buttons = root.Query<Button>().ToList();

        if (events.Length != _buttons.Count)
        {
            Debug.LogError("The list of buttons is longer than the number of events created!");
        }
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i >= events.Length)
            {
                Debug.LogError("No event found for " + _buttons[i].name + ".");
                continue;
            }
            if (events[i] != null)
            {
                _buttons[i].clicked += events[i].Invoke;
            }
            else
            {
                Debug.LogError("The event " + i + " is null. " + 
                               _buttons[i].name + " will not be implemented.");
            }
        }
    }

    private void DisableEvents()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i >= events.Length)
            {
                Debug.LogError("No event found for " + _buttons[i].name + ".");
                continue;
            }
            if (events[i] != null)
            {
                _buttons[i].clicked -= events[i].Invoke;
            }
            else
            {
                Debug.LogError("The event " + i + " is null. " + 
                               _buttons[i].name + " will not be implemented.");
            }
        }
    }

    public void EnableOrDisable(bool on)
    {
        if (!on)
        {
            DisableEvents();
            uiDoc.enabled = on;
        }
        else
        {
            uiDoc.enabled = on;
            Start();
        }
    }
}