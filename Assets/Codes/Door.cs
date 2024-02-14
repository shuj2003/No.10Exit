using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Common
{
    public AnimationCurve curve;

    public void openDoor()
    {
        StartCoroutine(RotateYTransform(transform, 0f, 80f, 2f, curve, null));
    }
}
