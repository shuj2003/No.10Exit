using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject strip;

    private List<GameObject> strips;
    private SpriteRenderer spriteRenderer;

    // 変化
    private int change_flag = 0;

    public void change_1()
    {
        change_flag = 1;
    }

    void Start()
    {
        
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        strips = new List<GameObject>();
        
        for (int i = 0; i < 41 ; i++)
        {
            GameObject obj = Instantiate(strip, transform);
            strips.Add(obj);
        }

        defauleSetting();

    }

    // Update is called once per frame
    void Update()
    {
        switch (change_flag)
        {
            case 1:// 変化：真ん中のボー段々太くなります。
                {
                    for (int i = 0; i < strips.Count; i++)
                    {
                        var obj = strips[i];
                        Transform tra = obj.GetComponent<Transform>();
                        Vector3 scale = tra.localScale;
                        if (scale.x < 1f)
                        {
                            // 1まであと0.5f,10秒かけて変化します
                            scale.x += (Time.deltaTime * 0.5f) / 10f;
                        }
                        else
                        {
                            scale.x = 1f;
                        }
                        tra.localScale = scale;
                    }
                }
                break;
            default:
                break;
        }

    }

    void defauleSetting()
    {

        for (int i = 0; i < strips.Count; i++)
        {
            var obj = strips[i];
            Transform transformObj = obj.GetComponent<Transform>();
            transformObj.localPosition = new Vector3((-20f + i) / transform.localScale.x, 1, 0);
            transformObj.localScale = new Vector3(0.5f, 1, 1);
            transformObj.parent = transform;
            obj.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder;
            obj.GetComponent<SpriteRenderer>().sortingLayerID = spriteRenderer.sortingLayerID;
        }

    }

}
