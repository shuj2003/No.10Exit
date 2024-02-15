using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : Common
{
    public AnimationCurve legCurve;
    public bool isLeft;
    public Button btn;

    private RectTransform rt;
    private bool isShow = false;
    private Coroutine coroutine;

    private void Start()
    {
        isShow = false;
    }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Show()
    {
        if (!isShow)
        {
            isShow = true;
            if (isLeft)
            {
                if(coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(MoveAnchoredPosition(rt, rt.anchoredPosition.x, 60f, 1f, legCurve, null));
            }
            else
            {
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(MoveAnchoredPosition(rt, rt.anchoredPosition.x, -60f, 1f, legCurve, null));
            }
        }
    }

    public void Hide()
    {
        if (isShow)
        {
            isShow = false;
            if (isLeft)
            {
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(MoveAnchoredPosition(rt, rt.anchoredPosition.x, -60f, 1f, legCurve, null));
            }
            else
            {
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(MoveAnchoredPosition(rt, rt.anchoredPosition.x, 60f, 1f, legCurve, null));
            }
        }
    }

}
