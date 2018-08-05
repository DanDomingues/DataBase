using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : AutomatedSingleton<SaveHandler>
{
    [SerializeField]
    ScriptableObject[] projectSaveObjects;

    ISaveObject[] saveObjects;
    public static bool LoadOnAwake;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        if(!SceneUtility.CheckForDomain("Menu"))
        {
            LoadOnAwake = true;
        }
#endif

        GetSaveObjects();

        bool load = LoadOnAwake;
        foreach (var obj in saveObjects)
        {
            if (load)
                obj.LoadSavedData();
            else
                obj.SetDefaultValues();
        }

        SceneUtility.OnSceneUnload += SaveObjects;
    }

    void GetSaveObjects()
    {
        var objs = new List<Object>(projectSaveObjects);
        objs.AddRange(FindObjectsOfType<MonoBehaviour>());

        var saveObjsList = new List<ISaveObject>();
        ISaveObject saveObj;
        foreach (var item in objs)
        {
            saveObj = item as ISaveObject;
            if (saveObj != null)
            {
                saveObjsList.Add(saveObj);
            }
        }

        saveObjects = saveObjsList.ToArray();

    }

    public string GetValue(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public void SetValue(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public string[] GetValues(string[] keys)
    {
        string[] values = new string[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            values[i] = GetValue(keys[i]);
        }
        return values;
    }

    public void SetValues(string[] keys, string[] values)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            SetValue(keys[i], values[i]);
        }
    }

    void SaveObjects()
    {
        foreach (var obj in saveObjects)
        {
            SetValues(obj.GetKeys(), obj.GetCurrentValues());
        }

        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        SceneUtility.OnSceneUnload -= SaveObjects;
    }

    private void OnApplicationQuit()
    {
        SaveObjects();
    }

}
