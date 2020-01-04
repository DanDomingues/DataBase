using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Select(bool value);
    void GetDirectionInput(Vector2 value);
    //void GetDirectionInputInt(Vector2Int value);
}
