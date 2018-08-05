using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioBehaviour : StateMachineBehaviour
{

    [SerializeField]
    PlayAudioField entry;
    [SerializeField]
    PlayAudioField end;
    [SerializeField]
    PlayAudioField exit;

    [SerializeField]
    Coroutine corout;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entry.active)
            SetAudioField(animator, entry);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateInfo.normalizedTime >= stateInfo.length && end.active)
        {
            SetAudioField(animator, end);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(exit.active)
            SetAudioField(animator, exit);

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

    void SetAudioField(Animator obj, PlayAudioField field)
    {
        if (field.delay > 0f)
        {
            if(corout == null)
            {
                corout = InvokeAlternatives.Invoke(obj.GetComponent<MonoBehaviour>(), field.delay, () =>
                {
                    SoundManager.instance.PlaySingle(field.value);
                    corout = null;
                });

            }

        }
        else
            SoundManager.instance.PlaySingle(field.value);

        //Debug.Log("Exit called");
    }

    [System.Serializable]
    public struct PlayAudioField
    {
        public bool active;
        public string name;
        public AudioClip value;
        public float delay;
    }

}
