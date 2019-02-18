using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType type;
    [SerializeField] SpriteRenderer render;
    [SerializeField] SpriteRenderer[] highlights;
    
    Vector2Int coords;
    [SerializeField] CharacterBase character;

    public Vector2Int Coords{ get { return coords; } }
    public bool IsVacant{ get { return character == null; } }
    public CharacterBase Character{ get { return character; } }

    public Tile[] SurroundingTiles
    {
        get
        {
            return LevelAssembler.Instance.GetTiles(TileUtility.SurroundingCoords(coords));
        }
    }
    public Tile[] NeighborTiles
    {
        get
        {
            return LevelAssembler.Instance.GetTiles(TileUtility.NeighborCoords(coords));
        }
    }
    
    public void SetTile(Sprite sprite, TileType type)
    {
        render.sprite = sprite;
        this.type = type;
    }

    public void SetCoords(Vector2Int coords)
    {
        this.coords = coords;
    }

    public void SetOccupyingCharacter(CharacterBase character)
    {
        this.character = character;
    }

    public void SetHighlight(bool active, Color color)
    {
        SetHighlightActive(active);
        SetHighlightColor(color);
    }
    public void SetHighlightActive(bool active)
    {
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].enabled = active;
        }
    }
    public void SetHighlightColor(Color color)
    {
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].color = color;
        }
    }

}
