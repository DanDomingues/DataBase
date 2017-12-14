using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ControllerPanel : PanelBase
{

    public bool controllerSupport = true;
    public int vIndex;
    public int vWay;

    //[HideInInspector]
    public bool active;
    public bool buttonActive;

    public Selectable[] selectables;

    protected override void Start()
    {
        base.Start();
        MoveSelection(ref vIndex, 0, selectables);
    }

    protected virtual void Update()
    {

        if(active)
        {
            if(CloseInput())
            {
                Toggle();
            }

        }
    }

    protected virtual void LateUpdate()
    {

        if (!controllerSupport || !active) return;

        int newWay = Mathf.Clamp ((int) (Input.GetAxisRaw("Vertical") * 2.0f), -1 , 1); 
        if (newWay != vWay && newWay != 0)
        {
            MoveSelection(ref vIndex, -newWay, selectables);
        }
        vWay = newWay;

        if(InputCentral.confirm || Input.GetKeyDown(KeyCode.Return))
        {
            HighlightSelectable(vIndex);
        }
        else if ((InputCentral.confirmReleased || Input.GetKeyUp(KeyCode.Return)) && buttonActive)
        {
            TriggerSelectable(vIndex);
        }

    }

    public override void Toggle()
    {
        if(transitionCorout == null)
        {
            active = !group.blocksRaycasts;
            if (active)
            {
                vIndex = 0;
                MoveSelection(ref vIndex, 0, selectables);
            }
        }
        base.Toggle();

    }

    protected virtual void HighlightSelectable(int index)
    {
        Button b = selectables[index].GetButton();
        if (b != null)
        {
            if(b.transition == Selectable.Transition.SpriteSwap)
            {
                b.image.sprite = b.spriteState.pressedSprite;
                buttonActive = true;
            }
            else
                ExecuteSelectableOnClick(vIndex);
        }
    }

    protected virtual void TriggerSelectable(int index)
    {
        Button b = selectables[index].GetButton();
        if (b != null)
        {
            if(b.transition == Selectable.Transition.SpriteSwap)
            {
                b.image.sprite = b.spriteState.highlightedSprite;
                ExecuteSelectableOnClick(vIndex);
                buttonActive = false;
            }

        }
    }

    protected virtual bool ToggleInput()
    {
        return false;
    }

    protected virtual bool CloseInput()
    {
        return (InputCentral.back || InputCentral.start || Input.GetKeyDown(KeyCode.Escape)) && group.blocksRaycasts;
    }

    protected void MoveSelection(ref int index, int vWay, Selectable[] selectables)
    {
        if (selectables.Length == 0) return;

        int newIndex =  Mathf.Clamp(index + vWay, 0, selectables.Length - 1); ;
        buttonActive = false;

        Button curButton = null;
        for (int i = 0; i < selectables.Length; i++)
        {
            curButton = selectables[i].GetButton();
            if (curButton != null)
            {
                if (curButton.transition == Selectable.Transition.SpriteSwap)
                    selectables[i].enabled = !controllerSupport;
            }

            selectables[i].interactable = (i == newIndex);
        }

        if(curButton != null)
        {
            selectables[index].SetButtonState(ButtonState.Disabled);
            selectables[newIndex].SetButtonState(ButtonState.Highlighted);
        }

        if (vWay != 0) SoundManager.instance.PlaySingle(profile.selectSound);
        index = newIndex;
    }

    protected void ExecuteSelectableOnClick(int index)
    {
        index = Mathf.Clamp(index, 0, selectables.Length);

        if (selectables[index].GetButton() != null)
            selectables[index].GetButton().onClick.SafeInvoke();

        active = false;

    }

}
