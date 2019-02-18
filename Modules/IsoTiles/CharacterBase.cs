using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected float movementDuration;
    [SerializeField] protected AnimationCurve movementCurve;
    [SerializeField] protected Vector3 positionOffset;
    [SerializeField] protected Color highlightColor;

    [Header("Runtime")]
    [SerializeField] protected Vector2Int currentCoords;
    [SerializeField] protected bool active;
    protected Coroutine moveRoutine;

    public bool IsMoving{ get { return moveRoutine != null; } }
    public Tile CurrentTile{ get { return LevelAssembler.Instance.GetTile(currentCoords); } }

    protected virtual void OnEnable()
    {
        LevelAssembler.Instance.OnAssemblyFinish += OnSetupComplete;
        
        /* Subscribes OnMatchFinish to external delegate event, to be run when the game finishes.
        By default, disables the character, allowing for a game over or victory sequence with no interruptions.*/
    
        //GameplayEvents.Instance.OnMatchFinish += OnMatchFinish;
    }

    protected virtual void Reset()
    {
        movementDuration = 0.5f;
        movementCurve = new AnimationCurve();
        movementCurve.AddKey(new Keyframe(0f, 0f, 0f, 2f));
        movementCurve.AddKey(new Keyframe(1f, 1f, 0f, 2f));
        //movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    protected virtual void OnSetupComplete()
    {
        SetActive(true);
    }

    /// <summary>
    /// Default subscription method for when the game finishes. At base level, disables the character.
    /// </summary>
    protected virtual void OnMatchFinish()
    {
        SetActive(false);
    }

    public virtual void SetActive(bool value)
    {
        active = value;
    }

    public virtual void MoveToTile(Tile tile, bool useTransition)
    {
        if(tile == null) return;
        if(!tile.IsVacant) return;

        var currentTile = LevelAssembler.Instance.GetTile(currentCoords);
        currentTile.SetOccupyingCharacter(null);
        tile.SetOccupyingCharacter(this);

        var target = tile.transform.position + positionOffset;
        SetCurrentCoords(tile.Coords, useTransition);

        if(useTransition)
        {
            moveRoutine = StartCoroutine(MoveToTile_Routine(target));
        }
        else
        {
            transform.position = target;
        }
    }
    public void MoveToTile(Vector2Int coords, bool useTransition)
    {
        MoveToTile(LevelAssembler.Instance.GetTile(coords), useTransition);
    }

    protected virtual void SetCurrentCoords(Vector2Int coords, bool usingTransition)
    {
        //LevelAssembler.Instance.GetTile(currentCoords).SetHighlightActive(false);
        //LevelAssembler.Instance.GetTile(coords).SetHighlight(true, highlightColor);
        currentCoords = coords;
    }

    protected virtual IEnumerator MoveToTile_Routine(Vector3 target)
    {
        var start = transform.position;
        var t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime / movementDuration;
            t = Mathf.Clamp01(t);
            transform.position = Vector3.Lerp(start, target, movementCurve.Evaluate(t));

            yield return new WaitForEndOfFrame();
        }

        moveRoutine = null;
    }

}
