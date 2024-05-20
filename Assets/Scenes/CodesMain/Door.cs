using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Common
{
    public AnimationCurve curve;
    public AnimationCurve fadeCurve;
    public SpriteRendererNo no;
    public GameObject door;

    public void OpenDoor(Action action)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.OpenDoor);
        StartCoroutine(RotateYTransform(transform, 0f, 80f, 2f, curve, delegate () {
            if (action != null) action();
        }));
    }

    public void CloseDoor(Action action)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.CloseDoor);
        StartCoroutine(RotateYTransform(transform, 80f, 0f, 2f, curve, delegate () {
            if (action != null) action();
        }));
    }

    public void FadeOut(Action action)
    {
        StartCoroutine(Fade(GetComponent<SpriteRenderer>(), 1f, 0f, 1f, fadeCurve, null));
        StartCoroutine(Fade(door.GetComponent<SpriteRenderer>(), 1f, 0f, 1f, fadeCurve, delegate () {
            if (action != null) action();
            door.SetActive(false);
        }));
    }

}
