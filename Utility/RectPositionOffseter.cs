using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectPositionOffseter : MonoBehaviour
{
    [SerializeField]
    RectTransform parent;

    [SerializeField]
    RectTransform[] children;
    [SerializeField]

    [Header("Values")]
    float rotationOffset;
    [SerializeField]
    Vector2 pivot;
    [SerializeField]
    bool invertOrder;

    [ContextMenu("Get Children")]
    public void GetChildren()
    {
        var childrenList = new List<RectTransform>();

        RectTransform value;
        for (int i = 0; i < parent.childCount; i++)
        {
            value = parent.GetChild(i).GetComponent<RectTransform>();
            childrenList.Add(value);
        }

        children = childrenList.ToArray();

    }

    [ContextMenu("Apply Rotation and Pivot")]
    public void ApplyOffsets()
    {
        int index;
        for (int i = 0; i < children.Length; i++)
        {
            index = invertOrder ? (children.Length - 1) - i : i;
            children[index].eulerAngles = Vector3.forward * ((i + 1) * rotationOffset);

            children[index].pivot = pivot;
            children[index].anchorMin = pivot;
            children[index].anchorMax = pivot;
        }
    }
}
