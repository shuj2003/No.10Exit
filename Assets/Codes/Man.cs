using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    public AnimationCurve legCurve;
    public SpriteRenderer legSp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rotate(30, -15, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Rotate(float start, float end, float time)
    {
        float during = 0;
        legSp.transform.rotation = Quaternion.Euler(0, 0, start);
        while (Mathf.Abs((legSp.transform.rotation.eulerAngles.z + 360f)%360f - (end + 360f) % 360f) > 0.1f)
        {
            during += Time.deltaTime;
            float t = legCurve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            legSp.transform.rotation = Quaternion.Euler(0, 0, (end - start) * t + start);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        legSp.transform.rotation = Quaternion.Euler(0, 0, end);
        StartCoroutine(RotateBack(start, end, time));
    }

    IEnumerator RotateBack(float start, float end, float time)
    {
        float during = time;
        legSp.transform.rotation = Quaternion.Euler(0, 0, end);
        while (Mathf.Abs((legSp.transform.rotation.eulerAngles.z + 360f) % 360f - (start + 360f) % 360f) > 0.1f)
        {
            during -= Time.deltaTime;
            float t = legCurve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            legSp.transform.rotation = Quaternion.Euler(0, 0, (end - start) * t + start);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            yield return null;
        }
        legSp.transform.rotation = Quaternion.Euler(0, 0, start);
        StartCoroutine(Rotate(start, end, time));
    }

}
