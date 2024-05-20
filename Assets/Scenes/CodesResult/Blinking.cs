using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking : Common
{
    public AnimationCurve legCurve;

    public Text text;
    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponentsInChildren<Text>(true)[0];
        StartCoroutine(BlinkingOff());
    }

    IEnumerator BlinkingOn()
    {
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, 0f);
        return FadeText(text, 0f, 1f, 0.5f, legCurve, delegate () {
            StartCoroutine(BlinkingOff());
        });
    }

    IEnumerator BlinkingOff()
    {
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, 1f);
        return FadeText(text, 1f, 0f, 0.5f, legCurve, delegate () {
            StartCoroutine(BlinkingOn());
        });
    }

}
