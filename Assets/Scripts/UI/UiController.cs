using System.Collections;
using System.Collections.Generic;
using Tools.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    private List<Button> _buttons;
    public UnityEvent[] events;
    private UIDocument _uiDoc;
    internal UIDocument UiDoc => this.LazyGetComponent(ref _uiDoc);

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f); 
        var root = UiDoc.rootVisualElement;
        _buttons = root?.Query<Button>().ToList();
        if (events?.Length != _buttons?.Count)
            Debug.LogError("The list of buttons is longer than the number of events created!");

        EnableEvents(true);
    }

    private void EnableEvents(bool enable)
    {
        for (var i = 0; i < _buttons?.Count; i++)
        {
            if (i >= events?.Length)
            {
                Debug.LogError("No event found for " + _buttons[i].name + ".");
                continue;
            }

            if (events?[i] != null)
            {
                if(enable)
                    _buttons[i].clicked += events[i].Invoke;
                else
                    _buttons[i].clicked -= events[i].Invoke;
            }
            else
                Debug.LogError("The event " + i + " is null. " + 
                               _buttons[i].name + " will not be implemented.");
        }
    }

    public void EnableOrDisable(bool on)
    {
        if (!on)
        {
            EnableEvents(false);
            UiDoc.enabled = false;
        }
        else //It needs to be in an if/else because of the order it's executed.
        {
            UiDoc.enabled = true;
            EnableEvents(true);
        }
    }
}