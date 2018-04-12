using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandaloneMenuBase : PanelBase
{

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Disables interaction and visibility for setup
    /// </summary>
    protected void StartSetup()
    {
        group.SetActive(false);
    }

    /// <summary>
    /// Enables interaction and visibility. Pairs with the StartSetup method.
    /// </summary>
    protected void EndSetup()
    {
        //canvasGroup.SetActive(true);
    }
}
