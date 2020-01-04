using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour, ISelectable
{
    [SerializeField] Button button;
    [SerializeField] Image highlightImage;

    private void Reset()
    {
        highlightImage = GetComponentInChildren<Image>();
        button = GetComponentInChildren<Button>();
    }
    private void OnDisable()
    {
        highlightImage.enabled = false;
    }

    public void Select(bool value)
    {
        highlightImage.enabled = value;
    }
    public void RunButtonAction()
    {
        button.onClick?.Invoke();
    }
}
