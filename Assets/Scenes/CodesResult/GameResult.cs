using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject resultGameOver;
    public GameObject resultGameClear;

    public void ShowResultGameOver()
    {
        resultGameOver.SetActive(true);
        resultGameClear.SetActive(false);
    }

    public void ShowResultGameClear()
    {
        resultGameOver.SetActive(false);
        resultGameClear.SetActive(true);
    }

}
