using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{

    [SerializeField]
    FloatAnimField entry;
    [SerializeField]
    FloatAnimField end;
    [SerializeField]
    FloatAnimField exit;

    [SerializeField]
    Coroutine corout;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entry.active)
            SetFloatField(animator, entry);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateInfo.normalizedTime >= stateInfo.length && end.active)
        {
            SetFloatField(animator, end);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(exit.active)
            SetFloatField(animator, exit);

        else if (corout != null)
        {
            animator.GetComponent<MonoBehaviour>().StopCoroutine(corout);
            corout = null;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    void SetFloatField(Animator obj, FloatAnimField field)
    {
        if (field.delay > 0f)
        {
            if(corout == null)
            {
                corout = InvokeAlternatives.Invoke(obj.GetComponent<MonoBehaviour>(), field.delay, () =>
                {
                    obj.SetFloat(field.name, field.value);
                    corout = null;
                });

            }

        }
        else
            obj.SetFloat(field.name, field.value);

        //Debug.Log("Exit called");
    }

    [System.Serializable]
    public struct FloatAnimField
    {
        public bool active;
        public string name;
        public float value;
        public float delay;
    }

}
