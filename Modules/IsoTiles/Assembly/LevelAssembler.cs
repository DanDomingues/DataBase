using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAssembler : MonoBehaviour
{
    static LevelAssembler instance;
    public static LevelAssembler Instance { get { return instance; } }


    [Header("Base Assembly")]
    [SerializeField] AssemblyPresets presets;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform pivot;
    [SerializeField] Vector2Int levelSize;

    [Header("Special Tiles")]
    [SerializeField] TileType specialTileType;
    [SerializeField] int minSpecialTiles;
    [SerializeField] int maxSpecialTiles;
    [SerializeField, Tooltip("Chance for the special tile to blend into an adjacent tile"), Range(0f, 1f)] 
    float specialTileSpread;
    [SerializeField, Tooltip("Chance for the special tile to continue to an adjacent tile"), Range(0f, 1f)] 
    float specialTileLoop;

    Tile[,] tiles;

    public Tile[,] Tiles{ get { return tiles; } }
    public Vector2Int LevelSize{ get { return levelSize; } }

    public delegate void AssemblyEvent();
    public AssemblyEvent OnAssemblyFinish;

    [Header("Debug (Runtime)")]
    [SerializeField] float multiplier;
    [SerializeField] Vector2 vectorMultiplier;
    [SerializeField, Tooltip("If left on, the grid will adjust it's spacing to the multiplier in runtime." + 
    " All characters are parented to their current tiles, and will move along with them.")] 
    bool adjustOnGizmos;

    bool setupDone;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AssembleLevel());
    }

    void Reset()
    {
        if(levelSize.x <= 0 || levelSize.y <= 0) levelSize = new Vector2Int(5, 5);
        if(pivot == null) pivot = transform;

        specialTileType = TileType.Stone;
        if(minSpecialTiles > maxSpecialTiles || minSpecialTiles <= 0) minSpecialTiles = 1;
        if(maxSpecialTiles < minSpecialTiles) maxSpecialTiles = 4;
        specialTileLoop = 0.7f;
        specialTileSpread = 0.7f;

        //adjustOnGizmos = false;
        if(multiplier < 0.1f) multiplier = 1f;        
        var x = vectorMultiplier.x < 0.1f ? 1f : vectorMultiplier.x;
        var y = vectorMultiplier.y < 0.1f ? 1f : vectorMultiplier.y;
        vectorMultiplier = new Vector2(x, y);
    }

    IEnumerator AssembleLevel()
    {

        var width = presets.defaultTile.bounds.size.x * 0.5f * multiplier;
        var startPos = pivot.position - new Vector3(((levelSize.x - levelSize.y) * width) / 2, - ((levelSize.y * width * 0.5f) / 2));
        Vector3 pos;
        GameObject obj;

        tiles = new Tile[levelSize.x, levelSize.y];
        for (int y = 0; y < levelSize.y; y++)
        {
            for (int x = 0; x < levelSize.x; x++)
            {
                pos = startPos + new Vector3((x - y) * width, (x + y) * width * -0.5f, 0);
                obj = Instantiate(tilePrefab, pos, Quaternion.identity);
                obj.name = string.Format("Tile_{0}-{1}", x + 1, y + 1);
                obj.transform.parent = pivot;

                tiles[x, y] = obj.GetComponent<Tile>();
                tiles[x, y].SetCoords(new Vector2Int(x, y));
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(AddLevelFeatures_Routine());

        yield return new WaitForEndOfFrame();        
        if(OnAssemblyFinish != null) OnAssemblyFinish();
        setupDone = true;

    }

    IEnumerator AddLevelFeatures_Routine()
    {
        int specialTilesCount = Random.Range(minSpecialTiles, maxSpecialTiles + 1);
        Vector2Int coords = Vector2Int.zero;
        Vector2Int[] neighbors;
        Tile tile;
        bool resetCoords = true;
        for (int index = 0; index < specialTilesCount; index++)
        {
            if(resetCoords)
                coords = new Vector2Int(Random.Range(0, levelSize.x), Random.Range(0, levelSize.y));

            tiles[coords.x, coords.y].SetTile(presets.tiles[0].GetSprite(Vector2Int.zero), specialTileType);

            neighbors = TileUtility.SurroundingCoords(coords, levelSize);
            Vector2Int dir;
            for (int i = 0; i < neighbors.Length; i++)
            {
                tile = tiles[neighbors[i].x, neighbors[i].y];
                if(Random.Range(0, 1f) < specialTileSpread && tile.type != specialTileType)
                {
                    dir = neighbors[i] - coords;
                    dir = new Vector2Int(dir.x, dir.y * -1);
                    tile.SetTile(presets.tiles[0].GetSprite(dir), specialTileType);
                }
            }

            resetCoords = Random.Range(0, 1f) < specialTileLoop;
            if(!resetCoords)
            {
                index--;
                coords = neighbors[Random.Range(0, neighbors.Length)];
            } 

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Returns the tile at the determined coords
    /// If out of bounds, return a null reference
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    public Tile GetTile(Vector2Int coords)
    {
        return CheckTileBounds(coords) ? tiles[coords.x, coords.y] : null;
    }
    public Tile[] GetTiles(Vector2Int[] coords)
    {
        var list = new List<Tile>();
        for (int i = 0; i < coords.Length; i++)
        {
            if(CheckTileBounds(coords[i]))
            {
                list.Add(tiles[coords[i].x, coords[i].y]);
            }
        }

        return list.ToArray();
    }

    public bool CheckTileBounds(Vector2Int coords)
    {
        return (coords.x >= 0 && coords.x < levelSize.x) && (coords.y >= 0 && coords.y < levelSize.y);
    }

    public IEnumerator ResetLevel_Routine()
    {
        for (int y = 0; y < levelSize.y; y++)
        {
            for (int x = 0; x < levelSize.x; x++)
            {
                tiles[x, y].SetTile(presets.defaultTile, TileType.Grass);
            }

        }

        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(AddLevelFeatures_Routine());
    }

    void OnDrawGizmosSelected()
    {
        if(adjustOnGizmos && setupDone)
        {
            var width = presets.defaultTile.bounds.size.x * 0.5f * multiplier;
            var startPos = pivot.position - new Vector3(((levelSize.x - levelSize.y) * width) / 2, - ((levelSize.y * width * 0.5f) / 2));
            Vector3 pos;

            for (int y = 0; y < levelSize.y; y++)
            {
                for (int x = 0; x < levelSize.x; x++)
                {
                    pos = startPos + new Vector3((x - y) * width * vectorMultiplier.x, (x + y) * width * -0.5f * vectorMultiplier.y, 0);
                    tiles[x, y].transform.position = pos;
                    if(!tiles[x, y].IsVacant)
                    {
                        tiles[x, y].Character.transform.parent = tiles[x, y].transform;
                    }
                }

            }
        }
    }

}
