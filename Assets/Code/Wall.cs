using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject strip;

    private Transform transform;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }

    private void Awake()
    {
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 41 ; i++)
        {
            GameObject obj = Instantiate(strip, transform);
            Transform transformObj = obj.GetComponent<Transform>();
            transformObj.localPosition = new Vector3(-20f + i, 1, 0);
            transformObj.localScale = new Vector3(0.5f, 1, 1);
            transformObj.parent = transform;
            obj.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
