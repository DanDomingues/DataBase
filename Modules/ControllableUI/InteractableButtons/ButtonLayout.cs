using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLayout : MonoBehaviour
{
    [SerializeField] bool loopInput;
    [SerializeField] MonoBehaviour[] startingElements;

    [SerializeField] List<ISelectable> elements;

    public int SelectedIndex { get; private set; }
    public ISelectable SelectedElement => elements[SelectedIndex];

    private void Start()
    {
        elements = new List<ISelectable>();

        ISelectable selectable;
        for (int i = 0; i < startingElements.Length; i++)
        {
            selectable = startingElements[i].gameObject.GetComponent<ISelectable>();
            if(selectable != null) elements.Add(selectable);
        }
    }
    private void Reset()
    {
        startingElements = GetComponentsInChildren<SelectableButton>();
    }

    public void SetSelection(int index)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].Select(i == index);
        }

        SelectedIndex = index;
    }
    public void ChangeSelection(int value)
    {
        var newValue = SelectedIndex + value;
        newValue = loopInput ? LoopInput(newValue, elements.Count) : Mathf.Clamp(newValue, 0, elements.Count - 1);
        SetSelection(newValue);
    }

    public void SetActive(bool value)
    {
        elements[SelectedIndex].Select(value);
    }

    public void AddElement(ISelectable element)
    {
        elements.Add(element);
    }
    public void RemoveElement(ISelectable element)
    {
        elements.Remove(element);
    }
    public void ResetElements()
    {
        elements = new List<ISelectable>();
    }

    int LoopInput(int input, int limit)
    {
        if (limit <= 0) return input;

        if (input >= limit) input -= limit;
        if (input < 0) input += limit;
        return input;
    }
}
