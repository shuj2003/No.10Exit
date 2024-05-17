using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Title : Common
{

    public AnimationCurve curve;
    public GameObject wall;

    void Start()
    {

        if (!GameManager.isLive)
        {
            RandomMove();
        }

    }

    void RandomMove()
    {
        Vector2 lb = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 ru = Camera.main.ViewportToWorldPoint(Vector2.one);

        float w = ru.x - lb.x;
        float h = ru.y - lb.y;

        float len = w / 2f;

        Vector2 lb2 = wall.GetComponent<SpriteRenderer>().bounds.min;
        Vector2 ru2 = wall.GetComponent<SpriteRenderer>().bounds.max;

        float x = UnityEngine.Random.Range(lb2.x + w, ru2.x - w);
        float y = UnityEngine.Random.Range(lb2.y + h, ru2.y - h);

        Vector2 start = new Vector2(x, y);
        Vector2 end = start + new Vector2(UnityEngine.Random.Range(0,2) * len * 2 - len, 0);
        StartCoroutine(MoveTransformPosition(transform, start, end, len * 2, curve, delegate ()
        {
            if (!GameManager.isLive)
            {
                transform.position = end;
                RandomMove();
            }
        }));

    }

}
