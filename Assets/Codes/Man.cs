using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : Common
{
    public AnimationCurve legCurve;
    public SpriteRenderer legSp;
    public GameObject hair;
    public GameObject hair2;
    public float aniTime;
    public GameObject[] eyebrows;
    public GameObject mouse;

    private void Awake()
    {
        aniTime = 0.7f;
        StartCoroutine(Rotate(20f, 0f));
        AudioManager.instance.PlayBgm(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Rotate(float start, float end)
    {
        return RotateZTransform(legSp.transform, start, end, aniTime, legCurve, delegate () {
            StartCoroutine(RotateBack(end, start));
        });
    }

    IEnumerator RotateBack(float start, float end)
    {
        return RotateZTransform(legSp.transform, start, end, aniTime, legCurve, delegate () {
            StartCoroutine(Rotate(end, start));
        });
    }

}
