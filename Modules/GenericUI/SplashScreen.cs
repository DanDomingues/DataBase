using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement ;

public class SplashScreen : MonoBehaviour {

    public float startDelay;
    public float fadeInTime;
    public float stayTime;
    public float fadeOutTime;
    public float endDelay;

    public string sceneToLoad;

    public   Image img; 
     



	// Use this for initialization
	void Start ()
    {

        StartCoroutine(UpdateSplash());

	}
	
	// Update is called once per frame
	IEnumerator UpdateSplash ()
    {

        img.color = new Color(1, 1, 1, 0 );
        yield return new WaitForSeconds(startDelay);

        while (img.color.a < 1)
        {
            img.color += Color.black * Time.deltaTime / fadeInTime ;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(stayTime);


        while (img.color.a >  0)
        {
            img.color -= Color.black *  Time.deltaTime / fadeOutTime ;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(endDelay);

         SceneManager.LoadScene(sceneToLoad);

    }
}
