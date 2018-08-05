using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : AutomatedSingleton<PoolHandler>
{
    public Transform poolHost;
    public Transform objHost;

    public List<PoolData> pools;

    protected override void Awake()
    {
        base.Awake();
        poolHost = transform;
        pools = new List<PoolData>();
    }

    public void SetHost(Transform host)
    {
        objHost = host;
    }

    public void AddPool(GameObject prefab, Transform objParent)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].prefab == prefab)
            {
                //Debug.Log("Object pool already exists");
                return;
            }
        }

        PoolData pool = new PoolData(prefab, poolHost, objParent);
        pools.Add(pool);
    }
    public void AddPool(GameObject prefab)
    {
        AddPool(prefab, null);
    }
    public void AddPool(GameObject[] prefabs)
    {
        AddPool(prefabs, null);
    }
    public void AddPool(GameObject[] prefabs, Transform parent)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            AddPool(prefabs[i], parent);
        }

    }

    public GameObject GetObject(string key)
    {
        GameObject value = null;

        var chosenPool = pools.FirstOrDefault(pool => pool.key == key);
        if(chosenPool != null)
        {
            value = chosenPool.GetObject();
        }
        else
        {
            Debug.Log(string.Format("No Pool found with given key ({0})", key));
        }

        return value;
    }

    public GameObject GetObject(string key, Vector3 position)
    {
        GameObject value = GetObject(key);

        if(value != null)
        {
            value.transform.localPosition = position;
        }

        return value;
    }

    public GameObject GetObject(string key, float returnTime)
    {
        GameObject value = GetObject(key);
        if (value != null)
        {
            StoreObject(value, returnTime);
        }

        return value;
    }

    public GameObject GetObject(string key, Vector3 position, float returnTime)
    {
        GameObject value = GetObject(key, position);
        if (value != null)
        {
            StoreObject(value, returnTime);
        }

        return value;
    }

    public void StoreObject(GameObject input)
    {
        int index = -1;
        string reference = "";

        for (int i = 0; i < pools.Count; i++)
        {
            if(input.name.Contains(pools[i].key) && pools[i].key.Length > reference.Length)
            {
                index = i;
                reference = pools[i].key;
            }
        }

        if(index >= 0) pools[index].StoreObject(input);
    }

    public void StoreObject(GameObject input, float delay)
    {
        InvokeAlternatives.Invoke(this, delay, () => { StoreObject(input); });
    }

    [System.Serializable]
    public class PoolData
    {
        public string key;
        public Transform bank;
        public Transform actionParent;
        public GameObject prefab;

        public PoolData(GameObject prefab, Transform poolParent, Transform objParent)
        {
            this.prefab = prefab;
            key = prefab.name;
            actionParent = objParent;

            bank = new GameObject(key + "Pool").transform;
            bank.parent = poolParent;
            bank.transform.position = Vector3.zero;

            bank.gameObject.SetActive(false);
        }

        public GameObject GetObject()
        {
            return ObjSetup();
        }

        public GameObject GetObject(Vector3 spawnLocalPos)
        {
            GameObject obj = ObjSetup();
            obj.transform.localPosition = spawnLocalPos;

            return obj;
        }

        private GameObject ObjSetup()
        {
            GameObject value = null;
            if (bank.childCount > 0)
            {
                value = bank.GetChild(0).gameObject;
                //value.SetActive(true);
            }
            else
            {
                value = Instantiate(prefab);
            }
          
            value.name = key;
            value.transform.parent = actionParent;

            return value;

        }


        public void StoreObject(GameObject obj)
        {
            obj.transform.parent = bank;
        }
    }
}
