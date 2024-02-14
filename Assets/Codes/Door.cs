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

    public void closeDoor()
    {
        StartCoroutine(RotateYTransform(transform, 80f, 0f, 2f, curve, null));
    }
}
