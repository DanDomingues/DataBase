using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TransformEvent : UnityEvent<Transform>
{

}

[System.Serializable]
public class IntEvent : UnityEvent<int>
{

}
