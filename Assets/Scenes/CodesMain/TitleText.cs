using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleText : MonoBehaviour
{
    public GameObject title10;
    public GameObject titleX;

    public void ShowTitle10()
    {
        title10.SetActive(true);
        titleX.SetActive(false);
    }

    public void ShowTitleX()
    {
        title10.SetActive(false);
        titleX.SetActive(true);
    }
}
