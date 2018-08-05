using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovingGroup : MonoBehaviour
{
    RectTransform rect;

    public List<RectTransform> objects;
    [Range(0,500)]
    public float speed = 10;

    public float stopPoint = 10.0f;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransform reinsertedRect;
        float distance;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].anchoredPosition += Vector2.right * Time.deltaTime * speed;

            if(objects[i].anchoredPosition.x > stopPoint)
            {
                
                if(objects.Count > 1 && i > 0)
                {
                    distance = Mathf.Abs(objects[i].anchoredPosition.x - objects[i - 1].anchoredPosition.x);
                    objects[i].anchoredPosition = objects[0].anchoredPosition - (Vector2.right * distance);
                }

                reinsertedRect = objects[i];
                objects.RemoveAt(i);
                objects.Insert(0, reinsertedRect);

                i++;
            }
        }
    }

}
