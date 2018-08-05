using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageBehaviour : StateMachineBehaviour
{
    [Header("Entry")]
    [SerializeField] bool entryActive;
    [SerializeField] string entryMessage;
    [SerializeField] float entryDelay;

    [Header("End")]
    [SerializeField] bool endActive;
    [SerializeField] string endMessage;
    [SerializeField] float endDelay;

    [Header("Exit")]
    [SerializeField] bool exitActive;
    [SerializeField] string exitMessage;
    [SerializeField] float exitDelay;

    [SerializeField]
    Coroutine corout;
    [SerializeField, HideInInspector]
    bool endCalled;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (entryActive) SendMessage(animator, entryMessage, entryDelay);

        endCalled = false;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateInfo.normalizedTime >= stateInfo.length && endActive && !endCalled)
        {
            endCalled = true;
            SendMessage(animator, endMessage, endDelay);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (exitActive)
        {
            SendMessage(animator, exitMessage, exitDelay);
        }
        else if (corout != null)
        {
            Debug.Log("Routine stopped");
            animator.GetComponent<MonoBehaviour>().StopCoroutine(corout);
            corout = null;
        }

    }


    void SendMessage(Animator obj, string message, float delay)
    {
        if (corout != null)
        {
            Debug.Log("Routine aborted");
            return;
        }

        corout = InvokeAlternatives.Invoke(obj.GetComponent<MonoBehaviour>(), delay, () =>
        {
            obj.SendMessage(message);
            corout = null;
        });

        //Debug.Log("Exit called");
    }
}
