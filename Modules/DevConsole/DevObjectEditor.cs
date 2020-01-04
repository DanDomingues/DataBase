using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevObjectEditor : MonoBehaviour
{
    [SerializeField] protected DevObjectBase dataObject;
    [SerializeField] protected WarningSetting[] warnings;
    [SerializeField] protected DevFieldBase[] fields;

    protected Dictionary<string, string> entries;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        entries = new Dictionary<string, string>();
        fields = GetComponentsInChildren<DevFieldBase>();
        SetFieldValues(GetTreatedObjectValues());

        for (int i = 0; i < warnings.Length; i++)
        {
            warnings[i].warning.SetActive(false);
        }
    }

    protected virtual void SetFieldValues(string[] values)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].SetEditor(this);
            fields[i].SetValue(values[i]);

            if (entries.ContainsKey(fields[i].Key))
            {
                foreach (var item in entries)
                {
                    Debug.Log(item.Key + " " + item.Value);
                }
                continue;
            }

            entries.Add(fields[i].Key, fields[i].Value);
        }

    }
    protected virtual string[] TreatLocalEntries()
    {
        var currentValues = dataObject.GetCurrentValues();
        var values = new string[currentValues.Length];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = currentValues[i];
        }


        var keys = new List<string>(dataObject.GetKeys());
        int index;
        foreach (var item in entries)
        {
            if (!keys.Contains(item.Key))
            {
                var masterKey = "";
                for (int i = 0; i < keys.Count; i++)
                {
                    masterKey += keys[i] + "_";
                }

                Debug.Log(string.Format("{0} not found. Trace is \n{1}", item.Key, masterKey));
                return null;
            }

            index = keys.IndexOf(item.Key);
            values[index] = item.Value;
        }

        Debug.Log("Output:" + (values == null ? "Null" : values.Length.ToString()));
        return values;
    }
    protected virtual string[] GetTreatedObjectValues()
    {
        var keys = new List<string>(dataObject.GetKeys());
        var values = dataObject.GetCurrentValues();
        var output = new string[fields.Length];

        int index;
        for (int i = 0; i < fields.Length; i++)
        {
            if(!keys.Contains(fields[i].Key))
            {
                Debug.Log(string.Format("Did not find key {0}", fields[i].Key));
                continue;
            }

            index = keys.IndexOf(fields[i].Key);
            output[i] = values[index];
        }

        return output;
    }

    protected virtual void UpdateObject(string[] values)
    {
        dataObject.LoadSavedData(values);
    }

    public virtual void GetChanges(string key, string value)
    {
        entries[key] = value;
        UpdateObject(TreatLocalEntries());

        for (int i = 0; i < warnings.Length; i++)
        {
            if (warnings[i].keys.Contains(key)) warnings[i].warning.SetActive(true);
        }
    }

    private void OnDisable()
    {
        //UpdateObject();
        for (int i = 0; i < warnings.Length; i++)
        {
            warnings[i].warning.SetActive(false);
        }
    }

    [System.Serializable] protected struct WarningSetting
    {
        public string[] keys;
        public GameObject warning;
    }

}
