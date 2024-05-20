using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultNotice : Common
{
    public AnimationCurve legCurve;
    public Text text;

    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Show(string cnt)
    {
        text.text = cnt;
        StartCoroutine(MoveAnchoredPosition(rt, rt.anchoredPosition.x, -90f, 1f, legCurve, null));
        StartCoroutine(Wait(delegate () {

            StartCoroutine(MoveAnchoredPosition(rt, rt.anchoredPosition.x, 90f, 1f, legCurve, null));

        }));
    }

    private IEnumerator Wait(Action action)
    {
        yield return new WaitForSeconds(5f);
        if (action != null) action();
    }


}
