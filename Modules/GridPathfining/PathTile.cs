using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TilePath
{
    public GridTile tile;
    public GridTile pathTile;
    [SerializeField]
    public TilePath path;
    public float CostSoFar { get { return path != null ? path.CostSoFar + 1 : 0; } }

    public TilePath(GridTile tile, TilePath path, float costSoFar)
    {
        this.tile = tile;
        this.path = path;

        if(path != null)
            pathTile = path.tile;
    }
}
