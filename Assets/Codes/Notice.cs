using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{
    public AnimationCurve legCurve;
    public bool isLeft;

    private bool isShow = false;

    private void Awake()
    {
        isShow = false;
    }

    public void Show()
    {
        if (!isShow)
        {
            isShow = true;
            if (isLeft)
            {
                StartCoroutine(Move(transform.position.x, 60f, 1f));
            }
            else
            {
                StartCoroutine(Move(transform.position.x, transform.position.x - 120f, 1f));
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
                StartCoroutine(Move(transform.position.x, -60f, 1f));
            }
            else
            {
                StartCoroutine(Move(transform.position.x, transform.position.x + 120f, 1f));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move(float start, float end, float time)
    {
        float during = 0;
        transform.position = new Vector3(start, transform.position.y, transform.position.z);
        while (Mathf.Abs(transform.position.x - end) > 0.1f)
        {
            during += Time.deltaTime;
            float t = legCurve.Evaluate(during / time);    //経過時間(0～1)を渡すとカーブにおける変化量を返してくれる
            transform.position = new Vector3((end - start) * t + start, transform.position.y, transform.position.z);//移動量 * 現在の変化量で、スタート地点からどれくらい移動しているか
            Debug.Log("x:"+ transform.position.x);
            yield return null;
        }
        transform.position = new Vector3(end, transform.position.y, transform.position.z);
    }

}
