using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : Common
{
    public AnimationCurve legCurve;
    public SpriteRenderer legSp;

    private void Awake()
    {
        StartCoroutine(Rotate(20f, 0f, 0.7f));
        AudioManager.instance.PlayBgm(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Rotate(float start, float end, float time)
    {
        return RotateZTransform(legSp.transform, start, end, time, legCurve, delegate () {
            StartCoroutine(RotateBack(end, start, time));
        });
    }

    IEnumerator RotateBack(float start, float end, float time)
    {
        return RotateZTransform(legSp.transform, start, end, time, legCurve, delegate () {
            StartCoroutine(Rotate(end, start, time));
        });
    }

}
