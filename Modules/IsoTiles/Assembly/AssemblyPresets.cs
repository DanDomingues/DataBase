using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssemblyPresets", menuName = "Data/AssemblyPresets")]
public class AssemblyPresets : ScriptableObject
{
    public Sprite defaultTile;
    public TileData[] tiles;
}
