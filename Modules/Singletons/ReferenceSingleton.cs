using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    public static string Name
    {
        get
        {
            return Instance ? Instance.name : DefaultName;
        }
    }
    public static string DefaultName
    {
        get
        {
            return typeof(T).ToString();
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.Log(string.Format("Instance value overwritten at {0}. {1} -> {2}",
                                DefaultName, instance.name, GetComponent<T>().name));
        }

        instance = GetComponent<T>();
    }
}
