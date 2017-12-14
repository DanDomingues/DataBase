using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI   ;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public RectTransform rect;
    public float waitTime;
    public float endTime;
    public float speed;
    public float target;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(creditsRoll());
        target = FindObjectOfType<CanvasScaler>().referenceResolution.y;
    }

    IEnumerator creditsRoll()
    {

        yield return new WaitForSeconds(waitTime);

        while(rect.anchoredPosition.y < target + rect.sizeDelta.y)
        {
            rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(endTime);

        SceneManager.LoadScene(0);
    }

}
