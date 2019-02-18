using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileUtility
{
    public static Vector2Int[] SurroundingCoords(Vector2Int startPoint)
    {
        var array = new Vector2Int[]
        {
            new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0),
            new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1)
        };

        for (int i = 0; i < array.Length; i++)
        {
            array[i] += startPoint;
        }

        return array;
    }
    public static Vector2Int[] SurroundingCoords(Vector2Int startPoint, Vector2Int bounds)
    {
        var list = new List<Vector2Int>(SurroundingCoords(startPoint));
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].x < 0 || list[i].x >= bounds.x || list[i].y < 0 || list[i].y >= bounds.y)
            {
                list.RemoveAt(i);
                i--;
            }
        }

        return list.ToArray();
    }
    public static Vector2Int[] SurroundingCoords()
    {
        return SurroundingCoords(Vector2Int.zero);
    }

    public static Vector2Int[] NeighborCoords(Vector2Int startPoint)
    {
        var array = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        for (int i = 0; i < array.Length; i++)
        {
            array[i] += startPoint;
        }

        return array;
    }
    public static Vector2Int[] NeighborCoords(Vector2Int startPoint, Vector2Int bounds)
    {
        var list = new List<Vector2Int>(NeighborCoords(startPoint));
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].x < 0 || list[i].x >= bounds.x || list[i].y < 0 || list[i].y >= bounds.y)
            {
                list.RemoveAt(i);
                i--;
            }
        }

        return list.ToArray();
    }
    public static Vector2Int[] NeighborCoords()
    {
        return NeighborCoords(Vector2Int.zero);
    }

    public static Vector2Int[] GetSurroundingCoords(Vector2Int coreCoords, int radiusDistance, bool useDiagonals)
    {
        var prevList = new List<Vector2Int>() { coreCoords };
        var output = new List<Vector2Int>() { coreCoords };
        var tempList = new List<Vector2Int>();

        Vector2Int[] addedCoords;
        for (int i = 0; i < radiusDistance; i++)
        {
            tempList = new List<Vector2Int>();

            for (int j = 0; j < prevList.Count; j++)
            {
                addedCoords = useDiagonals ? SurroundingCoords(prevList[j]) : NeighborCoords(prevList[j]);
                tempList.AddRange(addedCoords.Where(coord => !output.Contains(coord)));
            }

            prevList = new List<Vector2Int>(tempList);
            output.AddRange(tempList);
        }

        return output.ToArray();
    }
}
