using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{

    public static IEnumerator LerpImg(Image img, Color input, float rate, float limit)
    {

        //if(input == Color.clear) Debug.Log("Started");

        while (Mathf.Abs(img.color.r - input.r) > limit)
        {
            img.color = Color.Lerp(img.color, input, Time.deltaTime * rate);
            Debug.Log("Lerping Image to set Color");
            yield return new WaitForEndOfFrame();
        }

        img.color = input;

        //if (input == Color.clear) Debug.Log("Done");


    }
    public static IEnumerator LerpImg(Image img, float rate, float limit)
    {
        yield return new WaitForEndOfFrame();

        while (img.color.a > 0)
        {

            yield return new WaitForEndOfFrame();
            img.color -= Color.white * Time.deltaTime * rate;
            Debug.Log("Lerping Image to Clear");
        }

        img.color = Color.clear;

    }

    public static IEnumerator LerpSprite(SpriteRenderer sprite, Color input, float rate, float limit)
    {

        //if(input == Color.clear) Debug.Log("Started");

        while (Mathf.Abs(sprite.color.r - input.r) + Mathf.Abs(sprite.color.g - input.g)
            + Mathf.Abs(sprite.color.b - input.b) + Mathf.Abs(sprite.color.a - input.a) > limit)
        {
            sprite.color = Color.Lerp(sprite.color, input, Time.deltaTime * rate);

            yield return new WaitForEndOfFrame();

        }

        sprite.color = input;

        //if (input == Color.clear) Debug.Log("Done");


    }
    public static IEnumerator LerpSprite(SpriteRenderer sprite, float rate, float limit)
    {
        yield return new WaitForEndOfFrame();

        while (sprite.color.r < 1 &&   sprite.color.g < 1 
              && sprite.color.b < 1 && sprite.color.a < 1)
        {

            sprite.color += Color.white * Time.deltaTime * rate;
            yield return new WaitForEndOfFrame();

        }

        sprite.color = Color.white;

    }

}
