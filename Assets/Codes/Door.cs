using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Common
{
    public AnimationCurve curve;

    public void openDoor(Action action)
    {
        StartCoroutine(RotateYTransform(transform, 0f, 80f, 2f, curve, delegate () {
            if (action != null) action();
        }));
    }

    public void closeDoor(Action action)
    {
        StartCoroutine(RotateYTransform(transform, 80f, 0f, 2f, curve, delegate () {
            if (action != null) action();
        }));
    }
}
