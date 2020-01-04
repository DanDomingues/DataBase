using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic UI inputs scriptable object. 
/// Needs to be created and setup pior to being used.
/// Inputs are suggestions rather than demands, and can be modified at will.
/// </summary>

[CreateAssetMenu(fileName = "UI_Inputs", menuName = "Data/Inputs/UI_Inputs")]
public class UI_Inputs : ScriptableSingleton<UI_Inputs>
{
    [Header("Press inputs")]
    [SerializeField] ButtonInput select;
    public static bool Select => Instance.select.Value;

    [SerializeField] ButtonInput m_return;
    public static bool Return => Instance.m_return.Value;

    [SerializeField] ButtonInput pauseToggle;
    public static bool PauseToggle => Instance.pauseToggle.Value;

    [SerializeField] ButtonInput inventoryToggle;
    public static bool InventoryToggle => Instance.inventoryToggle.Value;

    [Header("Axes")]
    [SerializeField] InstantAxis switchCategory;
    public static int SwitchCategoryInt => Instance.switchCategory.Value;

    [SerializeField] AxisInput ItemWheelAxisX;
    [SerializeField] AxisInput ItemWheelAxisY;  
    public static Vector2Int ItemWheelAxis
    {
        get
        {
            return new Vector2Int((int)Instance.ItemWheelAxisX.Value, (int)Instance.ItemWheelAxisY.Value);
        }
    }

}
