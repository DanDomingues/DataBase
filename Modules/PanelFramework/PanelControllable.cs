using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControllable : PanelBase
{
    [SerializeField] public bool inputEnabled;

    protected Vector2Int lastMovementInt;
    protected Vector2Int currentMovementInt;

    protected Vector2 MovementInput { get { return GenericInputs.MovementRaw; } }
    protected virtual Vector2Int MovementInputInt
    {
        get
        {
            var raw = GenericInputs.MovementRawInt;// + ControllerInputs.DPad;
            return new Vector2Int(Mathf.Clamp(raw.x, -1, 1), Mathf.Clamp(raw.y, -1, 1));
        }
    }

    protected int BumperAxis => UI_Inputs.SwitchCategoryInt;
    protected bool Accept => UI_Inputs.Select;

    public virtual bool InputAllowed => true;
    public virtual bool InputEnabled => inputEnabled && !DevConsole.Instance.Active;

    protected override void Start()
    {
        base.Start();

        OnPanelToggleLate += SetInputEnabled;
    }
    protected virtual void Update()
    {
        currentMovementInt = new Vector2Int
            (lastMovementInt.x == 0 ? MovementInputInt.x : 0,
             lastMovementInt.y == 0 ? MovementInputInt.y : 0);

        if (Active && InputEnabled && !InTransition)
        {
            ActiveUpdate();
            lastMovementInt = MovementInputInt;
        }

    }
    protected virtual void LateUpdate()
    {

    }

    public virtual void SetInputEnabled(bool value)
    {
        SetInputEnabledRaw(value && InputAllowed);
    }
    public virtual void SetInputEnabledRaw(bool value)
    {
        StartCoroutine(WaitForFrame(() =>
        {
            inputEnabled = value;
            group.blocksRaycasts = inputEnabled;
        }));
    }

    public override void Toggle(bool value)
    {
        base.Toggle(value);
        if (!value) SetInputEnabled(false);
    }

    public void GrabInputs(PanelControllable reference)
    {
        currentMovementInt = reference.currentMovementInt;
        lastMovementInt = reference.lastMovementInt;
    }

    IEnumerator WaitForFrame(UnityEngine.Events.UnityAction action)
    {
        yield return new WaitForEndOfFrame();
        if(action != null) action();
    }

    protected virtual void ActiveUpdate() { }

}
