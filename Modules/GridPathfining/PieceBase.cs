using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceBase : MonoBehaviour
{
    public GridTile CurrentTile { get { return currentTile; } }

    [SerializeField]
    protected Vector2 startCoords;
    [SerializeField]
    public Vector2 lookDirection;

    [SerializeField] protected GridTile currentTile;

    public bool IsHookable;
    public Coroutine moveRoutine;

    protected virtual void Start()
    {
        GridHandler.Instance.OnSetupDone += OnSetupDone;
    }

    void OnSetupDone()
    {
        var tile = GridHandler.Instance.GetTile(startCoords);
        transform.position = tile.transform.position;
        SetCurrentTile(tile);
    }

    public virtual void SetCurrentTile(GridTile value)
    {
        if(currentTile != null)
            currentTile.SetLocalPiece(null);

        if(value != null)
        {
            if(value.IsHarzard)
            {
                value.SetLocalPiece(null);

                if (moveRoutine != null) StopCoroutine(moveRoutine);

                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                value.SetLocalPiece(this);
            }
        }

        currentTile = value;
    }

}
