#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PropReplacer : MonoBehaviour
{

    [Header("Input")]
    [SerializeField]
    string objectDomin;
    [SerializeField]
    List<Transform> listedObjects;
    [SerializeField]
    bool activeSelf;

    [Header("Output")]
    [SerializeField]
    Transform replaceHost;
    [SerializeField]
    GameObject replacePrefab;

    [Header("Collider Debugging")]
    [SerializeField]
    bool meshActive;

    [ContextMenu("List Objects")]
    public void ListObjects()
    {
        listedObjects = new List<Transform>();
        ListHierarchy(transform);
        Debug.Log(string.Format("Listing of root {0} had {1} results", objectDomin, listedObjects.Count));
    }

    void ListHierarchy(Transform source)
    {
        Transform child;
        for (int i = 0; i < source.childCount; i++)
        {
            child = source.GetChild(i);
            if(child.name.Contains(objectDomin))
            {
                listedObjects.Add(child);
            }
            ListHierarchy(child);
        }
    }

    [ContextMenu("Set Objects Active")]
    public void SetObjectsActive()
    {
        for (int i = 0; i < listedObjects.Count; i++)
        {
            listedObjects[i].gameObject.SetActive(activeSelf);
        }

    }

    [ContextMenu("Replace Objects")]
    public void ReplaceObjects()
    {
        WipeHost();

        GameObject obj;
        for(int i = 0; i < listedObjects.Count; i++)
        {
            obj = PrefabUtility.InstantiatePrefab(replacePrefab as GameObject) as GameObject;

            obj.transform.parent = replaceHost;

            obj.transform.position = listedObjects[i].position;
            obj.transform.localScale = listedObjects[i].localScale;
            obj.transform.eulerAngles = listedObjects[i].eulerAngles;

            obj.name += string.Format(" #{0}", i + 1);

            listedObjects[i].gameObject.SetActive(activeSelf);
        }
    }

    [ContextMenu("Wipe Host")]
    public void WipeHost()
    {
        while(replaceHost.childCount > 0)
        {
            DestroyImmediate(replaceHost.GetChild(0).gameObject);
        }
    }

    [ContextMenu("Apply Colliders")]
    public void ApplyColliders()
    {
        var meshComponents = GetComponentsInChildren<MeshFilter>();

        MeshCollider newCollider;
        foreach (var filter in meshComponents)
        {
            newCollider = filter.GetComponent<MeshCollider>();

            if (newCollider == null)
            {
               newCollider = filter.gameObject.AddComponent<MeshCollider>();
            }

            newCollider.sharedMesh = filter.sharedMesh;
        }

    }

    [ContextMenu("Set Meshes Active")]
    public void SetMeshesActive()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = meshActive;
        }
    }



}
#endif