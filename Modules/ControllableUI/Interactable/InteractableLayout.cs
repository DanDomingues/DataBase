using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLayout : MonoBehaviour
{
    [SerializeField] bool loopInput;
    [SerializeField] InteractableSlider[] startingElements;

    [SerializeField] List<InteractableSlider> elements;

    public int SelectedIndex { get; private set; }
    public InteractableSlider SelectedElement => elements[SelectedIndex];

    private void Start()
    {
        elements = new List<InteractableSlider>();

        for (int i = 0; i < startingElements.Length; i++)
        {
            elements.Add(startingElements[i]);
        }
    }
    private void Reset()
    {
        startingElements = GetComponentsInChildren<InteractableSlider>();
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
    public void GetDirectionInput(Vector2 value)
    {
        SelectedElement.GetDirectionInput(value);
    }

    public void AddElement(InteractableSlider element)
    {
        elements.Add(element);
    }
    public void RemoveElement(InteractableSlider element)
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
