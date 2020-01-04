using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableLayout : MonoBehaviour
{
    [SerializeField] bool loopInput;
    [SerializeField] MonoBehaviour[] startingElements;

    List<ISelectable> elements;

    public ISelectable[] Elements => elements.ToArray();
    public int SelectedIndex { get; private set; }
    public ISelectable SelectedElement => elements[SelectedIndex];

    private void Start()
    {
        elements = new List<ISelectable>();

        for (int i = 0; i < startingElements.Length; i++)
        {
            if(startingElements[i] is ISelectable)
            {
                elements.Add(startingElements[i].GetComponent<ISelectable>());
            }
        }
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
        if(elements.Count > SelectedIndex)
        {
            elements[SelectedIndex].Select(value);
        }
    }

    public void SetElements(ISelectable[] elements)
    {
        this.elements = new List<ISelectable>(elements);
    }
    public void AddElement(ISelectable element)
    {
        elements.Add(element);
    }
    public void RemoveElement(ISelectable element)
    {
        elements.Remove(element);
    }

    int LoopInput(int input, int limit)
    {
        if (limit <= 0) return input;

        if (input >= limit) input -= limit;
        if (input < 0) input += limit;
        return input;
    }
}
