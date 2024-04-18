using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject strip;

    public List<GameObject> strips;
    private SpriteRenderer spriteRenderer;

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
