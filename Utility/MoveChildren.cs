using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChildren : MonoBehaviour {

    public enum MovementType { vertical}
    public MovementType movementType;

    private Vector3[] startPositions;
    
    [Range(0.0f, 1.0f)]
    public float scaleFactor;

    [Range(0.0f, 5.0f)]
    public float timeFactor;

    public AnimationCurve curve;

	// Use this for initialization
	void Start ()
    {
        startPositions = new Vector3[transform.childCount];
		for(int i = 0; i < transform.childCount;i++)
        {
            startPositions[i] = transform.GetChild(i).localPosition;
        }

        //StartCoroutine(VerticalCycle(startSpeed, acceleration));
	}

    private void OnDisable()
    {
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float maxTime = curve.keys[curve.keys.Length - 1].time / timeFactor;

        float point = Time.time % maxTime;

        Vector3 offset = Vector3.up * curve.Evaluate(point * timeFactor) * scaleFactor;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = startPositions[i] + offset;
        }

    }

}
