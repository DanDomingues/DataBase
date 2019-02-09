using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField]
    GameObject baseTilePrefab;
    [Range(1, 25)]
    public int gridXSize = 5;
    [Range(1, 25)]
    public int gridYSize = 5;

#if UNITY_EDITOR
    [ContextMenu("Build Grid")]
    public void BuildGrid()
    {
        while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);

        var tileWidth = baseTilePrefab.GetComponent<BoxCollider>().size.x * baseTilePrefab.transform.lossyScale.x;
        GameObject tile;
        Transform row;
        for (int i = 0; i < gridXSize; i++)
        {
            row = new GameObject("Grid Row_" + (i + 1)).transform;
            row.parent = transform;
            row.localPosition = Vector3.zero;

            for (int j = 0; j < gridYSize; j++)
            {
                tile = UnityEditor.PrefabUtility.InstantiatePrefab(baseTilePrefab) as GameObject;
                tile.transform.parent = row;
                tile.transform.localPosition = new Vector3(tileWidth * i, 0, tileWidth * j);
                tile.name = string.Format("Base Tile_{0}-{1}", i + 1, j + 1);
            }
        }
    }
#endif

}