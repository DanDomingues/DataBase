using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger), typeof(Graphic))]
public class ScaleButton : MonoBehaviour 
{

	[SerializeField] float defaultScale;
	[SerializeField] float pressedScale;
	public UnityEvent onClick;
	EventTrigger trigger;

	Vector3 startScale;

	// Use this for initialization
	void Start () 
	{
		trigger = GetComponent<EventTrigger>();
		SubscribeEvent(SetScaleToPressed, EventTriggerType.PointerDown);
		SubscribeEvent(ResetScale, EventTriggerType.PointerUp);
		SubscribeEvent(() =>
		{
			InvokeAlternatives.Invoke(this, 0.15f, onClick.SafeInvoke);
		}, EventTriggerType.PointerClick);

		startScale = transform.localScale;
	}

	void Reset()
	{
		defaultScale = Mathf.Abs(((Vector2) transform.localScale).x);
		pressedScale = 0.85f;
		var button = GetComponent<Button>();
		if(button)
		{
			onClick = button.onClick;
		}
	}

	void SubscribeEvent(System.Action action, EventTriggerType type)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = type;
		entry.callback.AddListener( (eventData) => { action(); } );
		trigger.triggers.Add(entry);

	}

	void SetScaleToPressed()
	{
		transform.localScale = startScale * pressedScale;
	}

	void ResetScale()
	{
		transform.localScale = startScale * defaultScale;
	}
	
}
