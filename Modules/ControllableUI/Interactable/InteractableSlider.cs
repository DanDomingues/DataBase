using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InteractableSlider : MonoBehaviour, IInteractable
{
    [SerializeField] CanvasGroup group;
    [SerializeField] Slider slider;

    private void Reset()
    {
        slider = GetComponentInChildren<Slider>();
        group = GetComponentInChildren<CanvasGroup>();
    }

    public void GetDirectionInput(Vector2 value)
    {
        slider.value += value.x * Time.unscaledDeltaTime;
    }
    public void Select(bool value)
    {
        group.alpha = value ? 1f : 0.5f;
    }

}
