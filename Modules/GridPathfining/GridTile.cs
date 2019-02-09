using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField]
    bool isTraversable;
    [SerializeField]
    bool isHazard;
    [SerializeField]
    bool isBlock;
    [SerializeField]
    GridTile[] neighbors;
    [SerializeField]
    GridTile[] adjacentTiles;

    [SerializeField] PieceBase localPiece;

    public GridTile[] Neighbors { get { return neighbors; } }
    public GridTile[] AdjacentTiles { get { return adjacentTiles; } }

    public bool IsHarzard { get { return isHazard; } }
    public bool IsTraversable { get { return (isTraversable && !IsBlock) && LocalPiece == null; } }
    public bool IsBlock { get { return isBlock; } }

    public PieceBase LocalPiece { get { return localPiece; } }

    public Vector2 Coords
    {
        get
        {
            var coords = name.Split('_')[1].Split('-');
            int x = int.Parse(coords[0]) - 1;
            int y = int.Parse(coords[1]) - 1;

            return new Vector2(x, y);
        }
    }

    public void Setup(float width)
    {
        GetNeighbors(width);
    }

    void GetNeighbors(float width)
    {
        var directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
            
            new Vector3(1, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(-1, 0, -1),
            new Vector3(-1, 0, 1)
        };

        var flightDistance = 5f;
        var startPoint = transform.position + (Vector3.up * flightDistance);

        var newTilesFull = new List<GridTile>();
        var newTiles = new List<GridTile>();

        GridTile tile;
        RaycastHit hit;
        for(int i = 0; i < directions.Length; i++)
        {
            if(Physics.Raycast(startPoint + (directions[i] * width), Vector3.down, out hit, flightDistance * 3))
            {
                tile = hit.transform.GetComponent<GridTile>();
                if(tile != null)
                {
                    if(i < 4)
                    {
                        newTiles.Add(tile);
                    }
                    newTilesFull.Add(tile);
                }
            }
        }

        neighbors = newTiles.ToArray();
        adjacentTiles = newTilesFull.ToArray();
    }

    public void SetLocalPiece(PieceBase piece)
    {
        localPiece = piece;
    }

    public void SetIsBlocked(bool value)
    {
        isBlock = value;
    }
}
