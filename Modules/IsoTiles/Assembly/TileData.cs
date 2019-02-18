using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    [SerializeField] Sprite coreSprite;
    [SerializeField] Sprite[] adjacentSprites;

    public Sprite GetSprite(Vector2Int coords)
    {
        Sprite result = coreSprite;

        List<Vector2Int> list = new List<Vector2Int>()
        {
            new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0),
            new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1)
        };

        if(list.Contains(coords)) result = adjacentSprites[list.IndexOf(coords)];

        return result;
    }

}