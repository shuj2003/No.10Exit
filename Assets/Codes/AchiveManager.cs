using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!PlayerPrefs.HasKey("CollectionComplated"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("CollectionComplated", 0);
    }
}
