using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSwapButton : MonoBehaviour, ISelectable
{
    [SerializeField] Button button;
    [SerializeField] AudioClip pressedSfx;
    [SerializeField] GameObject defaultState;
    [SerializeField] GameObject selectedState;
    [SerializeField] GameObject pressedState;

    int currentState;
    bool AcceptIsPressed
    {
        get
        {
            return Input.GetKey(KeyCode.Return);
        }
    }

    private void Reset()
    {
        button = GetComponentInChildren<Button>();
    }

    private void OnDisable()
    {
        SetState(0);
    }

    void SetState(int value)
    {
        defaultState.SetActive(value == 0);
        selectedState.SetActive(value == 1);
        pressedState.SetActive(value == 2);

        currentState = value;
    }

    public void Select(bool value)
    {
        SetState(value ? 1 : 0);
    }
    public void RunButtonAction()
    {
        SetState(2);
        //SoundManager.Instance.PlaySingle(pressedSfx);

        StartCoroutine(InvokeRealtime(0.1f, () =>
        {
            button.onClick?.Invoke();
        }));

        StartCoroutine(ResetButton_Routine());
    }

    IEnumerator ResetButton_Routine()
    {
        yield return new WaitWhile(() => AcceptIsPressed);
        if(currentState == 2)
        {
            SetState(1);
        }
    }

    IEnumerator InvokeRealtime(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (action != null) action();
    }

}
