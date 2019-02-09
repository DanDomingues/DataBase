using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    static GridHandler instance;
    public static GridHandler Instance { get { return instance; } }

    [SerializeField]
    GameObject baseTile;
    [SerializeField]
    GridTile[,]  tiles;

    int gridWidth;
    int gridLength;

    public delegate void GridEvent();

    public event GridEvent OnSetupDone;

    Coroutine lastRoutine;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(GridSetup_Routine());
    }

    IEnumerator GridSetup_Routine()
    {
        var tileList = GetComponentsInChildren<GridTile>();
        var width = baseTile.GetComponent<BoxCollider>().size.x * baseTile.transform.lossyScale.x;

        var builder = GetComponent<GridBuilder>();
        gridWidth = builder.gridXSize;
        gridLength = builder.gridYSize;

        tiles = new GridTile[gridWidth, gridLength];

        string[] coords;
        int x, y;

        foreach (var tile in tileList)
        {
            coords = tile.name.Split('_')[1].Split('-');
            x = int.Parse(coords[0]) - 1;
            y = int.Parse(coords[1]) - 1;

            tiles[x, y] = tile;

            tile.Setup(width);
            yield return new WaitForEndOfFrame();
        }

        if (OnSetupDone != null) OnSetupDone();

    }

    public GridTile[] GetPath(GridTile start, GridTile target, bool requireTraversable, bool useDiagonals)
    {
        if (target == start)
        {
            Debug.Log("Path ends at start");
            return new GridTile[0];
        }
        var openList = new List<TilePath>();
        var closedList = new List<TilePath>();

        var firstElement = new TilePath(start, null, 0);

        openList.AddRange(GetPathTiles(firstElement, openList, closedList, target, requireTraversable, useDiagonals));
        //Debug.DrawLine(firstElement.tile.transform.position + Vector3.up, openList.Last().tile.transform.position + Vector3.up, Color.red, 10f);

        int sanityBreaker = 0;
        TilePath curTile;
        
        while(openList.Count > 0)
        {
            curTile = openList.Last();
            openList.RemoveAt(openList.Count - 1);
            closedList.Add(curTile);

            openList.AddRange(GetPathTiles(curTile, openList, closedList, target, requireTraversable, useDiagonals));

            if (openList.Count == 0)
            {
                //Debug.Log("Loop broke after open list got empty");
                return new GridTile[] { target };
            }

            if(openList.Last().tile == target)
            {
#if UNITY_EDITOR
                for (int i = 0; i < openList.Count; i++)
                {
                    Debug.DrawRay(openList[i].tile.transform.position, Vector3.up * openList[i].CostSoFar, Color.green, 10f);
                }
                for (int i = 0; i < closedList.Count; i++)
                {
                    Debug.DrawRay(closedList[i].tile.transform.position, Vector3.up * closedList[i].CostSoFar, Color.blue, 10f);
                }
#endif
                return GetPathFrom(start, openList.Last());
            }

            sanityBreaker++;
            if (sanityBreaker > 200)
            {
                Debug.Log("Sanity broke!");
                break;
            }

        }

        Debug.Log("Found no suitable path!");
        return new GridTile[0];
    }

    TilePath[] GetPathTiles(TilePath start, List<TilePath> openList, List<TilePath> closedList, GridTile target, bool requireTraversable, bool useDiagonals)
    {
        var tiles = useDiagonals ? start.tile.AdjacentTiles : start.tile.Neighbors;

        tiles = tiles.OrderByDescending(tile => Vector3.Distance(tile.transform.position, target.transform.position)).ToArray();

        tiles = tiles.Where(tile => tile.IsTraversable || !requireTraversable || tile == target).ToArray();
        var output = new List<TilePath>();

        var list = new List<TilePath>(openList);
        list.AddRange(closedList);

        for (int i = 0; i < tiles.Length; i++)
        {
            if(list.FirstOrDefault(tile => tile.tile == tiles[i]) == null)
            {
                output.Add(new TilePath(tiles[i], start, start.CostSoFar + 1));
            }
        }

        return output.ToArray();
    }

    GridTile[] GetPathFrom(GridTile start, TilePath path)
    {
        var output = new List<GridTile>();
        var curPath = path;

        while(path.tile != start && curPath.path != null)
        {
            output.Add(curPath.tile);
            curPath = curPath.path;
        }

        output.Reverse();

#if UNITY_EDITOR
        for (int i = 0; i < output.Count - 1; i++)
        {
            Debug.DrawLine(output[i].transform.position + Vector3.up, output[i + 1].transform.position + Vector3.up, Color.red, 10f);
        }
#endif

        return output.ToArray();
    }

    public bool IsPathValid(GridTile from, GridTile to)
    {
        var path = GetPath(from, to, false, false);
        return path.FirstOrDefault(tile => !tile.IsTraversable) == null;
    }
    public bool IsPathClear(GridTile from, GridTile to)
    {
        var path = GetPath(from, to, false, false);
        return path.FirstOrDefault(tile => tile.IsBlock) == null;
    }

    public GridTile GetTile(int x, int y)
    {
        GridTile tile = null;
        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridLength - 1);

        tile = tiles[x, y];

        return tile;
    }
    public GridTile GetTile(Vector2 coords)
    {
        return GetTile((int)coords.x, (int)coords.y);
    }

    public GridTile[] GetTiles(GridTile from, Vector2 direction, int range)
    {
        var list = new List<GridTile>();

        if (from == null)
        {
            Debug.Log("From is empty!");
            return new GridTile[0];
        }

        Vector2 coords = from.Coords;
        for (int i = 0; i < range; i++)
        {
            coords += direction;
            if(coords.x < 0 || coords.x >= gridWidth || coords.y < 0  || coords.y >= gridLength)
            {
                break;
            }

            list.Add(GetTile(coords));
        }

        return list.ToArray();
    }
}
