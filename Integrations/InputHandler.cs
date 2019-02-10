using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Input class for handling cross-platform swipe inputs
/// For it's best use, attach the script to the image to be used as the input layer, within a Canvas
/// If no image is found, the script will add the components and set to all invisble, 16:16 UI image
/// </summary>

public class InputHandler : MonoBehaviour 
{	

	[Header("Runtime")]
	[SerializeField] float movementLength;
	[SerializeField] Vector2 direction;
	[SerializeField] Vector2 distance;
	[SerializeField] Vector2 pointerPos;
	[SerializeField] bool isDragging;

	public float MovementLength { get { return movementLength; } }

	public Vector2 PointerPositionRaw
	{
		get
		{
			if(Input.touches.Length > 0) return Input.touches[0].position;
			return Input.mousePosition;
		}
	}
	public Vector2 PointerPosition
	{
		get
		{
			var input = PointerPositionRaw;
			return new Vector2((input.x / Screen.width) * 1080f, (input.y / Screen.height) * 1920f);

		}
	}

	void Reset()
	{
		var rect = GetComponent<RectTransform>();
		if(rect == null)
		{
			throw new System.InvalidOperationException("Input Handler is placed at a non-canvas object and will not function properly unless attached to a rect transform object");
		}

		var image = GetComponent<Image>();
		if(image == null)
		{
			image = gameObject.AddComponent<Image>();
			image.color = Color.clear;
		} 	
	}

	void Start()
	{
		var trigger = GetComponent<EventTrigger>();
		if(trigger == null)
		{
			trigger = gameObject.AddComponent<EventTrigger>();
			SubscribeEvent(trigger, OnDragStart, EventTriggerType.BeginDrag);
			SubscribeEvent(trigger, OnDrag, EventTriggerType.Drag);
			SubscribeEvent(trigger, OnDragEnd, EventTriggerType.EndDrag);

		}
	}

	void SetEvents()
	{
		var trigger = GetComponent<EventTrigger>();
		if(trigger == null)
		{
			trigger = gameObject.AddComponent<EventTrigger>();
			SubscribeEvent(trigger, OnDragStart, EventTriggerType.BeginDrag);
			SubscribeEvent(trigger, OnDrag, EventTriggerType.Drag);
			SubscribeEvent(trigger, OnDragEnd, EventTriggerType.EndDrag);

		}
	}

	void SubscribeEvent(EventTrigger trigger, System.Action action, EventTriggerType type)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = type;
		entry.callback.AddListener( (eventData) => { action(); } );
		trigger.triggers.Add(entry);

	}

	public void OnDragStart()
	{
		pointerPos = PointerPositionRaw;
		isDragging = true;
	}

	public void OnDrag()
	{
		distance = PointerPositionRaw - pointerPos;
		direction = distance.normalized;
		movementLength = distance.magnitude;
	}

	public void OnDragEnd()
	{
		isDragging = false;
	}

}
