using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelEvents : MonoBehaviour
{
    public static PanelEvents instance;

    public UnityEvent OnPanelClose;

    private void Awake()
    {
        instance = this;
    }

}
