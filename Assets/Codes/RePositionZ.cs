using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePositionZ : MonoBehaviour
{
    private SpriteRenderer sp;
    private SpriteRenderer[] spChildren;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        spChildren = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, sp.bounds.min.y);

        for (int i = 1 ; i < spChildren.Length ; i++)
        {
            SpriteRenderer spChild = spChildren[i];
            spChild.transform.position = new Vector3(spChild.transform.position.x, spChild.transform.position.y, transform.position.z - 0.001f);
        }
    }

}
