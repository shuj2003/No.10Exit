using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenFade : Common
{

    public AnimationCurve fadeCurve;
    public GameObject image;

    public void FadeIn(Action action)
    {
        var img = image.GetComponent<Image>();
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, 0f);
        StartCoroutine(FadeImage(image.GetComponent<Image>(), 0f, 1f, 1f, fadeCurve, delegate () {
            if (action != null) action();
        }));
    }

    public void FadeOut(Action action)
    {
        var img = image.GetComponent<Image>();
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, 1f);
        StartCoroutine(FadeImage(image.GetComponent<Image>(), 1f, 0f, 1f, fadeCurve, delegate () {
            if (action != null) action();
        }));
    }

}
