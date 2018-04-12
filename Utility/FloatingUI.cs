using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FloatingUI : MonoBehaviour
{
    public float Factor { get { return factor; } }

    [Header("Motion")]
    [SerializeField]
    AnimationCurve motionCurve;
    [SerializeField, Range(0.1f, 1080f), Tooltip("Reference for the curve, where 1 = movementAmplitude")]
    float movementAmplitude = 100.0f;
    [SerializeField, Tooltip("Delay before starting up")]
    float startDelay = 0.0f;

    [Header("Duration")]
    [SerializeField, Range(0.1f, 10.0f)]
    float duration = 1.0f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("Random modifier ranging from 0 to value, where 1 doubles the duration")]
    float randomDurationOffset;
    [SerializeField, Tooltip("If enabled, offset will range from (minus) -value to value")]
    bool offsetCanBeNegative;

    [Header("Interval")]
    [SerializeField, Range(0.0f, 3.0f), Tooltip("Time that will be surely waited between full movements")]
    float baseInterval = 0.2f;
    [SerializeField, Range(0.0f, 3.0f), Tooltip("Random time (from 0 to value) to be added on top of the base interval")]
    float randomIntervalAdd = 0.2f;
    [SerializeField, Tooltip("If active, interval times will be multiplied by the 'duration' variable")]
    bool intervalUsesDuration;

    private float factor;
    private RectTransform rect;
    private Vector2 startPosition;

    Coroutine moveCorout;

	// Use this for initialization
	void Start ()
    {
        rect = GetComponent<RectTransform>();
        startPosition = rect.anchoredPosition;
	}

    private void OnEnable()
    {
        InvokeAlternatives.Invoke(this, startDelay, () =>
        {
            moveCorout = StartCoroutine(Move_Routine());
        });

    }

    private void OnDisable()
    {
        StopCoroutine(moveCorout);
    }

    IEnumerator Move_Routine()
    {
        float curveDuration = motionCurve.GetLength();
        float offset = Random.Range(offsetCanBeNegative ? -randomDurationOffset : 0.0f, randomDurationOffset);
        float movementDuration = duration + offset;

        float t = 0;

        while(t < 1.0f)
        {
            factor = motionCurve.Evaluate(t * curveDuration);
            rect.anchoredPosition = startPosition + (Vector2.up * factor * movementAmplitude);

            t += Time.deltaTime / movementDuration;
            yield return new WaitForEndOfFrame();
        }

        float interval = baseInterval + Random.Range(0.0f, randomIntervalAdd);
        if (intervalUsesDuration) interval *= duration;

        yield return new WaitForSeconds(interval);

        moveCorout = StartCoroutine(Move_Routine());

    }


}
