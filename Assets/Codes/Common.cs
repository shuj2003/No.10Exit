using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Common : MonoBehaviour
{
    protected Rect rectTransformtoScreenRect()
    {
        var rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null) return Rect.zero;

        // ワールド空間におけるRectTransformの長方形の角の座標を取得
        // 返される4つの頂点は時計回りに「左下，左上，右上，右下」
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        return new Rect(
            corners[0].x
            , corners[0].y
            , corners[2].x - corners[0].x
            , corners[1].y - corners[0].y
            );
    }

    protected IEnumerator FadeImage(Image img, float start, float end, float time, AnimationCurve curve, Action action)
    {
        Color color = img.color;
        float during = 0;
        img.color = new Color(color.r, color.g, color.b, start);
        while (Mathf.Abs(img.color.a - end) > 0.1f)
        {
            during += Time.deltaTime;
            float t = curve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            img.color = new Color(color.r, color.g, color.b, (end - start) * t + start);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        img.color = new Color(color.r, color.g, color.b, end);
        if (action != null) action();
    }
    protected IEnumerator Fade(SpriteRenderer sp, float start, float end, float time, AnimationCurve curve, Action action)
    {
        Color color = sp.color;
        float during = 0;
        sp.color = new Color(color.r, color.g, color.b, start);
        while (Mathf.Abs(sp.color.a - end) > 0.1f)
        {
            during += Time.deltaTime;
            float t = curve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            sp.color = new Color(color.r, color.g, color.b, (end - start) * t + start);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        sp.color = new Color(color.r, color.g, color.b, end);
        if (action != null) action();
    }

    protected IEnumerator MoveTransformPosition(Transform transform, Vector2 start, Vector2 end, float time, AnimationCurve curve, Action action)
    {
        float during = 0;
        transform.position = start;
        while (Vector2.Distance(transform.position, end) > 0.1f)
        {
            during += Time.deltaTime;
            float t = curve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            transform.position = (end - start) * t + start;//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        transform.position = end;
        if (action != null) action();
    }

    protected IEnumerator MoveAnchoredPosition(RectTransform rt, float start, float end, float time, AnimationCurve curve, Action action)
    {
        float during = 0;
        rt.anchoredPosition = new Vector2(start, rt.anchoredPosition.y);
        while (Mathf.Abs(rt.anchoredPosition.x - end) > 0.1f)
        {
            during += Time.deltaTime;
            float t = curve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            rt.anchoredPosition = new Vector2((end - start) * t + start, rt.anchoredPosition.y);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        rt.anchoredPosition = new Vector2(end, rt.anchoredPosition.y);
        if (action != null) action();
    }

    protected IEnumerator RotateZTransform(Transform transform, float start, float end, float time, AnimationCurve curve, Action action)
    {
        float during = 0;
        transform.rotation = Quaternion.Euler(0, 0, start);
        while (Mathf.Abs((transform.rotation.eulerAngles.z + 360f) % 360f - (end + 360f) % 360f) > 0.1f)
        {
            during += Time.deltaTime;
            float t = curve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            transform.rotation = Quaternion.Euler(0, 0, (end - start) * t + start);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, end);
        if (action != null) action();
    }

    protected IEnumerator RotateYTransform(Transform transform, float start, float end, float time, AnimationCurve curve, Action action)
    {
        float during = 0;
        transform.rotation = Quaternion.Euler(0, start, 0);
        while (Mathf.Abs((transform.rotation.eulerAngles.y + 360f) % 360f - (end + 360f) % 360f) > 0.1f)
        {
            during += Time.deltaTime;
            float t = curve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            transform.rotation = Quaternion.Euler(0, (end - start) * t + start, 0);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, end, 0);
        if (action != null) action();
    }
}
